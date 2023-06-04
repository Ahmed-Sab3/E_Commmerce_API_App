using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specicfications.OrderSpec
{
    public class OrderSepecification :BaseSpecicfication<Order>
    {
        public OrderSepecification(string Email):base(O=>O.BuyerEmail==Email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            AddOderderByDesc(O => O.OrderDate);
        }


        public OrderSepecification(string Email,int orderID) : base(O => O.BuyerEmail == Email&&O.Id==orderID)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

        }

    }
}
