using CoreLibrary.DbCore;
using CoreLibrary.Logging;
using CoreLibrary.Models;
using CoreLibrary.Repository;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CoreLibrary
{


    public class BaseRepository : IBaseRepository
    {
        public static readonly ILog log = Logger.GetLogger(typeof(BaseRepository));
        private readonly string connectionString;
        protected DbConnection? connection;
        private DbTransaction? transaction;
        public BaseRepository()
        {
            this.connectionString = BaseDbSettings.ConnectionString;
            connection = new SqlConnection(connectionString);
        }
        public void Dispose()
        {
            try
            {
                connection?.Dispose();
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::Dispose::Error occured.", ex);
            }
        }

        #region GENERIC CRUD OPERATIONS
        public bool Update<T>(T model) where T : CoreDbModel
        {
            try
            {
                using (var command = DbCommandGenerator.GetUpdateCommand(model))
                {
                    return DbExecutor.UpdateExecutor(command);
                }
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::{nameof(T)}::Update::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::{nameof(T)}::Update::Error occured.", ex);
            }

        }
        public int Insert<T>(T model) where T : CoreDbModel
        {
            try
            {
                using (var command = DbCommandGenerator.GetInsertCommand(model))
                {
                    return DbExecutor.InsertExecutor<T>(command);
                }
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::{nameof(T)}::Insert::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::{nameof(T)}::Insert::Error occured.", ex);
            }
        }
        public List<T> SelectAll<T>() where T: CoreDbModel
        {
            try
            {
                using (var command = DbCommandGenerator.GetSelectAllCommand<T>())
                {
                    return DbExecutor.SelectExecutor<T>(command);
                }
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::{nameof(T)}::SelectAll::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::{nameof(T)}::SelectAll::Error occured.", ex);
            }
        }
        public T SelectById<T>(int id) where T : CoreDbModel
        {
            try
            {
                using (var command = DbCommandGenerator.GetSelectByIdCommand<T>(id))
                {
                    
                    return DbExecutor.SelectExecutor<T>(command).FirstOrDefault()!;
                }
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::{nameof(T)}::SelectById::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::{nameof(T)}::SelectById::Error occured.", ex);
            }
        }
        public void DeleteById<T>(int id) where T : CoreDbModel
        {
            try
            {
                using (var command = DbCommandGenerator.GetDeleteByIdCommand<T>(id))
                {
                    DbExecutor.DeleteExecutor(command);
                }
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::{nameof(T)}::DeleteById::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::{nameof(T)}::DeleteById::Error occured.", ex);
            }

        }
        public T GetSelectAllExceptKey<T>(T model) where T : CoreDbModel
        {
            try
            {
                using(var command = DbCommandGenerator.GetSelectAllExceptKeyCommand(model))
                {
                   return  DbExecutor.SelectExecutor<T>(command).FirstOrDefault()!;
                }
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::{nameof(T)}::GetSelectAllExceptKey::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::{nameof(T)}::GetSelectAllExceptKey::Error occured.", ex);
            }
        }
        #endregion
        #region TRANSACTIONS
        public void BeginTransaction()
        {
            if (connection == null)
                connection = new SqlConnection(connectionString);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            transaction = connection.BeginTransaction();
        }
        public void CommitTransaction()
        {
            try
            {
                transaction?.Commit();
            }
            finally
            {
                transaction = null;
                connection?.Close();
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                transaction?.Rollback();
            }
            finally
            {
                transaction = null;
                connection?.Close();
            }
        }

        #endregion
    }
}