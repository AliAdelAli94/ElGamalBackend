using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class CategoryDTO2
    {
        public Guid ID { get; set; }
        public string name { get; set; }
        public Guid? parentCategoryID { get; set; }

        public string parentCategoryName { get; set; }
    }
}
