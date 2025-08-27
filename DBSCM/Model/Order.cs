public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public List<Product> Products { get; set; }
    public string Status { get; set; }
    public string ReceiptQr { get; set; }
    public List<Transaction> Transactions { get; set; }
}