using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ShoppingML.DbModels;

namespace ShoppingML.Models
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
