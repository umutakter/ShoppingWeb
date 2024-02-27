using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingML
{
    public class Variant
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [Required]
        [StringLength(20)]
        public string Size { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        public virtual Product Product { get; set; }
    }
}
