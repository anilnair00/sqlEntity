using System.Data;

namespace AirCanada.Appx.Claim.DataAccess.Expense.Dals
{
    public interface IDapperWrapper
    {
        int Execute(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        T? QueryFirstOrDefault<T>(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}