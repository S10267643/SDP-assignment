using System.Collections.Generic;
using System;

namespace SDP_assignment
{

    public class Order
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; } = DateTime.Now;
        public List<MenuItem> Items { get; set; } = new List<MenuItem>();
        public decimal TotalAmount => Items.Sum(item => item.Price);
        public OrderState State { get; set; } = new PendingPaymentState();
        private PaymentStrategy paymentStrategy;
        private List<OrderObserver> observers = new List<OrderObserver>();

        public void SetPaymentStrategy(PaymentStrategy strategy)
        {
            paymentStrategy = strategy;
        }

        public void AttachObserver(OrderObserver observer)
        {
            observers.Add(observer);
        }

        public void ProcessPayment()
        {
            if (paymentStrategy.ProcessPayment(TotalAmount))
            {
                State = new PaidState();
                NotifyObservers();
            }
        }

        public void RestaurantReject()
        {
            State = new RejectedState();
            paymentStrategy.ProcessRefund(TotalAmount);
            NotifyObservers();
        }

        private void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update(this);
            }
        }
    }
}