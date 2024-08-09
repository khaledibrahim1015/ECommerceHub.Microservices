namespace Discount.Core.Entities;

[TableName("Coupon")]
public class Coupon
{
    [PrimaryKey(true,1,1)]
    public int Id { get; set; }

    [ColumnName("ProductName")]
    public string ProductName { get; set; }

    [ColumnName("Description")]
    public string Description { get; set; }

    [ColumnName("Amount")]
    public int Amount { get; set; }

    public Coupon() {  }

    public Coupon(int id, string productName, string description, int amount)
    {
        Id = id;
        ProductName = productName;
        Description = description;
        Amount = amount;
    }
}
