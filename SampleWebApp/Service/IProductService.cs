﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Service
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product InsertProduct();
    }
}
