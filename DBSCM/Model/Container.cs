public class Container
{
    public int Id { get; set; }
    public string ContainerNumber { get; set; }
    public int VendorId { get; set; }
    public Vendor Vendor { get; set; }
    public List<Product> Products { get; set; }
}