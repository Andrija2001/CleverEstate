using CleverState.Services.Interface;
using CleverEstate.Models;
using System.Collections.Generic;
using System;
using CleverEstate.Services.Interface;

namespace CleverState.Services.Classes
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _repository;

        public BuildingService(IBuildingRepository repository)
        {
            _repository = repository;
        }

        public void Create(Building building) => _repository.Insert(building);

        public void Delete(Guid id) => _repository.Delete(id);

        public Building GetBuilding(Guid id) => _repository.GetById(id);

        public void Update(Building building) => _repository.Update(building);

        public List<Building> GetAllBuildings() => _repository.GetAll();
    }
}
