using CleverEstate.Models;
using CleverEstate.Services.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleverEstate.Services.Classes.Repository
{
    public class ItemCatalogRepository : IItemCatalogRepository
    {
        private readonly DataDbContext _context;
        public ItemCatalogRepository(DataDbContext context)
        {
            _context = context;
        }
        public void Delete(Guid ItemCatalogId)
        {
            var ItemCatalogInDb = _context.ItemCatalogs.Find(ItemCatalogId);
            if (ItemCatalogInDb != null)
            {
                _context.ItemCatalogs.Remove(ItemCatalogInDb);
                _context.SaveChanges();
            }
        }
        public List<ItemCatalog> GetAll()
        {
            return _context.ItemCatalogs.ToList();
        }
        public ItemCatalog GetById(Guid id)
        {
            return _context.ItemCatalogs.Find(id);
        }
        public void Insert(ItemCatalog itemCatalog)
        {
            _context.ItemCatalogs.Add(itemCatalog);
            _context.SaveChanges();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Update(ItemCatalog itemCatalog)
        {
            var existingEntity = _context.ItemCatalogs
                        .SingleOrDefault(ic => ic.Id == itemCatalog.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = System.Data.Entity.EntityState.Detached;
            }
            _context.Entry(itemCatalog).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}