using CleverEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleverEstate.Services.Interface.Repository
{
    public interface IApartmentRepository
    {
        List<Apartment> GetAll();
        Apartment GetById(Guid id);
        void Insert(Apartment apartment);
        void Update(Apartment apartment);
        void Delete(Guid ApartmentId);
        void Save();
    }
}