﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
	[Route("/api/trips/{tripName}/stops")]
	public class StopsController : Controller
	{
		private IWorldRepository _repository;
		private ILogger<StopsController> _logger;
		private IMapper _mapper;
		private GeoCoordsService _coordsService;

		public StopsController(IWorldRepository repository, ILogger<StopsController> logger, IMapper mapper,
														GeoCoordsService coordsService)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
			_coordsService = coordsService;
		}

		[HttpGet("")]
		public IActionResult Get(string tripName)
		{
			try
			{
				var trip = _repository.GetTripByName(tripName);

				return Ok(_mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to get stops: {0}", ex);
			}

			return BadRequest("Failed to get stops");
		}

		[HttpPost("")]
		public async Task<ActionResult> Post(string tripName, [FromBody]StopViewModel vm)
		{
			try
			{
				//check if VM is valid
				if (ModelState.IsValid)
				{
					var newStop = _mapper.Map<Stop>(vm);

					//Lookup the Geocodes
					var result = await _coordsService.GetCoordsAsync(newStop.Name);
					if (!result.Success)
					{
						_logger.LogError(result.Message);
					}
					else
					{
						newStop.Latitude = result.Latitude;
						newStop.Longitude = result.Longitude;

						//Save to the Database
						_repository.AddStop(tripName, newStop);

						if (await _repository.SaveChangesAsync())
						{
							return Created($"/api/trips/{tripName}/stops/{newStop.Name}", _mapper.Map<StopViewModel>(newStop));
						}
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to save new Stop: {0}", ex);
			}

			return BadRequest("Failed to save new stop");
		}
	}
}
