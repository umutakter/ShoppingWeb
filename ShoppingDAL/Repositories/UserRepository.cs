using ShoppingML;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingDAL.Repositories
{
    public class UserRepository : IRepository<User>
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

        public void Insert(User model)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO Users (Username, Password, Firstname, LastName, Email, Gender) VALUES (@Username, @Password, @Firstname, @LastName, @Email, @Gender)", connection))
                {
                    command.Parameters.AddWithValue("@Username", model.Username);
                    command.Parameters.AddWithValue("@Password", model.Password);
                    command.Parameters.AddWithValue("@Firstname", model.FirstName);
                    command.Parameters.AddWithValue("@LastName", model.LastName);
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@Gender", model.Gender);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IList<User> SelectAll()
        {
            throw new NotImplementedException();
        }

        public User SelectedById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
