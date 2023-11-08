using Microsoft.EntityFrameworkCore;
using ThAhCo.Orders.Api.Data;

namespace ThAhCo.Orders.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<OrderContext>(options =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    var folder = Environment.SpecialFolder.LocalApplicationData;
                    var path = Environment.GetFolderPath(folder);
                    var dbPath = System.IO.Path.Join(path, "orders.db");
                    options.UseSqlite($"Data Source={dbPath}");
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                } else
                {
                    var cs = builder.Configuration.GetConnectionString("OrderContext");
                    options.UseSqlServer(cs);
                }
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/orders", async (OrderContext orderContext) =>
            {
                return await orderContext.Orders.ToListAsync();
            })
            .WithName("GetOrders")
            .WithOpenApi();

            var responseMessage = app.Configuration["Message"] ?? "";

            app.MapPost("/orders", async (OrderContext context, OrderDto dto) =>
            {
                var order = new Order { CustomerId = dto.CustomerId, RequestedDate = dto.RequestedDate };
                await context.AddAsync(order);
                await context.SaveChangesAsync();
                return responseMessage;
            });

            app.Run();

        }
    }
}
