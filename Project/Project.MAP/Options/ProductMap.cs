using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Options
{
   

    public class ProductMap :BaseMap<Product>
    {
        public ProductMap()
        {
            ToTable("Ürünler");
            Property(x => x.ProductName).HasColumnName("Ürün İsmi").HasMaxLength(150).IsRequired();

            Property(x => x.UnitPrice).HasColumnName("Ürün Fiyatı").HasColumnType("money").IsOptional();

            Property(x => x.UnitsInStock).HasColumnName("Stok Sayısı").IsOptional();

            Property(x => x.CategoryID).HasColumnName("KategoriID").IsOptional();

            Property(x => x.SupplierID).HasColumnName("TedarikciID").IsOptional();
        }
    }
}
