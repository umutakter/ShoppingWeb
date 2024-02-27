using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingML
{
    public class Stock
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Variant")]
        public int VariantID { get; set; }

        public int Quantity { get; set; }

        public virtual Variant Variant { get; set; }
    }
}
