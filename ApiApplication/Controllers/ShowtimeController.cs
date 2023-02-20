using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiApplication.Resources;
using ApiApplication.Database;
using ApiApplication.Database.Entities;
using ApiApplication.Mappers;
using System.Linq.Expressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimeController : ControllerBase
    {
        private readonly IShowtimesRepository _showtimesRepository;

        public ShowtimeController(IShowtimesRepository showtimesRepository)
        {
            _showtimesRepository = showtimesRepository;
        }

        [HttpGet]
        public IEnumerable<Showtime> Get(DateTime? date, string? title)
        {

            Expression<Func<ShowtimeEntity, bool>> predicate = (showTime) => ((date != null && date >= showTime.StartDate && date <= showTime.EndDate) || (date == null))
                                                                           && ((title != null && showTime.Movie.Title.Equals(title, StringComparison.InvariantCultureIgnoreCase)) || (title == null));

            var showTimesEntities = _showtimesRepository.GetCollection(predicate);
            var showTimes = showTimesEntities.Select(ShowTimeMapper.Map);
            return showTimes;
        }

        [HttpPost]
        public async Task<int> Post([FromBody] Showtime showTime)
        {
            _showtimesRepository.Add()
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Showtime showTime)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPatch]
        public void Patch()
        {
            throw new NotImplementedException();
        }
    }
}
