using CleverEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleverEstate.Services.Interface
{
    public interface IBuildingRepository
    {
        List<Building> GetAll();
        Building GetById(Guid id);
        void Insert(Building building);
        void Update(Building building);
        void Delete(Guid BuildingId);
        void Save();
    }
}