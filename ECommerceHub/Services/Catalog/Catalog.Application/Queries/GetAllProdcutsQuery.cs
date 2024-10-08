﻿using Amazon.Runtime.Internal;
using Catalog.Application.Responses;
using Catalog.Core.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries;

public class GetAllProdcutsQuery :IRequest<Pagination<ProductResponse>>
{
    public CatalogSpecsParams _catalogSpecsParams { get; set; }
    public GetAllProdcutsQuery(CatalogSpecsParams catalogSpecsParams)
    {
        _catalogSpecsParams = catalogSpecsParams;   
    }
}
