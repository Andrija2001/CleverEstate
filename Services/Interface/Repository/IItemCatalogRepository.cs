using CleverEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleverEstate.Services.Interface.Repository
{
    public interface IItemCatalogRepository
    {
        List<ItemCatalog> GetAll();
        ItemCatalog GetById(Guid id);
        void Insert(ItemCatalog itemCatalog);
        void Update(ItemCatalog itemCatalog);
        void Delete(Guid ItemCatalogId);
        void Save();
    }
}