using CleverEstate.Models;
using CleverEstate.Services.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleverEstate.Services.Classes.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataDbContext _context;
        public ClientRepository(DataDbContext context)
        {
            _context = context;
        }
        public void Delete(Guid ClientId)
        {
            var ClientInDb = _context.Clients.Find(ClientId);
            if (ClientInDb != null)
            {
                _context.Clients.Remove(ClientInDb);
                _context.SaveChanges();
            }
        }
        public List<Client> GetAll()
        {
            return _context.Clients.ToList();
        }
        public Client GetById(Guid id)
        {
            return _context.Clients.Find(id);
        }
        public void Insert(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Update(Client client)
        {
            var existingClient = _context.Clients.Find(client.Id);
            if (existingClient != null)
            {
                _context.Entry(existingClient).CurrentValues.SetValues(client);
                _context.SaveChanges();
            }
        }
    }
}