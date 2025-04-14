using CleverEstate.Models;
using System;

namespace CleverState.Services.Interface
{
    public interface IApartmentService
    {
        void Create(Apartment obj);
        Apartment GetApartment(Guid Id);
        void Update(Apartment apartment);
        void Delete(Guid Id);
    }
}
