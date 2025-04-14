using CleverState.Services.Interface;
using CleverEstate.Models;
using System.Collections.Generic;
using System;

namespace CleverState.Services.Classes
{
    public class BuildingService : IBuildingService
    {
        private List<Building> buildings = new List<Building>();

        public void Create(Building building)
        {
            buildings.Add(building);
        }

        public void Delete(Guid Id)
        {
            var RemoveBuildings = buildings.Find(a => a.Id == Id);
            if (RemoveBuildings != null)
            {
                buildings.Remove(RemoveBuildings);
            }
        }

        public Building GetBuilding(Guid Id)
        {
            return buildings.Find(a => a.Id == Id);
        }

        public void Update(Building building)
        {
            var UpdateBuildings = buildings.Find(a => a.Id == a.Id);
            if (UpdateBuildings != null)
            {
                UpdateBuildings.Id = building.Id;
                UpdateBuildings.Address = building.Address;
            }
        }

        public List<Building> GetAllBuildings()
        {
            return buildings;
        }
    }
}
