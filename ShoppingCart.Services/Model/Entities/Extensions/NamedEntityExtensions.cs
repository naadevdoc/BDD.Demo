using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities.Extensions
{
    internal static class NamedEntityExtensions
    {
        internal static T UpsertIntoRepository<T, Y>(this Y namedEntity,IList<NamedEntity> repository, T response)
            where T : EntityResponse
            where Y : NamedEntity
        {
            lock(repository)
            {
                var indexes = repository.Where(x => x.Name == namedEntity.Name)
                                        .Select(x => new KeyValuePair<int,NamedEntity> (repository.IndexOf(x),x))
                                        .ToList();
                indexes.ForEach(x => repository.RemoveAt(x.Key));
                repository.Add((Y)namedEntity.Clone());
                return response;
            }
        }
    }
}
