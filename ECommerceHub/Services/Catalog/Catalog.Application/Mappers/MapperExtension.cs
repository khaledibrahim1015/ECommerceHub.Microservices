using AutoMapper;
using AutoMapper.Internal;

namespace Catalog.Application.Mappers;

public static class MapperExtension
{
    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.ShouldMapProperty = propInfo => propInfo.GetMethod.IsPublic || propInfo.GetMethod.IsAssembly;
            cfg.AddProfile(new ProductMappingProfile());
        });
        var mapper = config.CreateMapper();
        return mapper;

    });

    public static IMapper Mapper => Lazy.Value;

}
