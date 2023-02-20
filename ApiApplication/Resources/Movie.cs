using System;

namespace ApiApplication.Resources
{
    public class Movie
    {
        public string Title { get; set; }

        public string ImdbId { get; set; }

        public string Stars { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
