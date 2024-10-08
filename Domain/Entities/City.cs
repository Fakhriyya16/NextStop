﻿
using Domain.Common;

namespace Domain.Entities
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public ICollection<Place> Places { get; set; }
    }
}
