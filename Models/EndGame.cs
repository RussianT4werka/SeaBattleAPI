using System;
using System.Collections.Generic;

namespace SeaBattleAPI.Models
{
    public partial class EndGame
    {
        public int Id { get; set; }
        public int? UserWin { get; set; }
        public int? UserShit { get; set; }
        public int? TurnCountUserWin { get; set; }
        public DateTime? GameTimeEnd { get; set; }
        public int RoomId { get; set; }

        public virtual Room Room { get; set; } = null!;
    }
}
