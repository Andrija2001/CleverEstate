using CleverEstate.Models;
using System;

namespace CleverState.Services.Interface
{
    internal interface IBuildingService
    {
        void Create(Building obj);
        Building GetBuilding(Guid Id);
        void Update(Building building);
        void Delete(Guid Id);
    }
}
