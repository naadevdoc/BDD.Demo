using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities.Extensions
{
    internal static class ExchangeRateExtensions
    {
        internal static T UpsertIntoRepository<T>(this ExchangeRate exchangeRate, IList<ExchangeRate> repository, T response)
            where T : EntityResponse
        {
            lock (repository)
            {
                var indexes = repository.Where(x => x.FromCurrency == exchangeRate.FromCurrency && x.ToCurrency == exchangeRate.FromCurrency)
                                        .Select(x => new KeyValuePair<int, ExchangeRate>(repository.IndexOf(x), x))
                                        .ToList();
                indexes.ForEach(x => repository.RemoveAt(x.Key));
                repository.Add(exchangeRate.Clone());
                return response;
            }
        }
    }
}
