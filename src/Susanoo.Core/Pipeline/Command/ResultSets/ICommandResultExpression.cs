﻿#region

using System;
using System.Collections.Generic;
using Susanoo.Pipeline.Command.ResultSets.Mapping;
using Susanoo.Pipeline.Command.ResultSets.Mapping.Properties;
using Susanoo.Pipeline.Command.ResultSets.Processing;

#endregion

namespace Susanoo.Pipeline.Command.ResultSets
{
    /// <summary>
    /// Shared components for Command Result Expressions.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    public interface ICommandResultExpressionCore<TFilter>
        : ICommandResultMappingExport, IFluentPipelineFragment
    {
        /// <summary>
        /// Gets the command expression.
        /// </summary>
        /// <value>The command expression.</value>
        ICommandExpression<TFilter> CommandExpression { get; }

        /// <summary>
        /// Exports a results mappings for processing.
        /// </summary>
        /// <typeparam name="TResultType">The type of the result type.</typeparam>
        /// <returns>IDictionary&lt;System.String, IPropertyMapping&gt;.</returns>
        IDictionary<string, IPropertyMapping> Export<TResultType>()
            where TResultType : new();



        /// <summary>
        /// Converts to a single result expression.
        /// </summary>
        /// <typeparam name="TSingle">The type of the single.</typeparam>
        /// <returns>ICommandResultExpression&lt;TFilter, TResult&gt;.</returns>
        ICommandResultExpression<TFilter, TSingle> ToSingleResult<TSingle>()
            where TSingle : new();
    }
    /// <summary>
    /// Exposes property mapping export capabilities.
    /// </summary>
    public interface ICommandResultMappingExport : IFluentPipelineFragment
    {
        /// <summary>
        /// Exports a results mappings for processing.
        /// </summary>
        /// <param name="resultType">Type of the result.</param>
        /// <returns>IDictionary&lt;System.String, IPropertyMapping&gt;.</returns>
        IDictionary<string, IPropertyMapping> Export(Type resultType);
    }

    /// <summary>
    /// Provides methods for customizing how results are handled and compiling result mappings.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface ICommandResultExpression<TFilter, TResult> : ICommandResultExpressionCore<TFilter>
        where TResult : new()
    {
        /// <summary>
        /// Allows customization of result set.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        /// <returns>ICommandResultExpression&lt;TFilter, TResult&gt;.</returns>
        ICommandResultExpression<TFilter, TResult> ForResults(
            Action<IResultMappingExpression<TFilter, TResult>> mappings);

        /// <summary>
        /// Realizes the pipeline and compiles result mappings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ICommandProcessor&lt;TFilter, TResult&gt;.</returns>
        ICommandProcessor<TFilter, TResult> Realize(string name = null);
    }

    /// <summary>
    /// Provides methods for customizing how results are handled and compiling result mappings.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    public interface ICommandResultExpression<TFilter, TResult1, TResult2> : ICommandResultExpressionCore<TFilter>
        where TResult1 : new()
        where TResult2 : new()
    {
        /// <summary>
        /// Allows customization of a type of result set.
        /// </summary>
        /// <typeparam name="TResultType">The type of the result type.</typeparam>
        /// <param name="mappings">The mappings.</param>
        /// <returns>ICommandResultExpression&lt;TFilter, TResult1, TResult2&gt;.</returns>
        ICommandResultExpression<TFilter, TResult1, TResult2> ForResultsOfType<TResultType>(
            Action<IResultMappingExpression<TFilter, TResultType>> mappings)
            where TResultType : new();

        /// <summary>
        /// Realizes the pipeline and compiles result mappings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ICommandProcessor&lt;TFilter, TResult1, TResult2&gt;.</returns>
        ICommandProcessor<TFilter, TResult1, TResult2> Realize(string name = null);
    }

    /// <summary>
    /// Provides methods for customizing how results are handled and compiling result mappings.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    /// <typeparam name="TResult3">The type of the 3rd result.</typeparam>
    public interface ICommandResultExpression<TFilter, TResult1, TResult2, TResult3> :
        ICommandResultExpressionCore<TFilter>
        where TResult1 : new()
        where TResult2 : new()
        where TResult3 : new()
    {
        /// <summary>
        /// Allows customization of a type of result set.
        /// </summary>
        /// <typeparam name="TResultType">The type of the result type.</typeparam>
        /// <param name="mappings">The mappings.</param>
        /// <returns>ICommandResultExpression&lt;TFilter, TResult1, TResult2, TResult3&gt;.</returns>
        ICommandResultExpression<TFilter, TResult1, TResult2, TResult3> ForResultsOfType<TResultType>(
            Action<IResultMappingExpression<TFilter, TResultType>> mappings)
            where TResultType : new();

        /// <summary>
        /// Realizes the pipeline and compiles result mappings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ICommandProcessor&lt;TFilter, TResult1, TResult2, TResult3&gt;.</returns>
        ICommandProcessor<TFilter, TResult1, TResult2, TResult3> Realize(string name = null);
    }

    /// <summary>
    /// Provides methods for customizing how results are handled and compiling result mappings.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    /// <typeparam name="TResult3">The type of the 3rd result.</typeparam>
    /// <typeparam name="TResult4">The type of the 4th result.</typeparam>
    public interface ICommandResultExpression<TFilter, TResult1, TResult2, TResult3, TResult4> :
        ICommandResultExpressionCore<TFilter>
        where TResult1 : new()
        where TResult2 : new()
        where TResult3 : new()
        where TResult4 : new()
    {
        /// <summary>
        /// Allows customization of a type of result set.
        /// </summary>
        /// <typeparam name="TResultType">The type of the result type.</typeparam>
        /// <param name="mappings">The mappings.</param>
        /// <returns>ICommandResultExpression&lt;TFilter, TResult1, TResult2, TResult3, TResult4&gt;.</returns>
        ICommandResultExpression<TFilter, TResult1, TResult2, TResult3, TResult4> ForResultsOfType<TResultType>(
            Action<IResultMappingExpression<TFilter, TResultType>> mappings)
            where TResultType : new();

        /// <summary>
        /// Realizes the pipeline and compiles result mappings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ICommandProcessor&lt;TFilter, TResult1, TResult2, TResult3, TResult4&gt;.</returns>
        ICommandProcessor<TFilter, TResult1, TResult2, TResult3, TResult4> Realize(string name = null);
    }

    /// <summary>
    /// Provides methods for customizing how results are handled and compiling result mappings.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    /// <typeparam name="TResult3">The type of the 3rd result.</typeparam>
    /// <typeparam name="TResult4">The type of the 4th result.</typeparam>
    /// <typeparam name="TResult5">The type of the 5th result.</typeparam>
    public interface ICommandResultExpression<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5> :
        ICommandResultExpressionCore<TFilter>
        where TResult1 : new()
        where TResult2 : new()
        where TResult3 : new()
        where TResult4 : new()
        where TResult5 : new()
    {
        /// <summary>
        /// Allows customization of a type of result set.
        /// </summary>
        /// <typeparam name="TResultType">The type of the result type.</typeparam>
        /// <param name="mappings">The mappings.</param>
        /// <returns>ICommandResultExpression&lt;TFilter, TResult1, TResult2, TResult3, TResult4, TResult5&gt;.</returns>
        ICommandResultExpression<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5> ForResultsOfType
            <TResultType>(
            Action<IResultMappingExpression<TFilter, TResultType>> mappings)
            where TResultType : new();

        /// <summary>
        /// Realizes the pipeline and compiles result mappings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ICommandProcessor&lt;TFilter, TResult1, TResult2, TResult3, TResult4, TResult5&gt;.</returns>
        ICommandProcessor<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5> Realize(string name = null);
    }

    /// <summary>
    /// Provides methods for customizing how results are handled and compiling result mappings.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    /// <typeparam name="TResult3">The type of the 3rd result.</typeparam>
    /// <typeparam name="TResult4">The type of the 4th result.</typeparam>
    /// <typeparam name="TResult5">The type of the 5th result.</typeparam>
    /// <typeparam name="TResult6">The type of the 6th result.</typeparam>
    public interface ICommandResultExpression<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> :
        ICommandResultExpressionCore<TFilter>
        where TResult1 : new()
        where TResult2 : new()
        where TResult3 : new()
        where TResult4 : new()
        where TResult5 : new()
        where TResult6 : new()
    {
        /// <summary>
        /// Allows customization of a type of result set.
        /// </summary>
        /// <typeparam name="TResultType">The type of the result type.</typeparam>
        /// <param name="mappings">The mappings.</param>
        /// <returns>ICommandResultExpression&lt;TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6&gt;.</returns>
        ICommandResultExpression<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> ForResultsOfType
            <TResultType>(
            Action<IResultMappingExpression<TFilter, TResultType>> mappings)
            where TResultType : new();

        /// <summary>
        /// Realizes the pipeline and compiles result mappings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ICommandProcessor&lt;TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6&gt;.</returns>
        ICommandProcessor<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> Realize(
            string name = null);
    }

    /// <summary>
    /// Provides methods for customizing how results are handled and compiling result mappings.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    /// <typeparam name="TResult3">The type of the 3rd result.</typeparam>
    /// <typeparam name="TResult4">The type of the 4th result.</typeparam>
    /// <typeparam name="TResult5">The type of the 5th result.</typeparam>
    /// <typeparam name="TResult6">The type of the 6th result.</typeparam>
    /// <typeparam name="TResult7">The type of the 7th result.</typeparam>
    public interface ICommandResultExpression<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6,
        TResult7> : ICommandResultExpressionCore<TFilter>
        where TResult1 : new()
        where TResult2 : new()
        where TResult3 : new()
        where TResult4 : new()
        where TResult5 : new()
        where TResult6 : new()
        where TResult7 : new()
    {
        /// <summary>
        /// Allows customization of a type of result set.
        /// </summary>
        /// <typeparam name="TResultType">The type of the result type.</typeparam>
        /// <param name="mappings">The mappings.</param>
        /// <returns>ICommandResultExpression&lt;TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7&gt;.</returns>
        ICommandResultExpression<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>
            ForResultsOfType<TResultType>(
            Action<IResultMappingExpression<TFilter, TResultType>> mappings)
            where TResultType : new();

        /// <summary>
        /// Realizes the pipeline and compiles result mappings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ICommandProcessor&lt;TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7&gt;.</returns>
        ICommandProcessor<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> Realize(
            string name = null);
    }

    /// <summary>
    /// Extension methods for ICommandResultExpressions
    /// </summary>
    public static class CommandResultExtensions
    {

        /// <summary>
        /// Gets a matching processor.
        /// </summary>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="resultExpression">The result expression.</param>
        /// <returns>IResultMapper&lt;TResult&gt;.</returns>
        public static IResultMapper<TResult> GetProcessor<TFilter, TResult>(this
            ICommandResultExpressionCore<TFilter> resultExpression)
            where TResult : new()
        {
            return SingleResultSetCommandProcessor<TFilter, TResult>.BuildOrRegenResultMapper(
                resultExpression.ToSingleResult<TResult>());
        }
    }
}
