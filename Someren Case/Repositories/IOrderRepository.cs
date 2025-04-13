using Someren_Case.Models;

// IOrderRepository.cs
public interface IOrderRepository
{
    void AddOrder(Order order);
    List<Order> GetAllOrders();
}