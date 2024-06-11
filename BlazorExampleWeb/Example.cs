using CoreLibrary;
using Microsoft.AspNetCore.Components;
using ShoppingDAL.Models;
using ShoppingDAL.Repositories;

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
            var repository = new BaseRepository();
            users = repository.SelectAll<User>();
        }
        protected void GetDetails(int id)
        {
            
            var repository = new BaseRepository();
            inputUser = repository.SelectById<User>(id) ?? new User();
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
            repository.DeleteById<User>(id);
            LoadUsers();
        }
    }
}
