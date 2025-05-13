using CleverState.Services.Interface;
using CleverEstate.Models;
using System.Collections.Generic;
using System;
using CleverEstate.Services.Interface.Repository;

namespace CleverState.Services.Classes
{
    public class ItemCatalogService : IItemCatalogService
    {
        private readonly IItemCatalogRepository _repository;

        public ItemCatalogService(IItemCatalogRepository repository)
        {
            _repository = repository;
        }

        public void Create(ItemCatalog itemCatalog) => _repository.Insert(itemCatalog);

        public void Delete(Guid id) => _repository.Delete(id);

        public ItemCatalog GetItemCatalog(Guid id) => _repository.GetById(id);

        public void Update(ItemCatalog itemCatalog) => _repository.Update(itemCatalog);

        public List<ItemCatalog> GetItemCatalogItems() => _repository.GetAll();
    }
}
