using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetroDRY;

namespace MyServer
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
            //TODO:If you don't need CORS, you can remove this and the UseCors call below
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void InitializeRetroDRY()
        {
            //This will tell RetroDRY how to access your database
            //TODO:Modify this resolver to return an open DbConnection to your database. If you are using PostgreSQL, then you probably
            //only need to change the connection string. If you are using a different database vendor, then you should remove the "npgsql" nuget
            //package and add the package for your driver.
            static Task<DbConnection> dbResolver(int databaseNumber)
            {
                var db = new Npgsql.NpgsqlConnection("host=localhost;database=retrodrydemo;username=postgres;password=mypasswordgoeshere");
                db.Open();
                return Task.FromResult(db as DbConnection);
            }

            //build data dictionary from annotations
            var ddict = new DataDictionary();
            ddict.AddDatonsUsingClassAnnotation(typeof(Startup).Assembly);

            //TODO:Here is the best place to add things like: custom columns, multi-language prompts, custom validation,
            //and default values initializers

            //start up RetroDRY
            ddict.FinalizeInheritance();
            Globals.Retroverse?.Dispose();
            Globals.Retroverse = new Retroverse();
            Globals.Retroverse.Initialize(SqlFlavorizer.VendorKind.PostgreSQL, ddict, dbResolver);

            //TODO:Here is the best place to add things like custom exception text rewriting and error logging
        }

    }
}
