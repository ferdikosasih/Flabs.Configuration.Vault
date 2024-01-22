using Flabs.Configuration.VaultSharp.Extensions;

namespace Flabs.Sample.Configuration
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //var flabsOptions = new FlabsConfigOptions("root", "http://localhost:8200/");
            //builder.Services.AddFlabsConfig(flabsOptions);
            builder.Services.AddFlabsConfig(options =>
            {
                options.VaultToken = "root";
                options.VaultAddress = "http://localhost:8200/";
                options.ReloadTimeMinute = 60;
            });

            builder.Services
                .AddConfigOptions<SampleOptions>()
                .AddConfigOptions<Sample2Options>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
