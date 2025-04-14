using CleverState.Services.Interface;
using CleverEstate.Models;
using System.Collections.Generic;
using System;

namespace CleverState.Services.Classes
{
    public class ApartmentService : IApartmentService
    {
        private List<Apartment> apartments = new List<Apartment>();

        public void Create(Apartment apartment)
        {
            apartments.Add(apartment);
        }

        public void Delete(Guid Id)
        {
            var apartmentToRemove = apartments.Find(a => a.Id == Id);
            if (apartmentToRemove != null)
            {
                apartments.Remove(apartmentToRemove);
            }
        }

        public Apartment GetApartment(Guid Id)
        {
            return apartments.Find(a => a.Id == Id);
        }
        public void Update(Apartment apartment)
        {
            var apartmentToUpdate = apartments.Find(a => a.Id == a.Id);
            if (apartmentToUpdate != null)
            {
                apartmentToUpdate.BuildingId = apartment.BuildingId;
                apartmentToUpdate.Number = apartment.Number;
                apartmentToUpdate.Area = apartment.Area;
                apartmentToUpdate.ClientId = apartment.ClientId;
            }
        }
        public List<Apartment> GetAllApartments()
        {
            return apartments;
        }

    }
}
