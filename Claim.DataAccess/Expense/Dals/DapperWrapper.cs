using System.Data;
using Dapper;

namespace AirCanada.Appx.Claim.DataAccess.Expense.Dals
{
    public class DapperWrapper : IDapperWrapper
    {
        public int Execute(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public T? QueryFirstOrDefault<T>(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
        }
    }
}