using CleverEstate.Models;
using CleverEstate.Services.Interface.Repository;
using CleverState.Services.Interface;
using System;
using System.Collections.Generic;
namespace CleverState.Services.Classes
{
    public class ApartmentService : IApartmentService
    {
        private readonly IApartmentRepository _repository;

        public ApartmentService(IApartmentRepository repository)
        {
            _repository = repository;
        }
        public void Create(Apartment apartment) => _repository.Insert(apartment);

        public void Delete(Guid id) => _repository.Delete(id);

        public Apartment GetApartment(Guid id) => _repository.GetById(id);

        public void Update(Apartment apartment) => _repository.Update(apartment);

        public List<Apartment> GetAllApartmants() => _repository.GetAll();
    }
}