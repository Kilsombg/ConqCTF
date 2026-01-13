using ConqCTF.Application;
using ConqCTF.Infrastructure;
using ConqCTF.Infrastructure.Data;
using ConqCTF.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.Services.InitialiseDatabaseAsync();
}

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(builder.Configuration["JWT:Audience"]);
});

app.UseHttpsRedirection();

app.UseExceptionHandler(options => { });

app.UseRouting();

/*
app.Use(async (context, next) =>
{
    var authHeader = context.Request.Headers.Authorization.ToString();
    Console.WriteLine($"Authorization header: {authHeader}");

    await next();
});
*/

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
