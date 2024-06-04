using CoreLibrary;
using CoreLibrary.DbCore;
using CoreLibrary.Logging;
using CoreLibrary.Repository;
using log4net;
using ShoppingML.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingDAL.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private static readonly ILog log = Logger.GetLogger(typeof(UserRepository));
        private readonly string connectionString;
        protected DbConnection? connection;
        public UserRepository()
        {
            this.connectionString = BaseSettings.ConnectionString;
            connection = new SqlConnection(connectionString);
        }
        public void Dispose()
        {
            try
            {
                if (connection != null)
                    connection.Dispose();
            }
            catch (Exception ex)
            {
                log.Error($"ShoppingDAL:Repositories::UserRepository::Dispose::Error occured.", ex);
                throw new Exception($"ShoppingDAL:Repositories::UserRepository::Dispose::Error occured.", ex);
            }
        }
    }
}
