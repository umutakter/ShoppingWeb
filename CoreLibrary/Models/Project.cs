using ShoppingML.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Models
{
    internal class Project : CoreDbModel
    {
        public const string TABLE_NAME = "PROJECT";

        [CoreKey]
        public int Id { get; set; }
        public string ProjectName { get; set; }
    }
}
