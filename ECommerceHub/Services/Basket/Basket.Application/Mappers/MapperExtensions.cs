using AutoMapper;

namespace Basket.Application.Mappers;

public  class MapperExtensions
{
    private static readonly Lazy<IMapper> _lazyMapper = new Lazy<IMapper>(() =>
    {
        return new MapperConfiguration(mapConfig =>
         {

             mapConfig.ShouldMapProperty = (propInfo) => propInfo.GetMethod.IsPublic || propInfo.GetMethod.IsAssembly;
             mapConfig.AddProfile(new BasketMappingProfile());
         }).CreateMapper();
    });

    public static IMapper Mapper => _lazyMapper.Value;



}
