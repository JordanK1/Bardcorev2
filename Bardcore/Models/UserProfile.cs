using System;
using System.Collections.Generic;

namespace Bardcore.Models
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            UserPlaylist = new HashSet<UserPlaylist>();
        }

        public int Userid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Gender { get; set; }
        public byte? Age { get; set; }
        public string Bio { get; set; }
        public string UserAccountId { get; set; }
        public string PhotoPath { get; set; }

        public ICollection<UserPlaylist> UserPlaylist { get; set; }
    }
}
