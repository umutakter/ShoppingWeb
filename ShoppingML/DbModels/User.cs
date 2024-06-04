using CoreLibrary.Models;
using ShoppingML.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ShoppingML.DbModels
{
    public class User : CoreDbModel
    {
        public const string TABLE_NAME = "Users";

        [CoreKey]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }
    }
}
