using CleverEstate.Models;
using CleverEstate.Services.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CleverEstate.Services.Classes.Repository
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly DataDbContext _context;
        public ApartmentRepository(DataDbContext context)
        {
            _context = context;
        }
        public void Delete(Guid ApartmentId)
        {
            var ApartmentInDb = _context.Apartments.Find(ApartmentId);
            if (ApartmentInDb != null)
            {
                _context.Apartments.Remove(ApartmentInDb);
                _context.SaveChanges();
            }
        }
        public List<Apartment> GetAll()
        {
            return _context.Apartments.ToList();
        }
        public Apartment GetById(Guid id)
        {
            return _context.Apartments.Find(id);
        }
        public void Insert(Apartment apartment)
        {
            _context.Apartments.Add(apartment);
            _context.SaveChanges();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Update(Apartment apartment)
        {
            var existingEntity = _context.Apartments
                     .SingleOrDefault(a => a.Id == apartment.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = System.Data.Entity.EntityState.Detached;
            }
            _context.Entry(apartment).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}