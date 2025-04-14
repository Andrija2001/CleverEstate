using CleverEstate.Models;
using System;

namespace CleverState.Services.Interface
{
    internal interface IClientService
    {
        void Create(Client obj);
        Client GetClient(Guid Id);
        void Update(Client client);
        void Delete(Guid Id);
    }
}
