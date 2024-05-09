using System;

namespace MI.Dapper.Data.Models
{
    public class AppRole
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }
    }
}