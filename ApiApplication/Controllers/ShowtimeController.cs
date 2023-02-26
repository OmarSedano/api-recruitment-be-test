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
using ApiApplication.Auth;
using ApiApplication.Services;
using IMDbApiLib;
using Microsoft.AspNetCore.Authorization;
using ApiApplication.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimeController : ControllerBase
    {
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IImdbService _imdbService;

        public ShowtimeController(IShowtimesRepository showtimesRepository, IImdbService imdbService)
        {
            _showtimesRepository = showtimesRepository;
            _imdbService = imdbService;
        }

        //TODO: Change to IActionResult methods
        [Authorize(Policy = Policies.Read)]
        [HttpGet]
        public IEnumerable<Showtime> Get(DateTime? date, string? title)
        {
            Expression<Func<ShowtimeEntity, bool>> predicate = (showTime) => ((date != null && date >= showTime.StartDate && date <= showTime.EndDate) || (date == null))
                                                                           && ((title != null && showTime.Movie.Title.Equals(title, StringComparison.InvariantCultureIgnoreCase)) || (title == null));

            var showTimesEntities = _showtimesRepository.GetCollection(predicate);
            var showTimes = showTimesEntities.Select(ShowTimeMapper.Map);
            return showTimes;
        }

        [Authorize(Policy = Policies.Write)]
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

            showTime = await SetMovieData(showTime);

            var newShowTime = ShowTimeMapper.Map(showTime);
            var savedShowTime = _showtimesRepository.Add(newShowTime);
            return ShowTimeMapper.Map(savedShowTime);
        }

        [Authorize(Policy = Policies.Write)]
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
                showTime = await SetMovieData(showTime);
            }

            var showtimeEntity = ShowTimeMapper.Map(showTime);
            _showtimesRepository.Update(showtimeEntity);
        }

        [Authorize(Policy = Policies.Write)]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _showtimesRepository.Delete(id);
        }

        [Authorize(Policy = Policies.Write)]
        [HttpPatch]
        public void Patch()
        {
            throw new CustomException();
        }

        private async Task<Showtime> SetMovieData(Showtime showTime)
        {
            var titleData = await _imdbService.GetTitleDataAsync(showTime.Movie.ImdbId);

            showTime.Movie.Stars = titleData.Stars;
            showTime.Movie.Title = titleData.Title;
            showTime.Movie.ReleaseDate = DateTime.Parse(titleData.ReleaseDate);
            return showTime;
        }
    }
}
