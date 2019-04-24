using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MODEL.Entities
{
   public class Supplier:BaseEntity
    {
        public Supplier()
        {
            Products = new List<Product>();
        }
        public string CompanyName { get; set; }

        public string Contact { get; set; }

        

        public virtual List<Product> Products { get; set; }


    }
}
