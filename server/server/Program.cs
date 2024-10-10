using Services.YoutubeAPI;
using DataAccessLayer;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using server.Auth;
using DataAccessLayer.Seed;
using Microsoft.EntityFrameworkCore;
using Services.DiscordSevice;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
builder.Services.AddScoped<IYoutubeService, YoutubeService>();
//builder.Services.AddScoped<IDiscordService, DiscordService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistance(builder.Configuration);

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, AuthHandler>("BasicAuthentication", null);

builder.Services.AddAuthorization();
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        var seeder = services.GetRequiredService<DbSeeder>();

        dbContext.Database.Migrate();

        await seeder.SeedCategoriesFromCSV();
        await seeder.SeedChannelsFromCSV();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyHeader()
        .AllowAnyMethod()
        //.WithOrigins("http://localhost:5173/");
        .WithOrigins(builder.Configuration.GetSection("CorsURLs").Get<string[]>())
        .SetIsOriginAllowed(origin => true);
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
