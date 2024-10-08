﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderDbContextSeed
{

    public static async Task SeedAsync(OrderDbContext orderContext, ILogger<OrderDbContextSeed> logger)
    {
        try
        {
            if (!await orderContext.Orders.AnyAsync())
            {
                logger.LogInformation("Seeding initial data for Orders...");
                await orderContext.Orders.AddRangeAsync(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seeding initial data completed.");
            }
            else
            {
                logger.LogInformation("Database already contains data. Seeding skipped.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>()
        {
            new Order()
            {
                UserName = "khaled",
                FirstName = "khaled",
                LastName = "ibrahim",
                EmailAddress = "khaledIbrahim@eshop.net",
                AddressLine = "23st.",
                Country = "egypt",
                TotalPrice = 750,
                State = "sharkia",
                ZipCode = "560001",

                CardName = "Visa",
                CardNumber = "1234567890123456",
                CreatedBy = "khaled",
                Expiration = "12/25",
                Cvv = "123",
                PaymentMethod = 1,
                LastModifiedBy = "khaled",
                LastModifiedDate = new DateTime(),
            }
        };
    }
}
