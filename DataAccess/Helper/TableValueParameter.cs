using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess.Helpers
{
    public class TableValueParameter : SqlMapper.ICustomQueryParameter
    {
        private readonly DataTable dataTable;

        public TableValueParameter(DataTable _dataTable)
        {
            dataTable = _dataTable;
        }

        public void AddParameter(IDbCommand command, string name)
        {
            var parameter = (SqlParameter)command.CreateParameter();

            parameter.ParameterName = name;
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.Value = dataTable;
            parameter.TypeName = dataTable.TableName;

            command.Parameters.Add(parameter);
        }
    }
}
