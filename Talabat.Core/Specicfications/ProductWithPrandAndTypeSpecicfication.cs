using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specicfications
{
    public class ProductWithPrandAndTypeSpecicfication:BaseSpecicfication<Product>
    {  //this Cons is used to get all Products
        public ProductWithPrandAndTypeSpecicfication(ProductSpecParams specParams)
            : base(P =>
                        (string.IsNullOrEmpty(specParams.Search)||P.Name.ToLower().Contains(specParams.Search))&&
                        (!specParams.brandId.HasValue || P.ProductBrandId == specParams.brandId.Value)&&
                        (!specParams.typeId.HasValue ||P.ProductTypeId== specParams.typeId.Value)
            )
        {
            Includes.Add(p => p.productBrand);
            Includes.Add(p => p.productType);

            AddOderderBy(P => P.Name);

            if (!string.IsNullOrEmpty(specParams.sort))
            {
                switch (specParams.sort) 
                {
                    case "priceAsc":
                        AddOderderBy(P=>P.Price);
                        break;
                    case "priceDesc":
                        AddOderderByDesc(P => P.Price);
                        break;

                    default:
                        AddOderderBy(P => P.Name); 
                        break; 
                        
                
                
                }
            }

            ApplyPagination(specParams.PageSize*(specParams.PageIndex-1),specParams.PageSize); 

        }

        //this cons used for get specific product
        public ProductWithPrandAndTypeSpecicfication(int id):base(p=>p.Id==id)
        {
            Includes.Add(p => p.productBrand);
            Includes.Add(p => p.productType);

        }

    }
}
