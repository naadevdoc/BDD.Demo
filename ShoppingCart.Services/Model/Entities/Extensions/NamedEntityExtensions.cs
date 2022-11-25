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
        public static T UpsertIntoRepository<T, Y>(this Y namedEntity,IList<NamedEntity> repository, T response)
            where T : EntityResponse
            where Y : NamedEntity
        {
            lock(repository)
            {
                repository.ToList().RemoveAll(x => x.Name == namedEntity.Name); 
                repository.Add((Y)namedEntity.Clone());
                return response;
            }
        }
    }
}
