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
            _context.Showtimes.Add(showtimeEntity);
            _context.SaveChanges();
            return showtimeEntity;
        }

        //Modified to return void
        public void Delete(int id)
        {
            try
            {
                var showtimeEntity = new ShowtimeEntity()
                {
                    Id = id
                };

                _context.Entry(showtimeEntity).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Entity does not exist");
            }
        }

        public ShowtimeEntity GetByMovie(Func<IQueryable<MovieEntity>, bool> filter)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ShowtimeEntity> GetCollection()
        {
            return GetCollection(null);
        }

        //Could not figure out how to make it work with Func<IQueryable<ShowtimeEntity>, bool> filter and used an alternative instead
        public IEnumerable<ShowtimeEntity> GetCollection(Expression<Func<ShowtimeEntity, bool>> filter)
        {
            return filter == null
                ? _context.Showtimes.Include(x => x.Movie)
                : _context.Showtimes.Where(filter).Include(x => x.Movie);
        }

        public ShowtimeEntity Update(ShowtimeEntity updateShowtimeEntity)
        {
            var showTimeEntity = _context.Showtimes.Include(x => x.Movie).FirstOrDefault(x => x.Id == updateShowtimeEntity.Id);
            _context.Entry(showTimeEntity).CurrentValues.SetValues(updateShowtimeEntity);
            if (updateShowtimeEntity.Movie != null)
            {
                showTimeEntity.Movie = updateShowtimeEntity.Movie;
            }
            _context.SaveChanges();
            return updateShowtimeEntity;
        }
    }
}
