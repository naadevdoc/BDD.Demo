using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.OperationsService
{
    public class CommitPurchaseResponse : OperationsPersonaResponse
    {
        public List<string> PurchaseMessages { get; set; } = new List<string>();
    }
}
