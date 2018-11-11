﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Producer.Infrastructure;
using Producer.Repositories;

namespace Producer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDatabase, Database>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IEventRepository, EventRepository>();
            services.AddSingleton<ISubscriptionRepository, SubscriptionRepository>();

            services.AddSingleton<IEventQueue, EventQueue>();
            services.AddSingleton<IEventQueueWorker, EventQueueWorker>();
            
            services.AddSingleton<HttpClient>(provider => new HttpClient());

            services.AddMvc()
                .AddJsonOptions(jo => jo.SerializerSettings.Converters.Add(new StringEnumConverter()));

            Dapper.SqlMapper.AddTypeMap(typeof(string), DbType.AnsiString);
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}
