using ShoppingML.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Models
{
    internal class Controller : CoreDbModel
    {
        public const string TABLE_NAME = "CONTROLLER";

        [CoreKey]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ControllerName { get; set; }
    }
}
