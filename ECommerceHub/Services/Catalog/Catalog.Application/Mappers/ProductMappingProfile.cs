﻿using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Mappers
{
    public class ProductMappingProfile :Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductBrand , BrandResponse>().ReverseMap(); 
            CreateMap<ProductType , TypesResponse>().ReverseMap();
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<Product , CreateProductCommand>().ReverseMap();
            CreateMap<Pagination<Product>,Pagination<ProductResponse>>().ReverseMap();  
        }
    }
}
