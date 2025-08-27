
public class Transaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; }
    public int? CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int? OrderId { get; set; }
    public Order Order { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}
