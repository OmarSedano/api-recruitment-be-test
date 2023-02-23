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
using IMDbApiLib;

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

        //TODO: Change to IActionResult methods
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
        public async Task<Showtime> Post([FromBody] Showtime showTime)
        {
            if (showTime == null)
            {
                throw new ArgumentNullException("showTime can not be null");
            }

            if (showTime.Movie?.ImdbId == null)
            {
                throw new ArgumentNullException("showTime > Movie > Imdb can not be null");
            }

            //TODO: Move to IConfiguration service. read from appsettings
            var apiLib = new ApiLib("k_ecm9jl9u");
            var titleData = await apiLib.TitleAsync(showTime.Movie.ImdbId);

            showTime.Movie.Stars = titleData.Stars;
            showTime.Movie.Title = titleData.Title;
            showTime.Movie.ReleaseDate = DateTime.Parse(titleData.ReleaseDate);

            var newShowTime = ShowTimeMapper.Map(showTime);
            var savedShowTime = _showtimesRepository.Add(newShowTime);
            return ShowTimeMapper.Map(savedShowTime);
        }


        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Showtime showTime)
        {
            if (showTime == null)
            {
                throw new ArgumentNullException("showTime can not be null");
            }

            showTime.Id = id;

            if (showTime.Movie != null)
            {
                showTime = await UpdateMovieData(showTime.Movie.ImdbId, showTime);
            }

            var showtimeEntity = ShowTimeMapper.Map(showTime);
            _showtimesRepository.Update(showtimeEntity);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _showtimesRepository.Delete(id);
        }

        [HttpPatch]
        public void Patch()
        {
            throw new NotImplementedException();
        }

        //TODO: Move to a Service
        private async Task<Showtime> UpdateMovieData(string imdbId, Showtime showTime)
        {
            var apiLib = new ApiLib("k_ecm9jl9u");
            var titleData = await apiLib.TitleAsync(showTime.Movie.ImdbId);

            showTime.Movie.Stars = titleData.Stars;
            showTime.Movie.Title = titleData.Title;
            showTime.Movie.ReleaseDate = DateTime.Parse(titleData.ReleaseDate);
            return showTime;
        }
    }
}
