using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;

namespace TheWorld.Controllers.Api
{
	public class TripsController : Controller
	{
		[HttpGet("api/trips")]
		public JsonResult Get()
		{
			return Json(new Trip() { Name = "My Trip" });
		}
	}
}
