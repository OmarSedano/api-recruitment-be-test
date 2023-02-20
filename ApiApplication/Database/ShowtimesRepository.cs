using ApiApplication.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiApplication.Database
{
    public class ShowtimesRepository : IShowtimesRepository
    {
        private readonly CinemaContext _context;
        public ShowtimesRepository(CinemaContext context)
        {
            _context = context;
        }

        public ShowtimeEntity Add(ShowtimeEntity showtimeEntity)
        {
            throw new System.NotImplementedException();
        }

        public ShowtimeEntity Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public ShowtimeEntity GetByMovie(Func<IQueryable<MovieEntity>, bool> filter)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ShowtimeEntity> GetCollection()
        {
            return GetCollection(null);
        }

        //Could not figure out how to make it work with Func<IQueryable<ShowtimeEntity>, bool> filter :( and used an alternative instead
        public IEnumerable<ShowtimeEntity> GetCollection(Expression<Func<ShowtimeEntity, bool>> filter)
        {
            return filter == null
                ? _context.Showtimes.Include(x => x.Movie)
                : _context.Showtimes.Where(filter).Include(x => x.Movie);
        }

        public ShowtimeEntity Update(ShowtimeEntity showtimeEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}
