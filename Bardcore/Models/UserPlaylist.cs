using System;
using System.Collections.Generic;

namespace Bardcore.Models
{
    public partial class UserPlaylist
    {
        

        public int PlaylistId { get; set; }
        public int? PlaylistCreator { get; set; }
        public string PlaylistName { get; set; }

        public UserProfile PlaylistCreatorNavigation { get; set; }
        public List<UserPlaylistTrack> UserPlaylistTrack { get; set; }

        public UserPlaylist()
        {
            UserPlaylistTrack = new List<UserPlaylistTrack>();
        }
    }
}
