using ShoppingML.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Models
{
    internal class Method : CoreDbModel
    {
        public const string TABLE_NAME = "METHOD";

        [CoreKey]
        public int Id { get; set; }
        public int ControllerId { get; set; }
        public string MethodName { get; set; }

    }
}
