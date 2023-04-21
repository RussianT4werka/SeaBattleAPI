using System;
using System.Collections.Generic;

namespace SeaBattleAPI.Models
{
    public partial class Room
    {
        public Room()
        {
            EndGames = new HashSet<EndGame>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int UserCreatorId { get; set; }
        public int? UserSlowId { get; set; }
        public DateTime? GameTimeStart { get; set; }
        public int StatusId { get; set; }

        public virtual Status Status { get; set; } = null!;
        public virtual User UserCreator { get; set; } = null!;
        public virtual User? UserSlow { get; set; }
        public virtual ICollection<EndGame> EndGames { get; set; }
    }
}
