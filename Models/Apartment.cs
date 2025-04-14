using System;

namespace CleverEstate.Models
{
    public class Apartment
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public int Number { get; set; }
        public decimal Area { get; set; }
        public Guid ClientId { get; set; }
    }
}
