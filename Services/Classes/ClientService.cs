using CleverState.Services.Interface;
using CleverEstate.Models;
using System.Collections.Generic;
using System;

namespace CleverState.Services.Classes
{
    public class ClientService : IClientService
    {
        private List<Client> clients = new List<Client>();
        public void Create(Client client)
        {
            clients.Add(client);

        }

        public void Delete(Guid Id)
        {
            var RemoveClients = clients.Find(a => a.Id == Id);
            if (RemoveClients != null)
            {
                clients.Remove(RemoveClients);
            }
        }

        public Client GetClient(Guid Id)
        {
            return clients.Find(a => a.Id == Id);
        }

        public void Update(Client client)
        {
            var UpdateClients = clients.Find(a => a.Id == a.Id);
            if (UpdateClients != null)
            {
                UpdateClients.Id = client.Id;
                UpdateClients.InvoiceId = client.InvoiceId;
                UpdateClients.Name = client.Name;
                UpdateClients.PIB = client.PIB;
                UpdateClients.Address = client.Address;
                UpdateClients.City = client.City;
                UpdateClients.BankAccount = client.BankAccount;
                UpdateClients.Surname = client.Surname;
            }
        }

        public List<Client> GetAllClients()
        {
            return clients;
        }
    }
}
