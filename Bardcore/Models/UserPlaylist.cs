using System;
using System.Collections.Generic;

namespace Bardcore.Models
{
    public partial class UserPlaylist
    {
        public UserPlaylist()
        {
            UserPlaylistTrack = new HashSet<UserPlaylistTrack>();
        }

        public int PlaylistId { get; set; }
        public int? PlaylistCreator { get; set; }
        public string PlaylistName { get; set; }

        public UserProfile PlaylistCreatorNavigation { get; set; }
        public ICollection<UserPlaylistTrack> UserPlaylistTrack { get; set; }
    }
}
