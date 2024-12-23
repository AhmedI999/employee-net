using Employee.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure connection string
String connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Ensure you pass the connection string where required
builder.Services.AddSingleton(_ => new EmployeeRepository(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();
app.MapControllers();
app.Run();