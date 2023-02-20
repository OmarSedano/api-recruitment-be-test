using ApiApplication.Database.Entities;
using ApiApplication.Resources;
using System;

namespace ApiApplication.Mappers
{
    public static class ShowTimeMapper
    {
        public static Showtime Map(ShowtimeEntity entity)
        {
            return new Showtime
            {
                Id = entity.Id,
                EndDate = entity.EndDate,
                StartDate = entity.StartDate,
                Movie = Map(entity.Movie),
                Schedule = string.Join(", ", entity.Schedule),
                AuditoriumId = entity.AuditoriumId
            };
        }

        private static Movie Map(MovieEntity movie)
        {
            return new Movie()
            {
                Title = movie.Title,
                ImdbId = movie.ImdbId,
                Stars = movie.Stars,
                ReleaseDate = movie.ReleaseDate
            };
        }

    }
}
