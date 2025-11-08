using Dapper;
using Nexora.Core.Data.EfCoreModels;
using System.Data;

namespace Nexora.Core.Data.DapperRepository
{
    public interface IDapperRepository
    {
        Task<OrmResultModel<int>> Execute(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text, int? commandTimeout = null);

        Task<OrmResultModel<T>> GetAsync<T>(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text);

        Task<OrmResultModel<List<T>>> GetAllAsync<T>(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text);

        OrmResultModel<int> Insert(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text);

        OrmResultModel<int> Update(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text);

        OrmResultModel<int> Delete(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text);
    }
}