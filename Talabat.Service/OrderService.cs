using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specicfications.OrderSpec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUntiOfWork _untiOfWork;
        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRepository<Order> _ordersRepo;

        public OrderService(
            IBasketRepository basketRepository
            //,IGenericRepository<Product> productRepo
            //,IGenericRepository<DeliveryMethod> deliveryMethodRepo
            //,IGenericRepository<Order> OrdersRepo
            ,IUntiOfWork untiOfWork)
        {
            _basketRepository = basketRepository;
            _untiOfWork = untiOfWork;
            //_productRepo = productRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
            //_ordersRepo = OrdersRepo;
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1. Get Baskets From Bsket Repo
            var basket = await _basketRepository.GetBasketAsync(basketId);
            //2.Get Selected Items at Basket From Products Repo
            var orderItems = new List<OrderItem>();


            if (basket?.Items.Count > 0) 
            {
                foreach (var item in basket.Items)
                {

                    var productsRepo = _untiOfWork.Repository<Product>();
                    if (productsRepo is not null)
                    {

                        var product = await productsRepo.GetByIdAsync(item.Id);
                        if (product is not null)
                        {

                            var productItemsOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                            var orderItem = new OrderItem(productItemsOrdered, product.Price, item.Quantity);

                            orderItems.Add(orderItem);
                        }

                    }

                }

            }

       
           

            //Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Price* item.Quantity);
            //4.Get Delivery Method
            DeliveryMethod deliveryMethod = new DeliveryMethod();

            var deliveryMethodsRepo = _untiOfWork.Repository<DeliveryMethod>();


            if(deliveryMethodsRepo is not null)
                     deliveryMethod = await deliveryMethodsRepo.GetByIdAsync(deliveryMethodId);


            //5. Create Order
            var order=  new Order(buyerEmail,shippingAddress,deliveryMethod,orderItems,subTotal );
            var ordersRepo = _untiOfWork.Repository<Order>();  
            if (ordersRepo is not null)
            { 
                
                await ordersRepo.Add(order);
                var result = await _untiOfWork.Complete();
                if (result > 0)
                    return order;
            
            
            } 

            return null;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            var deliveryMethod = await _untiOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethod; 
        }

        public async Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSepecification(buyerEmail, orderId);

            var order = await _untiOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            if (order is null) return null;
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {

            var spec = new OrderSepecification(buyerEmail);
            
            var orders = await _untiOfWork.Repository<Order>().GetAllwithSpecAsync(spec);
            return orders;
        }
    }
}
