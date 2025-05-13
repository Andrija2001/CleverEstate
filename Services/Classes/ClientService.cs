using CleverState.Services.Interface;
using CleverEstate.Models;
using System.Collections.Generic;
using System;
using CleverEstate.Services.Interface;
using CleverEstate.Services.Interface.Repository;

namespace CleverState.Services.Classes
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public void Create(Client client) => _repository.Insert(client);

        public void Delete(Guid id) => _repository.Delete(id);

        public Client GetClient(Guid id) => _repository.GetById(id);

        public void Update(Client client) => _repository.Update(client);

        public List<Client> GetAllClients() => _repository.GetAll();
    }
}
