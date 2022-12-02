using ShoppingCart.Services.Model.Entities;

namespace ShoppingCart.Services.Model.OperationsService
{
    public class TransformPriceForPersonRequest : EntityRequest
    {
        public string PersonaName = string.Empty;
        public string ProductName = string.Empty;
    }
}