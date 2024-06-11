using CoreLibrary.DbCore;
using CoreLibrary.Models;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Repository
{
    internal class LicenseRepository : BaseRepository
    {
        public List<VewLicenseDetails> GetLicensePermissions(string licenseKey)
        {
            try
            {
                string tableName = DbHelpers.GetTableName<VewLicenseDetails>();
                var query = $"SELECT * FROM {tableName} WHERE LicenseKey = @licenseKey";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Parameters.AddWithValue($"@licenseKey", licenseKey);
                    return DbExecutor.SelectExecutor<VewLicenseDetails>(command);
                }
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::LicenseRepository::GetLicensePermissions::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::LicenseRepository::GetLicensePermissions::Error occured.", ex);
            }
        }
    }
}
