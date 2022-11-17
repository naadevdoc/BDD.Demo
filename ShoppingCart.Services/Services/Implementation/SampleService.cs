using ShoppingCart.Services.Model.SampleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Services.Implementation
{
    internal class SampleService : ISampleService
    {
        public SampleResponse GetTrue(SampleRequest request) => new SampleResponse();
    }
}
