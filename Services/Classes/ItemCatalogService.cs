using CleverState.Services.Interface;
using CleverEstate.Models;
using System.Collections.Generic;
using System;

namespace CleverState.Services.Classes
{
    public class ItemCatalogService : IItemCatalogService
    {
        private List<ItemCatalog> ItemCatalogs = new List<ItemCatalog>();

        public void Create(ItemCatalog ItemCatalog)
        {
            ItemCatalogs.Add(ItemCatalog);
        }

        public void Delete(Guid Id)
        {
            var RemoveItem = ItemCatalogs.Find(a => a.Id == Id);
            if (RemoveItem != null)
            {
                ItemCatalogs.Remove(RemoveItem);
            }
        }

        public ItemCatalog GetItemCatalog(Guid id)
        {
            return ItemCatalogs.Find(a => a.Id == id);
        }

        public void Update(ItemCatalog ItemCatalog)
        {
            var UpdateItemCatalog = ItemCatalogs.Find(a => a.Id == a.Id);
            if (UpdateItemCatalog != null)
            {
                UpdateItemCatalog.Id = ItemCatalog.Id;
                UpdateItemCatalog.Unit = ItemCatalog.Unit;
                UpdateItemCatalog.PricePerUnit = ItemCatalog.PricePerUnit;
                UpdateItemCatalog.Name = ItemCatalog.Name;
            }
        }


        public List<ItemCatalog> GetAllCatalogItems()
        {
            return ItemCatalogs;
        }
    }
}
