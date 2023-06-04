using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed { 
        public static async Task SeedAsync(StoreContext context) 
        {
            if (!context.productBrands.Any())
            {
                var brandsdata = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsdata);

                if (brands is not null && brands.Count > 0)
                {

                    foreach (var brand in brands)
                    {
                        await context.Set<ProductBrand>().AddAsync(brand);
                    }
                    await context.SaveChangesAsync();

                }

            }
            if (!context.productTypes.Any())
            {
                var typesdata = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesdata);

                if (types is not null && types.Count > 0)
                {

                    foreach (var type in types)
                    {
                        await context.Set<ProductType>().AddAsync(type);
                    }
                    await context.SaveChangesAsync();

                }

            }
            if (!context.Products.Any())
            {
                var productsdata = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsdata);

                if (products is not null && products.Count > 0)
                {

                    foreach (var product in products)
                    {
                        await context.Set<Product>().AddAsync(product);
                    }
                    await context.SaveChangesAsync();

                }

            }
            
            if (!context.DeliveryMethods.Any())
            {
                var DeliveryMethodsData= File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);

                if (DeliveryMethodsData is not null && deliveryMethods?.Count > 0)
                {

                    foreach (var deliverMethod in deliveryMethods)
                    {
                        await context.Set<DeliveryMethod>().AddAsync(deliverMethod);
                    }
                    await context.SaveChangesAsync();

                }

            }
        }

    }
}
