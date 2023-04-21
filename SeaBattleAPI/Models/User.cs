using System;
using System.Collections.Generic;

namespace SeaBattleAPI.Models
{
    public partial class User
    {
        public User()
        {
            RoomUserCreators = new HashSet<Room>();
            RoomUserSlows = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Room> RoomUserCreators { get; set; }
        public virtual ICollection<Room> RoomUserSlows { get; set; }
    }
}
