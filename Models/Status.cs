using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SeaBattleAPI.Models
{
    public partial class Status
    {
        public Status()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        //[JsonIgnore]
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
