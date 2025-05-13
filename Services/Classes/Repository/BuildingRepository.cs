using CleverEstate.Models;
using CleverEstate.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleverEstate.Services.Classes
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly DataDbContext _context;

        public BuildingRepository(DataDbContext context)
        {
            _context = context;
        }

        public void Insert(Building building)
        {
            _context.Buildings.Add(building);
            _context.SaveChanges();
        }

        public void Delete(Guid buildingId)
        {
            var buildingInDb = _context.Buildings.Find(buildingId);
            if (buildingInDb != null)
            {
                _context.Buildings.Remove(buildingInDb);
                _context.SaveChanges();
            }
        }

        public Building GetById(Guid id)
        {
            return _context.Buildings.Find(id);
        }

        public List<Building> GetAll()
        {
            return _context.Buildings.ToList();
        }

        public void Update(Building building)
        {
            var buildingInDb = _context.Buildings.FirstOrDefault(b => b.Id == building.Id);
            if (buildingInDb != null)
            {
                buildingInDb.Address = building.Address;
                // Dodaj druga polja ako ih imaš
                _context.SaveChanges();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}