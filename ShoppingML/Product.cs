using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingML
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public string Description { get; set; }
    }
}
