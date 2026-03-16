var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); //Add MVC/Api controllers and related services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Show detailed error pages in development
    app.UseSwagger(); // Enable Swagger middleware for API documentation in development
    app.UseSwaggerUI(); // Enable Swagger UI for interactive API documentation in development
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseRouting(); // Enable routing middleware to route requests to endpoints
app.UseAuthontication(); // Enable authentication middleware to authenticate users
app.UseAuthorization(); // Enable authorization middleware to authorize users

app.MapControllers(); // Map controller routes to endpoints

app.Run(); // Run the application