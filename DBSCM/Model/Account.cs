
public class Account
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentAccountId { get; set; }
    public Account ParentAccount { get; set; }
    public List<Account> ChildAccounts { get; set; }
    public string Type { get; set; } // Asset, Liability, Equity, Revenue, Expense
    public List<Transaction> Transactions { get; set; }
}
