using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
var builder = WebApplication.CreateBuilder(args);
//welcome
// Register services
builder.Services.AddControllers(); // 👈 Add controller support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .WithOrigins("http://localhost:4200") // Angular dev server URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();
app.UseCors("AllowAngularApp");

// Use Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoContext>();
    db.Database.Migrate();  // <-- This creates/updates tables
    // db.Database.EnsureCreated();

}


app.UseHttpsRedirection();

app.UseAuthorization();

// 👇 Enable attribute routing for controllers like [Route("todo")]
app.MapControllers();

app.Run();
