﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Susanoo
{
    /// <summary>
    /// Susanoo's initial step in the command definition Fluent API, in which parameters and command information are provided.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class CommandExpression<TFilter, TResult>
        : ICommandExpression<TFilter, TResult>
        where TResult : new()
    {
        private readonly IDictionary<string, Action<IDbDataParameter>> parameterInclusions = new Dictionary<string, Action<IDbDataParameter>>();
        private readonly List<IDbDataParameter> constantParameters = new List<IDbDataParameter>();

        private object syncRoot = new object();

        private IList<IDbDataParameter> _PropertyCache = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExpression{TFilter, TResult}" /> class.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        public CommandExpression(IDatabaseManager databaseManager, string commandText, CommandType commandType)
        {
            this.DatabaseManager = databaseManager;
            this.CommandText = commandText;
            this.DBCommandType = commandType;
        }

        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <value>The command text.</value>
        public virtual string CommandText { get; private set; }

        /// <summary>
        /// Gets the type of the database command.
        /// </summary>
        /// <value>The type of the database command.</value>
        public virtual CommandType DBCommandType { get; private set; }

        /// <summary>
        /// Gets the database manager.
        /// </summary>
        /// <value>The database manager.</value>
        public virtual IDatabaseManager DatabaseManager { get; private set; }

        private IEnumerable<IDbDataParameter> PropertyParameterCache
        {
            get
            {
                return _PropertyCache;
            }
        }

        /// <summary>
        /// Includes the property of the filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>ICommandExpression&lt;TFilter, TResult&gt;.</returns>
        public virtual ICommandExpression<TFilter, TResult> IncludeProperty(Expression<Func<TFilter, object>> propertyExpression)
        {
            return IncludeProperty(propertyExpression, null);
        }

        /// <summary>
        /// Includes a property of the filter or modifies its inclusion.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="parameterOptions">The parameter options.</param>
        /// <returns>ICommandExpression&lt;T&gt;.</returns>
        public virtual ICommandExpression<TFilter, TResult> IncludeProperty(Expression<Func<TFilter, object>> propertyExpression, Action<IDbDataParameter> parameterOptions)
        {
            return IncludeProperty(propertyExpression.GetPropertyName(), parameterOptions);
        }

        /// <summary>
        /// Includes the property of the filter.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>ICommandExpression&lt;TFilter, TResult&gt;.</returns>
        public virtual ICommandExpression<TFilter, TResult> IncludeProperty(string propertyName)
        {
            return IncludeProperty(propertyName, null);
        }

        /// <summary>
        /// Includes a property of the filter or modifies its inclusion.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="parameterOptions">The parameter options.</param>
        /// <returns>ICommandExpression&lt;T&gt;.</returns>
        public virtual ICommandExpression<TFilter, TResult> IncludeProperty(string propertyName, Action<IDbDataParameter> parameterOptions)
        {
            //TODO: Check for existing parameter inclusion and modify instead.
            this.parameterInclusions.Add(propertyName, parameterOptions);

            return this;
        }

        /// <summary>
        /// Adds parameters that will always use the same value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>ICommandExpression&lt;T&gt;.</returns>
        public virtual ICommandExpression<TFilter, TResult> AddConstantParameters(params IDbDataParameter[] parameters)
        {
            this.constantParameters.AddRange(parameters);

            return this;
        }

        /// <summary>
        /// Excludes a property of the filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>ICommandExpression&lt;TFilter, TResult&gt;.</returns>
        public virtual ICommandExpression<TFilter, TResult> ExcludeProperty(Expression<Func<TFilter, object>> propertyExpression)
        {
            return ExcludeProperty(propertyExpression.GetPropertyName());
        }

        /// <summary>
        /// Excludes a property of the filter.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>ICommandExpression&lt;TFilter, TResult&gt;.</returns>
        public virtual ICommandExpression<TFilter, TResult> ExcludeProperty(string propertyName)
        {
            if (this.parameterInclusions.ContainsKey(propertyName))
            {
                this.parameterInclusions.Remove(propertyName);
            }

            return this;
        }

        /// <summary>
        /// Defines the result mappings (Moves to next Step in Fluent API).
        /// </summary>
        /// <returns>ICommandResultMappingExpression&lt;TFilter, TResult&gt;.</returns>
        public virtual ICommandResultMappingExpression<TFilter, TResult> DefineResultMappings()
        {
            return new CommandResultMappingExpression<TFilter, TResult>(this);
        }

        /// <summary>
        /// Builds the parameters (Not part of Fluent API).
        /// </summary>
        /// <returns>IEnumerable&lt;IDbDataParameter&gt;.</returns>
        public virtual IDbDataParameter[] BuildParameters(TFilter filter, params IDbDataParameter[] explicitParameters)
        {
            IDbDataParameter[] parameters = null;

            int parameterCount = 0;

            IEnumerable<IDbDataParameter> propertyParameters = BuildPropertyParameters(filter);

            parameterCount = (propertyParameters.Count() + this.constantParameters.Count) + ((explicitParameters != null) ? explicitParameters.Count() : 0);
            parameters = new IDbDataParameter[parameterCount];

            int i = 0;
            foreach (var item in propertyParameters)
            {
                parameters[i] = item;
                i++;
            }

            foreach (var item in this.constantParameters)
            {
                parameters[i] = item;
                i++;
            }

            if (explicitParameters != null)
                foreach (var item in explicitParameters)
                {
                    parameters[i] = item;
                    i++;
                }

            return parameters;
        }

        /// <summary>
        /// Builds the property inclusion parameters.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>IEnumerable&lt;IDbDataParameter&gt;.</returns>
        public virtual IEnumerable<IDbDataParameter> BuildPropertyParameters(TFilter filter)
        {
            var properties = new List<IDbDataParameter>();

            foreach (var item in this.parameterInclusions)
            {
                var propInfo = typeof(TFilter).GetProperty(item.Key, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                var param = this.DatabaseManager.CreateParameter();

                param.ParameterName = item.Key;
                param.Direction = ParameterDirection.Input;

                if (item.Value != null)
                    item.Value.Invoke(param);

                param.Value = propInfo.GetValue(filter);

                properties.Add(param);
            }

            return properties;
        }
    }

    public class CommandExpression<TResult>
        : ICommandExpression<TResult>
        where TResult : new()
    {
        private readonly IDictionary<string, Action<IDbDataParameter>> parameterInclusions = new Dictionary<string, Action<IDbDataParameter>>();
        private readonly List<IDbDataParameter> constantParameters = new List<IDbDataParameter>();

        private object syncRoot = new object();

        private IList<IDbDataParameter> _PropertyCache = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExpression{TFilter, TResult}" /> class.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        public CommandExpression(IDatabaseManager databaseManager, string commandText, CommandType commandType)
        {
            this.DatabaseManager = databaseManager;
            this.CommandText = commandText;
            this.DBCommandType = commandType;
        }

        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <value>The command text.</value>
        public virtual string CommandText { get; private set; }

        /// <summary>
        /// Gets the type of the database command.
        /// </summary>
        /// <value>The type of the database command.</value>
        public virtual CommandType DBCommandType { get; private set; }

        /// <summary>
        /// Gets the database manager.
        /// </summary>
        /// <value>The database manager.</value>
        public virtual IDatabaseManager DatabaseManager { get; private set; }

        private IEnumerable<IDbDataParameter> PropertyParameterCache
        {
            get
            {
                return _PropertyCache;
            }
        }

        /// <summary>
        /// Adds parameters that will always use the same value.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>ICommandExpression&lt;T&gt;.</returns>
        public virtual ICommandExpression<TResult> AddConstantParameters(params IDbDataParameter[] parameters)
        {
            this.constantParameters.AddRange(parameters);

            return this;
        }

        /// <summary>
        /// Defines the result mappings (Moves to next Step in Fluent API).
        /// </summary>
        /// <returns>ICommandResultMappingExpression&lt;TFilter, TResult&gt;.</returns>
        public virtual ICommandResultMappingExpression<TResult> DefineResultMappings()
        {
            return new CommandResultMappingExpression<dynamic, TResult>(this);
        }

        /// <summary>
        /// Builds the parameters (Not part of Fluent API).
        /// </summary>
        /// <returns>IEnumerable&lt;IDbDataParameter&gt;.</returns>
        public virtual IDbDataParameter[] BuildParameters(dynamic filter, params IDbDataParameter[] explicitParameters)
        {
            IDbDataParameter[] parameters = null;

            int parameterCount = 0;

            IEnumerable<IDbDataParameter> propertyParameters = BuildPropertyParameters(filter);

            parameterCount = (propertyParameters.Count() + this.constantParameters.Count) + ((explicitParameters != null) ? explicitParameters.Count() : 0);
            parameters = new IDbDataParameter[parameterCount];

            int i = 0;
            foreach (var item in propertyParameters)
            {
                parameters[i] = item;
                i++;
            }

            foreach (var item in this.constantParameters)
            {
                parameters[i] = item;
                i++;
            }

            if (explicitParameters != null)
                foreach (var item in explicitParameters)
                {
                    parameters[i] = item;
                    i++;
                }

            return parameters;
        }

        /// <summary>
        /// Builds the property inclusion parameters.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>IEnumerable&lt;IDbDataParameter&gt;.</returns>
        public virtual IEnumerable<IDbDataParameter> BuildPropertyParameters(dynamic filter)
        {
            var properties = new List<IDbDataParameter>();

            if (filter != null)
            {

                foreach (PropertyInfo propInfo in filter.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
                {
                    var param = this.DatabaseManager.CreateParameter();

                    param.ParameterName = propInfo.Name;
                    param.Direction = ParameterDirection.Input;

                    //if (item.Value != null)
                    //    item.Value.Invoke(param);

                    param.Value = propInfo.GetValue(filter);

                    properties.Add(param);
                }
            }

            return properties;
        }


        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>IEnumerable&lt;IDbDataParameter&gt;.</returns>
        public IEnumerable<IDbDataParameter> BuildParameters(params IDbDataParameter[] explicitParameters)
        {
            return this.BuildParameters(null, explicitParameters);
        }
    }
}