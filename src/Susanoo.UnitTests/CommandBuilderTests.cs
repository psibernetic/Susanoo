using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Susanoo;
using System.Data;
using System.Data.SqlClient;

namespace Susanoo.UnitTests
{
    [TestFixture]
    public class CommandBuilderTests
    {
        [SetUp]
        public void RegisterDatabaseManager()
        {
            CommandManager.RegisterDatabaseManager(new DatabaseManagerFake());
        }

        [TearDown]
        public void UnRegisterDatabaseManager()
        {
            CommandManager.RegisterDatabaseManager(null);
        }

        [TestCase("commandText1", CommandType.Text)]
        [TestCase("commandText2", CommandType.StoredProcedure)]
        public void DefineCommand_OfTFilterTResult_AssemblesCommand_ContainsCommandInfo(string commandText, CommandType commandType)
        {
            var builder = new CommandBuilder();

            var result = builder.DefineCommand<object, object>(commandText, commandType);

            Assert.IsInstanceOf<ICommandExpression<object, object>>(result);
            Assert.AreEqual(commandText, result.CommandText);
            Assert.AreEqual(commandType, result.DBCommandType);
        }

        [TestCase("commandText1", CommandType.Text)]
        [TestCase("commandText2", CommandType.StoredProcedure)]
        public void DefineCommand_OfTResult_AssemblesCommand_ContainsCommandInfo(string commandText, CommandType commandType)
        {
            var builder = new CommandBuilder();

            var result = builder.DefineCommand<object>(commandText, commandType);

            Assert.IsInstanceOf<ICommandExpression<dynamic, object>>(result);
            Assert.AreEqual(commandText, result.CommandText);
            Assert.AreEqual(commandType, result.DBCommandType);
        }

        [TestCase("commandText", CommandType.TableDirect)]
        public void DefineCommand_OfTFilterTResult_TableDirectCommandType_Throws(string commandText, CommandType commandType)
        {
            var builder = new CommandBuilder();

            var ex = Assert.Catch<ArgumentException>(() => builder.DefineCommand<object, object>(commandText, commandType));

            Assert.AreEqual("commandType", ex.ParamName);
        }

        [TestCase("commandText", CommandType.TableDirect)]
        public void DefineCommand_OfTresult_TableDirectCommandType_Throws(string commandText, CommandType commandType)
        {
            var builder = new CommandBuilder();

            var ex = Assert.Catch<ArgumentException>(() => builder.DefineCommand<object>(commandText, commandType));

            Assert.AreEqual("commandType", ex.ParamName);
        }

        [Test]
        public void DefineCommand_OfTResult_NullCommandText_Throws()
        {
            var builder = new CommandBuilder();

            var ex = Assert.Catch<ArgumentNullException>(() => builder.DefineCommand<object>(null, CommandType.Text));

            Assert.AreEqual("commandText", ex.ParamName);
        }

        [Test]
        public void DefineCommand_OfTResult_EmptyCommandText_Throws()
        {
            var builder = new CommandBuilder();

            var ex = Assert.Catch<ArgumentException>(() => builder.DefineCommand<object>(string.Empty, CommandType.Text));

            Assert.AreEqual("commandText", ex.ParamName);
        }

        [Test]
        public void DefineCommand_OfTResult_WhitespaceForCommandText_Throws()
        {
            var builder = new CommandBuilder();

            var ex = Assert.Catch<ArgumentException>(() => builder.DefineCommand<object>(" ", CommandType.Text));

            Assert.AreEqual("commandText", ex.ParamName);
        }

        [Test]
        public void DefineCommand_OfTFilterTResult_NullCommandText_Throws()
        {
            var builder = new CommandBuilder();

            var ex = Assert.Catch<ArgumentNullException>(() => builder.DefineCommand<object, object>(null, CommandType.Text));

            Assert.AreEqual("commandText", ex.ParamName);
        }

        [Test]
        public void DefineCommand_OfTFilterTResult_EmptyCommandText_Throws()
        {
            var builder = new CommandBuilder();

            var ex = Assert.Catch<ArgumentException>(() => builder.DefineCommand<object, object>(string.Empty, CommandType.Text));

            Assert.AreEqual("commandText", ex.ParamName);
        }

        [Test]
        public void DefineCommand_OfTFilterTResult_WhitespaceForCommandText_Throws()
        {
            var builder = new CommandBuilder();

            var ex = Assert.Catch<ArgumentException>(() => builder.DefineCommand<object, object>(" ", CommandType.Text));

            Assert.AreEqual("commandText", ex.ParamName);
        }

        private class DatabaseManagerFake : IDatabaseManager
        {
            public IDataReader ExecuteDataReader(string commandText, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters)
            {
                throw new NotImplementedException();
            }

            public IDataReader ExecuteDataReader(string commandText, CommandType commandType, params IDbDataParameter[] parameters)
            {
                throw new NotImplementedException();
            }

            public T ExecuteScalar<T>(string commandText, CommandType commandType, IDbTransaction transaction, params IDataParameter[] parameters)
            {
                throw new NotImplementedException();
            }

            public T ExecuteScalar<T>(string commandText, CommandType commandType, params IDataParameter[] parameters)
            {
                throw new NotImplementedException();
            }

            public int ExecuteStoredProcedureNonQuery(string commandText, CommandType commandType, IDbTransaction transaction, params IDbDataParameter[] parameters)
            {
                throw new NotImplementedException();
            }

            public int ExecuteStoredProcedureNonQuery(string commandText, CommandType commandType, params IDbDataParameter[] parameters)
            {
                throw new NotImplementedException();
            }

            public IDbDataParameter CreateParameter()
            {
                throw new NotImplementedException();
            }

            public IDbTransaction BeginTransaction()
            {
                throw new NotImplementedException();
            }

            public IDbDataParameter CreateParameter(string parameterName, ParameterDirection parameterDirection, DbType parameterType, object value)
            {
                throw new NotImplementedException();
            }

            public IDbDataParameter CreateInputParameter(string parameterName, DbType parameterType, object value)
            {
                throw new NotImplementedException();
            }
        }
    }
}