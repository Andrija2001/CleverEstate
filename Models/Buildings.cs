using System;
using System.Collections.Generic;

namespace CleverEstate.Models
{
    public class Building
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public List<Apartment> Apartments { get; set; } = new List<Apartment>();
    }
}