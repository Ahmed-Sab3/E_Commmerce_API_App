using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.APIs.Dtos
{
    public class OrderToReturnDto
    {

        public int Id { get; set; }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }

        public string status { get; set; } 
        public Address ShippingAddress { get; set; }

        public int DeliveryMethodId { get; set; }

        //public DeliveryMethod DeliveryMethod { get; set; } //Navigational Prop 1 

        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } 


        public decimal Subtotal { get; set; }
        public string PaymentItentId { get; set; } = string.Empty;

        public decimal Total { get; set; }

    }
}
