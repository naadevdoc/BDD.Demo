using ShoppingCart.Services.Model.SampleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Services
{
    public interface ISampleService
    {
        public SampleResponse GetTrue(SampleRequest request);
    }
}
