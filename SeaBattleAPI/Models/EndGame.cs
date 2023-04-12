using System;
using System.Collections.Generic;

namespace SeaBattleAPI.Models
{
    public partial class EndGame
    {
        public int Id { get; set; }
        public int UserWinId { get; set; }
        public int UserShitId { get; set; }
        public int TurnCountUserWin { get; set; }
        public int TurnCountUserShit { get; set; }

        public virtual User UserShit { get; set; } = null!;
        public virtual User UserWin { get; set; } = null!;
    }
}
