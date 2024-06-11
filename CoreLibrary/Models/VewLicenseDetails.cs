using ShoppingML.Attributes;

namespace CoreLibrary.Models
{
    public class VewLicenseDetails: CoreDbModel
    {
        public const string TABLE_NAME = "VEW_LICENSE_DETAILS";

        [CoreKey]
        public int ID { get; set; }
        public string LicenseKey { get; set; }
        public int UserId { get; set; }
        public DateTime ExpireDate { get; set; }
        public string CombinedName { get; set; }
        public int ExpireIn { get; set; }
    }
}
