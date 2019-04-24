using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.TOOLS.SpecIinit
{
   public class InitSpec<T>:CreateDatabaseIfNotExists<T> where T:DbContext
    {
        
        protected override void Seed(T context)
        {
            Category item = new Category();
            context.Set<Category>().Add(item);
        }
    }
}
