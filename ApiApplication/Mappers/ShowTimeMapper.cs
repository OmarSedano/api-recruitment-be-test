using ApiApplication.Database.Entities;
using ApiApplication.Resources;
using System;
using System.Linq;

namespace ApiApplication.Mappers
{
    public static class ShowTimeMapper
    {
        public static ShowtimeEntity Map(Showtime showTime)
        {
            return new ShowtimeEntity
            {
                Id = showTime.Id,
                AuditoriumId = showTime.AuditoriumId,
                EndDate = showTime.EndDate,
                Schedule = showTime.Schedule.Split(",").Select(x => x.Trim()),
                StartDate = showTime.StartDate,
                Movie = Map(showTime.Movie)
            };
        }

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

        private static MovieEntity Map(Movie movie)
        {
            if(movie == null) return null;

            return new MovieEntity()
            {
                Title = movie.Title,
                ImdbId = movie.ImdbId,
                Stars = movie.Stars,
                ReleaseDate = movie.ReleaseDate,
            };
        }

    }
}
