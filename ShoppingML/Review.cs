using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingML
{
    public class Review
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public int Rating { get; set; }

        [StringLength(250)]
        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
