using CleverEstate.Models;
using System;

namespace CleverState.Services.Interface
{
    internal interface IItemCatalogService
    {
        void Create(ItemCatalog obj);
        ItemCatalog GetItemCatalog(Guid id);
        void Update(ItemCatalog ItemCatalog);
        void Delete(Guid id);
    }
}
