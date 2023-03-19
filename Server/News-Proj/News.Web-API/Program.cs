using Microsoft.AspNetCore.Cors.Infrastructure;
using News.Entities;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var corsBuilder = new CorsPolicyBuilder();
corsBuilder.AllowAnyHeader();
corsBuilder.AllowAnyMethod();
corsBuilder.AllowAnyOrigin();
//corsBuilder.AllowCredentials();
builder.Services.AddCors(options => { options.AddPolicy("AllowAll", corsBuilder.Build()); });




var app = builder.Build();

app.UseCors(builder =>
    builder.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//what we add
app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.UseAuthorization();



MainManager.Instance.Init();
app.Run();