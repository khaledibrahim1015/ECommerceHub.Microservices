using Discount.Grpc.Protos;

namespace Basket.Application.DiscountService;

public class DiscountGrpcService
{
    public DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<CouponModel> GetDiscount(string productName)
    {
        var discountrequest= new GetDiscountRequest { ProductName = productName };
       return  await _discountProtoServiceClient.GetDiscountAsync(discountrequest);
    }
}
