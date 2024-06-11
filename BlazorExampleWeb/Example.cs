using Microsoft.AspNetCore.Components;
using ShoppingDAL.Repositories;
using ShoppingML.DbModels;

namespace BlazorExampleWeb
{
    public class IbbServices : ComponentBase
    {
        public int MyProperty { get; set; }
        public List<User>? users = null;
        public User inputUser = new User();
        public bool showDetails = false;
        protected override async Task OnInitializedAsync()
        {
            LoadUsers();
        }

        public void LoadUsers()
        {
            var repository = new UserRepository();
            users = repository.SelectAll();
        }
        protected void GetDetails(int id)
        {
            
            var repository = new UserRepository();
            inputUser = repository.SelectById(id) ?? new User();
        }
        public void SaveUser()
        {
            var repository = new UserRepository();
            if (inputUser.ID == 0)
            {
                repository.Insert(inputUser);
            }
            else
            {
                repository.Update(inputUser);
            }
            LoadUsers();
            inputUser = new User();
            showDetails = false;
        }

        /// <summary>
        /// Kullanıcının sıcılId'sınden sılme ıslemı yapar.
        /// </summary>
        /// <param name="id"></param>

        public void DeleteUser(int id)
        {
            var repository = new UserRepository();
            repository.DeleteById(id);
            LoadUsers();
        }
    }
}
