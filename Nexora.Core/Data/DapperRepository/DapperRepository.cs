using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nexora.Core.Data.EfCoreModels;
using System.Data;

namespace Nexora.Core.Data.DapperRepository
{
    public sealed class DapperRepository(ILogger<DapperRepository> _logger, IConfiguration configuration) : IDapperRepository
    {
        private IDbConnection CreateConnection() => new SqlConnection(configuration.GetConnectionString("ApplicationDbContext"));

        public async Task<OrmResultModel<int>> Execute(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            var result = new OrmResultModel<int>();

            try
            {
                using IDbConnection db = CreateConnection();

                result.Data = await db.ExecuteAsync(sp, parms, commandType: commandType, commandTimeout: commandTimeout);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                result.Error = ex;
            }

            return result;
        }

        public async Task<OrmResultModel<T>> GetAsync<T>(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text)
        {
            var result = new OrmResultModel<T>();

            try
            {
                using IDbConnection db = CreateConnection();

                result.Data = (await db.QueryAsync<T>(sp, parms, commandType: commandType)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                result.Error = ex;
            }

            return result;
        }

        public async Task<OrmResultModel<List<T>>> GetAllAsync<T>(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text)
        {
            var result = new OrmResultModel<List<T>>();

            try
            {
                using IDbConnection db = CreateConnection();

                result.Data = (await db.QueryAsync<T>(sp, parms, commandType: commandType)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                result.Error = ex;
            }

            return result;
        }

        public OrmResultModel<int> Insert(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text)
        {
            var result = new OrmResultModel<int>();

            using IDbConnection db = CreateConnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result.Data = db.Execute(sp, parms, commandType: commandType, transaction: tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _logger.LogError(ex, ex.Message);

                    result.Error = ex;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                result.Error = ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public OrmResultModel<int> Update(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text)
        {
            var result = new OrmResultModel<int>();

            using IDbConnection db = CreateConnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result.Data = db.Execute(sp, parms, commandType: commandType, transaction: tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _logger.LogError(ex, ex.Message);

                    result.Error = ex;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                result.Error = ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public OrmResultModel<int> Delete(string sp, DynamicParameters? parms = null, CommandType commandType = CommandType.Text)
        {
            var result = new OrmResultModel<int>();

            using IDbConnection db = CreateConnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result.Data = db.Execute(sp, parms, commandType: commandType, transaction: tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _logger.LogError(ex, ex.Message);

                    result.Error = ex;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                result.Error = ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public void Dispose()
        {

        }
    }

}
