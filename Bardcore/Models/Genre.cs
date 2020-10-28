using System;
using System.Collections.Generic;

namespace Bardcore.Models
{
    public partial class Genre
    {
        public Genre()
        {
            SongInfo = new HashSet<SongInfo>();
        }

        public int GenreId { get; set; }
        public string Gname { get; set; }

        public ICollection<SongInfo> SongInfo { get; set; }
    }
}
