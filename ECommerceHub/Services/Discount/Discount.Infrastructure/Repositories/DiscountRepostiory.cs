using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Infrastructure.Configuration;
using System.ComponentModel;
using System.Data.Common;

namespace Discount.Infrastructure.Repositories;

public class DiscountRepostiory : IDiscountRepository
{
    private readonly ConnectionManager _connectionManager;
    public DiscountRepostiory(ConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    private async Task<DbConnection> GetDbConnectionAsync()
     => await _connectionManager.GetConnectionAsync();

    public async Task<Coupon> GetDiscount(string productName)
    {
        DbConnection connection = await GetDbConnectionAsync();
        Coupon? coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                        "SELECT * FROM COUPON WHERE ProductName=@ProductName",
                        new { ProductName = productName }
                        );

        return coupon ?? new Coupon()
        {
            ProductName = "No Discount",
            Amount = 0,
            Description = "No Discount Desc "
        };
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        DbConnection connection = await GetDbConnectionAsync();

        int noOfRowffected = await connection.ExecuteAsync(
                         $@"
                        INSERT INTO {typeof(Coupon).Name}   
                        ( {string.Join(",", typeof(Coupon).GetProperties().Where(prop => prop.Name != "Id").Select(prop => prop.Name))} )
                        Values
                         (@ProductName, @Description, @Amount)
                           " ,new { ProductName  = coupon.ProductName , Description = coupon.Description , Amount=coupon.Amount });

        return noOfRowffected > 0;
        
    }
    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        
        DbConnection connection = await  GetDbConnectionAsync();

        var noOfRowAffected = await connection.ExecuteAsync(
                              "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });
        return noOfRowAffected > 0;

    }
    public async Task<bool> DeleteDiscount(string productName)
    {
        DbConnection connection = await GetDbConnectionAsync();
        var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

        return affected > 0;
    }




}
