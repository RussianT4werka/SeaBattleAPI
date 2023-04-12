using System;
using System.Collections.Generic;

namespace SeaBattleAPI.Models
{
    public partial class User
    {
        public User()
        {
            EndGameUserShits = new HashSet<EndGame>();
            EndGameUserWins = new HashSet<EndGame>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<EndGame> EndGameUserShits { get; set; }
        public virtual ICollection<EndGame> EndGameUserWins { get; set; }
    }
}
