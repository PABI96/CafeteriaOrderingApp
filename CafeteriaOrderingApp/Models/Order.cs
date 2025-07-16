namespace CafeteriaOrderingApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
