using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specicfications
{
    public class ProductwithFilterationForCountSpecification :BaseSpecicfication<Product>
    {
        public ProductwithFilterationForCountSpecification(ProductSpecParams specParams)
            :base(P=>
                        (string.IsNullOrEmpty(specParams.Search)||P.Name.ToLower().Contains(specParams.Search))&&
                        (!specParams.brandId.HasValue || P.ProductBrandId == specParams.brandId.Value) &&
                        (!specParams.typeId.HasValue || P.ProductTypeId == specParams.typeId.Value)
            )
        {
                
        }

    }
}
