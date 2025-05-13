using CleverEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleverEstate.Services.Interface.Repository
{
    public interface IClientRepository
    {
        List<Client> GetAll();
        Client GetById(Guid id);
        void Insert(Client client);
        void Update(Client client);
        void Delete(Guid ClientId);
        void Save();
    }
}