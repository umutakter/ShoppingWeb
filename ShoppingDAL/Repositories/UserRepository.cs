using ShoppingML;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingDAL.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private readonly string connectionString;
        private SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
        public UserRepository()
        {
            this.connectionString = "Server=DESKTOP-VPPU1BG;Database=ShoppingDb;Integrated Security=True;";
        }
        public bool DeletedById(int id)
        {
            throw new NotImplementedException();
        }
        public IList<User> SelectAll()
        {
            throw new NotImplementedException();
        }
        public User SelectedById(int id)
        {
            throw new NotImplementedException();
        }

    }
}
