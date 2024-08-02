namespace Catalog.Api
{
    public class Startup
    {
        public IConfiguration Configuration;


        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();

            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



        }


    }
}
