using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingML
{
    public class Campaign
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string CampaignName { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal DiscountRate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [ForeignKey("Variant")]
        public int VariantID { get; set; }

        public virtual Variant Variant { get; set; }
    }
}
