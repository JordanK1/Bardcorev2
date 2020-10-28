using System;
using System.Collections.Generic;

namespace Bardcore.Models
{
    public partial class SongInfo
    {
        public SongInfo()
        {
            UserPlaylistTrack = new HashSet<UserPlaylistTrack>();
        }

        public int TrackId { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public int? Genre { get; set; }
        public short? ReleaseYear { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileLocation { get; set; }

        public Genre GenreNavigation { get; set; }
        public ICollection<UserPlaylistTrack> UserPlaylistTrack { get; set; }
    }
}
