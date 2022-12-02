using ShoppingCart.Services.Model.Entities;

namespace ShoppingCart.Services.Model.OperationsService
{
    public class TransformPriceForPersonResponse : EntityResponse
    {
        public string PersonaName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public CurrencyType? From { get; set; }
        public CurrencyType? To { get; set; } 
    }
}