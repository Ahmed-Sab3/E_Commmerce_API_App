using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()

        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.productBrand,o=>o.MapFrom(s=>s.productBrand.Name))
                .ForMember(d=>d.productType,o=>o.MapFrom(s=>s.productType.Name))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();


            CreateMap<AddressDto,Core.Entities.OrderAggregate.Address>();


            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d=>d.DeliveryMethodCost,O=>O.MapFrom(S=>S.DeliveryMethod.Cost));


            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                .ForMember(d=>d.PictureUrl,O=>O.MapFrom<OrderItemPictureUrlResolver>());

        }

    }
}
