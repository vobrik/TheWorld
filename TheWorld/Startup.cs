﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld
{
	public class Startup
	{
		private IHostingEnvironment _env;
		private IConfigurationRoot _config;		

		public Startup(IHostingEnvironment env)
		{
			_env = env;

			var builder = new ConfigurationBuilder()
				.SetBasePath(_env.ContentRootPath)
				.AddJsonFile("config.json")
				.AddEnvironmentVariables();

			_config = builder.Build();
			
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(_config);

			if (_env.IsEnvironment("Development") || _env.IsEnvironment("Testing"))
			{
				services.AddLogging(loggingBuilder =>
				{
					loggingBuilder.SetMinimumLevel(LogLevel.Information);
					loggingBuilder.AddDebug();
				});

				services.AddScoped<IMailService, DebugMailService>();
			}
			else
			{
				services.AddLogging(loggingBuilder =>
				{
					loggingBuilder.SetMinimumLevel(LogLevel.Error);
					loggingBuilder.AddDebug();
				});

				//Implement a real Mail Service
			}

			services.AddDbContext<WorldContext>();

			services.AddScoped<IWorldRepository , WorldRepository>();

			services.AddTransient<GeoCoordsService>();

			services.AddTransient<WorldContextSeedData>();

			var mappingConfig = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<TripViewModel , Trip>().ReverseMap();
				cfg.CreateMap<StopViewModel , Stop>().ReverseMap();
			});

			services.AddSingleton<IMapper>(sp => mappingConfig.CreateMapper());

			services.AddMvc()
				.AddJsonOptions(config =>
				{
					config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
														WorldContextSeedData seeder)//, ILoggerFactory factory)
		{
			//Mapper.Initialize(config =>
			//{

			//});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				//factory.AddDebug(LogLevel.Information);
			}
			else
			{
				//factory.AddDebug(LogLevel.Error);
			}
			app.UseStaticFiles();
			app.UseMvc(config =>
			{
				config.MapRoute(
					name: "Default",
					template: "{controller}/{action}/{id?}",
					defaults: new { controller = "App", action = "Index" }
					);
			});

			seeder.EnsureSeedData().Wait();
		}
	}
}
