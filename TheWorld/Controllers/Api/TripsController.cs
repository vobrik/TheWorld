using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
	[Route("api/trips")]
	public class TripsController : Controller
	{
		private IWorldRepository _repository;
		private ILogger<TripsController> _logger;
		private IMapper _mapper;

		public TripsController(IWorldRepository repository, ILogger<TripsController> logger, IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
		}

		[HttpGet("")]
		public IActionResult Get()
		{
			try
			{
				var results = _repository.GetAllTrips();

				return Ok(_mapper.Map<IEnumerable<TripViewModel>>(results));
			}
			catch (Exception ex)
			{				
				_logger.LogError($"Failed to get All Trips: {ex}");

				return BadRequest("Error occurred");
			}
		}

		[HttpPost("")]
		public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
		{
			if (ModelState.IsValid)
			{
				//Save to the Database
				var newTrip = _mapper.Map<Trip>(theTrip);
				_repository.AddTrip(newTrip);

				if (await _repository.SaveChangesAsync())
				{
					return Created($"api/trips/{theTrip.Name}" , _mapper.Map<TripViewModel>(newTrip));
				}				
			}

			return BadRequest("Failed to save the trip");
		}
	}
}
