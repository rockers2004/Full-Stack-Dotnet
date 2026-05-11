# Introduction to Building the E-commerce Backend with ASP.NET Core

Lesson ID: 2888

Total Sections: 10

---

## 1. Introduction to Building the E-commerce Backend with ASP.NET Core

Welcome to the first part of our end-to-end e-commerce platform implementation! In this comprehensive lab, we will dive deep into building the robust backend services and APIs that power our online store. This module is crucial for understanding how data is managed, how users are authenticated, and how the core business logic of an e-commerce application operates. We will leverage the power of C#, ASP.NET Core, and Entity Framework Core to create a scalable and secure foundation. This lesson directly supports the module's learning objectives by enabling you to **apply learned technologies in a real-world project** , **design and implement a full-stack e-commerce application** , and **integrate authentication, authorization, and data management**. The skills acquired here are fundamental for any aspiring full-stack developer, providing a solid understanding of server-side development, API design, and data persistence, which are essential in today's tech landscape.


---

## 2. Project Setup: Establishing the ASP.NET Core Web API Foundation

Our journey begins with setting up a new ASP.NET Core Web API project. This project will serve as the backbone of our e-commerce platform, handling all server-side logic, data access, and API requests. We will utilize Visual Studio 2022 for this process, ensuring we are using the latest stable versions of .NET and its associated tools.

**Key Steps:**

           1. **Create a New Project:** Open Visual Studio 2022 and select 'Create a new project'.
           2. **Select Project Template:** Choose the 'ASP.NET Core Web API' template. Ensure the .NET SDK version selected is the latest stable LTS (Long-Term Support) version, such as .NET 6 or .NET 7.
           3. **Configure Project:** Name your project (e.g., `ECommerce.Api`). Select a location for your project files.
           4. **Additional Information:** Choose the framework version. For production, it's recommended to use the latest LTS version. Select 'Use controllers' and ensure 'Enable OpenAPI support' is checked, as this will help us with API documentation and testing later. Do not enable HTTPS for local development if it causes issues, but ensure it's enabled for production.
           5. **Create Project:** Click 'Create' to generate the project structure.

Upon creation, Visual Studio will generate a basic project structure including a `Controllers` folder, `appsettings.json` for configuration, and `Program.cs` (or `Startup.cs` in older versions) for application bootstrapping. We will extensively modify and expand upon this structure.

**Best Practices:**

           * **Naming Conventions:** Adhere to standard C# naming conventions (PascalCase for classes, methods, properties; camelCase for local variables).
           * **Project Structure:** As the project grows, we will refactor it into a more organized structure (e.g., separating concerns into layers like Domain, Application, Infrastructure).
           * **Dependency Injection:** ASP.NET Core has built-in support for Dependency Injection (DI). We will leverage this extensively to manage dependencies and promote loose coupling.


---

## 3. Data Persistence with Entity Framework Core: Setting the Stage

To manage our e-commerce data (products, users, orders, etc.), we need a robust data persistence solution. Entity Framework Core (EF Core) is Microsoft's modern, cross-platform Object-Relational Mapper (ORM) for .NET. It allows us to work with a database using C# objects, abstracting away much of the direct SQL interaction.

**Installation:**

First, we need to install the necessary EF Core NuGet packages. Open the Package Manager Console in Visual Studio (Tools -> NuGet Package Manager -> Package Manager Console) and run the following commands:
    
    Install-Package Microsoft.EntityFrameworkCore
    Install-Package Microsoft.EntityFrameworkCore.SqlServer
    Install-Package Microsoft.EntityFrameworkCore.Tools

`Microsoft.EntityFrameworkCore.SqlServer` is the provider for SQL Server. If you prefer another database (e.g., PostgreSQL, SQLite), you would install the corresponding provider package.

**Database Context:**

The core of EF Core is the `DbContext` class. This class represents a session with the database and allows us to query and save data. We'll create a custom class that inherits from `DbContext`.
    
    // ECommerce.Api/Data/ECommerceDbContext.cs
    using Microsoft.EntityFrameworkCore;
    
    namespace ECommerce.Api.Data
    {
        public class ECommerceDbContext : DbContext
        {
            public ECommerceDbContext(DbContextOptions options) : base(options)
            {
            }
    
            // DbSet properties for your entities will go here
            // public DbSet Products { get; set; }
            // public DbSet Users { get; set; }
            // public DbSet Orders { get; set; }
        }
    }
    

**Configuration:**

Next, we need to configure our `DbContext` in `Program.cs` (or `Startup.cs`) and register it with the Dependency Injection container. We also need to define a connection string in `appsettings.json`.

`appsettings.json`:
    
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
      },
      "Logging": {
        // ... other logging settings
      },
      "AllowedHosts": "*"
    }

`Program.cs`:
    
    // ... other usings
    using ECommerce.Api.Data;
    using Microsoft.EntityFrameworkCore;
    
    var builder = WebApplication.CreateBuilder(args);
    
    // Add services to the container.
    builder.Services.AddControllers();
    
    // Add EF Core DbContext
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext(options =>
        options.UseSqlServer(connectionString));
    
    // Add Swagger/OpenAPI support
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
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
    

**Migrations:**

EF Core Migrations allow us to evolve our database schema over time as our application models change. To create the initial database and tables, we'll use the Package Manager Console:
    
    Add-Migration InitialCreate
    Update-Database

`Add-Migration InitialCreate` generates migration files that describe the changes to be made to the database. `Update-Database` applies these changes to the database specified in the connection string.


---

## 4. Defining Entity Models for the E-commerce Domain

Before we can create API endpoints, we need to define the data structures (entities) that represent our e-commerce domain. These C# classes will map directly to database tables using EF Core. We will focus on core entities like `Product`, `Category`, `User`, `Order`, and `OrderItem`.

**Product Entity:**

Represents an item available for sale in the store.
    
    // ECommerce.Api/Models/Product.cs
    using System.ComponentModel.DataAnnotations;
    
    namespace ECommerce.Api.Models
    {
        public class Product
        {
            [Key]
            public int Id { get; set; } // Primary Key
    
            [Required, MaxLength(100)]
            public string Name { get; set; }
    
            [MaxLength(500)]
            public string Description { get; set; }
    
            [Required, DataType(DataType.Currency)]
            public decimal Price { get; set; }
    
            [Required, Range(0, int.MaxValue)]
            public int StockQuantity { get; set; }
    
            [MaxLength(255)]
            public string ImageUrl { get; set; } // URL to the product image
    
            // Foreign Key to Category
            public int CategoryId { get; set; }
            public Category Category { get; set; }
    
            // Navigation property for OrderItems
            public ICollection OrderItems { get; set; } = new List();
        }
    }
    

**Category Entity:**

Represents a classification for products.
    
    // ECommerce.Api/Models/Category.cs
    using System.ComponentModel.DataAnnotations;
    
    namespace ECommerce.Api.Models
    {
        public class Category
        {
            [Key]
            public int Id { get; set; }
    
            [Required, MaxLength(50)]
            public string Name { get; set; }
    
            // Navigation property for Products
            public ICollection Products { get; set; } = new List();
        }
    }
    

**User Entity:**

Represents a customer or administrator.
    
    // ECommerce.Api/Models/User.cs
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    namespace ECommerce.Api.Models
    {
        public class User
        {
            [Key]
            public int Id { get; set; }
    
            [Required, MaxLength(100)]
            public string Username { get; set; }
    
            [Required, MaxLength(255)]
            public string Email { get; set; }
    
            [Required]
            public byte[] PasswordHash { get; set; } // Store hashed passwords
    
            [Required]
            public byte[] PasswordSalt { get; set; } // Salt for password hashing
    
            [Required, MaxLength(50)]
            public string Role { get; set; } = "Customer"; // e.g., 'Customer', 'Admin'
    
            // Navigation property for Orders
            public ICollection Orders { get; set; } = new List();
        }
    }
    

**Order Entity:**

Represents a customer's purchase.
    
    // ECommerce.Api/Models/Order.cs
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    namespace ECommerce.Api.Models
    {
        public class Order
        {
            [Key]
            public int Id { get; set; }
    
            [Required]
            public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
            [Required, MaxLength(50)]
            public string OrderStatus { get; set; } = "Pending"; // e.g., 'Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled'
    
            [Required, MaxLength(255)]
            public string ShippingAddress { get; set; }
    
            [Required, DataType(DataType.Currency)]
            public decimal TotalAmount { get; set; }
    
            // Foreign Key to User
            public int UserId { get; set; }
            public User User { get; set; }
    
            // Navigation property for OrderItems
            public ICollection OrderItems { get; set; } = new List();
        }
    }
    

**OrderItem Entity:**

Represents a single item within an order.
    
    // ECommerce.Api/Models/OrderItem.cs
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    namespace ECommerce.Api.Models
    {
        public class OrderItem
        {
            [Key]
            public int Id { get; set; }
    
            [Required]
            public int OrderId { get; set; } // Foreign Key to Order
            public Order Order { get; set; }
    
            [Required]
            public int ProductId { get; set; } // Foreign Key to Product
            public Product Product { get; set; }
    
            [Required, Range(1, int.MaxValue)]
            public int Quantity { get; set; }
    
            [Required, DataType(DataType.Currency)]
            public decimal PriceAtPurchase { get; set; } // Price of the product when the order was placed
        }
    }
    

**Registering Entities with DbContext:**

Update your `ECommerceDbContext` to include `DbSet` properties for each entity:
    
    // ECommerce.Api/Data/ECommerceDbContext.cs
    using Microsoft.EntityFrameworkCore;
    using ECommerce.Api.Models;
    
    namespace ECommerce.Api.Data
    {
        public class ECommerceDbContext : DbContext
        {
            public ECommerceDbContext(DbContextOptions options) : base(options)
            {
            }
    
            public DbSet Products { get; set; }
            public DbSet Categories { get; set; }
            public DbSet Users { get; set; }
            public DbSet Orders { get; set; }
            public DbSet OrderItems { get; set; }
        }
    }
    

After defining these entities, you'll need to run the EF Core migrations again to create the corresponding database tables:
    
    Add-Migration AddedEntities
    Update-Database


---

## 5. Implementing Product Catalog CRUD APIs

Now, let's build the API endpoints to manage our product catalog. We will create a `ProductsController` that handles Create, Read, Update, and Delete (CRUD) operations for products. This involves interacting with our `ECommerceDbContext`.

**Product DTOs (Data Transfer Objects):**

It's good practice to use DTOs to shape the data sent to and from the API, separating API concerns from domain models. This prevents accidental exposure of sensitive data and allows for flexible API contracts.
    
    // ECommerce.Api/DTOs/ProductDto.cs
    namespace ECommerce.Api.DTOs
    {
        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public string ImageUrl { get; set; }
            public int CategoryId { get; set; }
            public string CategoryName { get; set; } // Include category name for easier display
        }
    
        public class CreateProductDto
        {
            // Id is generated by the database
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public string ImageUrl { get; set; }
            public int CategoryId { get; set; }
        }
    
        public class UpdateProductDto
        {
            // Id is required for update
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public string ImageUrl { get; set; }
            public int CategoryId { get; set; }
        }
    }

**ProductsController:**

Create a new controller file `ProductsController.cs` in the `Controllers` folder.
    
    // ECommerce.Api/Controllers/ProductsController.cs
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ECommerce.Api.Data;
    using ECommerce.Api.Models;
    using ECommerce.Api.DTOs;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    namespace ECommerce.Api.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ProductsController : ControllerBase
        {
            private readonly ECommerceDbContext _context;
    
            public ProductsController(ECommerceDbContext context)
            {
                _context = context;
            }
    
            // GET: api/Products
            [HttpGet]
            public async Task>> GetProducts()
            {
                var products = await _context.Products
                    .Include(p => p.Category) // Eager load the category
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        ImageUrl = p.ImageUrl,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name
                    })
                    .ToListAsync();
    
                return Ok(products);
            }
    
            // GET: api/Products/5
            [HttpGet("{id}")]
            public async Task> GetProduct(int id)
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);
    
                if (product == null)
                {
                    return NotFound();
                }
    
                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ImageUrl = product.ImageUrl,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name
                };
    
                return Ok(productDto);
            }
    
            // POST: api/Products
            [HttpPost]
            public async Task> PostProduct(CreateProductDto createProductDto)
            {
                // Check if category exists
                var category = await _context.Categories.FindAsync(createProductDto.CategoryId);
                if (category == null)
                {
                    return BadRequest("Category not found.");
                }
    
                var product = new Product
                {
                    Name = createProductDto.Name,
                    Description = createProductDto.Description,
                    Price = createProductDto.Price,
                    StockQuantity = createProductDto.StockQuantity,
                    ImageUrl = createProductDto.ImageUrl,
                    CategoryId = createProductDto.CategoryId
                };
    
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
    
                // Return the created product DTO, including category name
                var createdProductDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ImageUrl = product.ImageUrl,
                    CategoryId = product.CategoryId,
                    CategoryName = category.Name
                };
    
                // Return 201 Created status code with the location of the new resource
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, createdProductDto);
            }
    
            // PUT: api/Products/5
            [HttpPut("{id}")]
            public async Task PutProduct(int id, UpdateProductDto updateProductDto)
            {
                if (id != updateProductDto.Id)
                {
                    return BadRequest("Product ID mismatch.");
                }
    
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
    
                // Check if category exists
                var category = await _context.Categories.FindAsync(updateProductDto.CategoryId);
                if (category == null)
                {
                    return BadRequest("Category not found.");
                }
    
                product.Name = updateProductDto.Name;
                product.Description = updateProductDto.Description;
                product.Price = updateProductDto.Price;
                product.StockQuantity = updateProductDto.StockQuantity;
                product.ImageUrl = updateProductDto.ImageUrl;
                product.CategoryId = updateProductDto.CategoryId;
    
                _context.Entry(product).State = EntityState.Modified;
    
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Rethrow the exception if it's not a concurrency issue we can handle
                    }
                }
    
                return NoContent(); // Successful update, no content to return
            }
    
            // DELETE: api/Products/5
            [HttpDelete("{id}")]
            public async Task DeleteProduct(int id)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
    
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
    
                return NoContent(); // Successful deletion
            }
    
            private bool ProductExists(int id)
            {
                return _context.Products.Any(e => e.Id == id);
            }
        }
    }

**Testing with Swagger:**

With OpenAPI support enabled, you can run your application and access the Swagger UI at the root URL (e.g., `https://localhost:5001/swagger`). This provides an interactive interface to test your API endpoints directly.


---

## 6. Implementing User Authentication and Authorization with JWT

Securing our e-commerce platform is paramount. We will implement user authentication and authorization using JSON Web Tokens (JWT). JWT is an open standard (RFC 7519) that defines a compact and self-contained way for securely transmitting information between parties as a JSON object. This information can be verified and trusted because it is digitally signed.

**JWT Flow:**

           1. **User Registration/Login:** A user provides credentials (username/password).
           2. **Server Validation:** The server validates the credentials.
           3. **Token Generation:** Upon successful validation, the server generates a JWT containing user information (e.g., user ID, role) and signs it with a secret key.
           4. **Token Issuance:** The server sends the JWT back to the client.
           5. **Client Storage:** The client stores the JWT (e.g., in local storage or cookies).
           6. **Subsequent Requests:** For protected API endpoints, the client includes the JWT in the `Authorization` header (typically as `Bearer `).
           7. **Server Verification:** The server verifies the JWT's signature and expiration. If valid, it extracts user information and grants access.

**Dependencies:**

Install the necessary JWT authentication packages:
    
    Install-Package Microsoft.AspNetCore.Authentication.JwtBearer

**User Entity Enhancements:**

Ensure your `User` entity has fields for storing password hashes and salts. We'll need a utility class for password hashing.
    
    // ECommerce.Api/Helpers/PasswordHasher.cs
    using System;
    using System.Security.Cryptography;
    
    namespace ECommerce.Api.Helpers
    {
        public static class PasswordHasher
        {
            private const int SaltSize = 16; // 128 bits
            private const int KeySize = 32;  // 256 bits
    
            public static byte[] HashPassword(string password, out byte[] salt)
            {
                salt = RandomNumberGenerator.GetBytes(SaltSize);
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
                {
                    return pbkdf2.GetBytes(KeySize);
                }
            }
    
            public static bool VerifyPassword(string password, byte[] salt, byte[] storedHash)
            {
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
                {
                    byte[] computedHash = pbkdf2.GetBytes(KeySize);
                    return computedHash.SequenceEqual(storedHash);
                }
            }
        }
    }
    

**Authentication Service:**

Create a service to handle user registration, login, and JWT generation.
    
    // ECommerce.Api/Services/IAuthService.cs
    namespace ECommerce.Api.Services
    {
        public interface IAuthService
        {
            Task<(bool Success, string Token, string ErrorMessage)> RegisterUser(string username, string email, string password);
            Task<(bool Success, string Token, string ErrorMessage)> LoginUser(string username, string password);
        }
    }
    
    // ECommerce.Api/Services/AuthService.cs
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using ECommerce.Api.Data;
    using ECommerce.Api.Models;
    using ECommerce.Api.Helpers;
    
    namespace ECommerce.Api.Services
    {
        public class AuthService : IAuthService
        {
            private readonly ECommerceDbContext _context;
            private readonly IConfiguration _configuration;
    
            public AuthService(ECommerceDbContext context, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
            }
    
            public async Task<(bool Success, string Token, string ErrorMessage)> RegisterUser(string username, string email, string password)
            {
                if (await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower() || u.Email.ToLower() == email.ToLower()))
                {
                    return (false, null, "Username or email already exists.");
                }
    
                var salt = RandomNumberGenerator.GetBytes(16);
                var passwordHash = PasswordHasher.HashPassword(password, out salt);
    
                var newUser = new User
                {
                    Username = username,
                    Email = email,
                    PasswordHash = passwordHash,
                    PasswordSalt = salt,
                    Role = "Customer" // Default role
                };
    
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
    
                var token = GenerateJwtToken(newUser);
                return (true, token, null);
            }
    
            public async Task<(bool Success, string Token, string ErrorMessage)> LoginUser(string username, string password)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    
                if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
                {
                    return (false, null, "Invalid username or password.");
                }
    
                var token = GenerateJwtToken(user);
                return (true, token, null);
            }
    
            private string GenerateJwtToken(User user)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
    
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30), // Token expiration time
                    signingCredentials: credentials);
    
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
    

**Configuration in`appsettings.json`:**
    
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
      },
      "Jwt": {
        "Key": "YOUR_SUPER_SECRET_KEY_REPLACE_THIS_IN_PRODUCTION_WITH_A_STRONG_KEY",
        "Issuer": "YourApiIssuer",
        "Audience": "YourApiAudience"
      },
      "Logging": {
        // ...
      },
      "AllowedHosts": "*"
    }

**Registering Services in`Program.cs`:**
    
    // ... inside Program.cs
    
    // Add Authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });
    
    // Register Auth Service and DbContext
    builder.Services.AddDbContext(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped();
    
    // Add Authorization
    builder.Services.AddAuthorization(); // Basic authorization setup
    
    // ... rest of Program.cs
    
    // Add UseAuthentication and UseAuthorization middleware
    app.UseAuthentication();
    app.UseAuthorization();
    

**Authentication Controller:**

Create an `AuthController` to handle registration and login requests.
    
    // ECommerce.Api/Controllers/AuthController.cs
    using Microsoft.AspNetCore.Mvc;
    using ECommerce.Api.Services;
    using ECommerce.Api.DTOs;
    using System.Threading.Tasks;
    
    namespace ECommerce.Api.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly IAuthService _authService;
    
            public AuthController(IAuthService authService)
            {
                _authService = authService;
            }
    
            [HttpPost("register")]
            public async Task Register(UserRegisterDto userRegisterDto)
            {
                var (success, token, errorMessage) = await _authService.RegisterUser(userRegisterDto.Username, userRegisterDto.Email, userRegisterDto.Password);
    
                if (!success)
                {
                    return BadRequest(new { message = errorMessage });
                }
    
                return Ok(new { token });
            }
    
            [HttpPost("login")]
            public async Task Login(UserLoginDto userLoginDto)
            {
                var (success, token, errorMessage) = await _authService.LoginUser(userLoginDto.Username, userLoginDto.Password);
    
                if (!success)
                {
                    return Unauthorized(new { message = errorMessage });
                }
    
                return Ok(new { token });
            }
        }
    }
    

**Auth DTOs:**
    
    // ECommerce.Api/DTOs/AuthDtos.cs
    namespace ECommerce.Api.DTOs
    {
        public class UserRegisterDto
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
    
        public class UserLoginDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }

**Securing Endpoints:**

To protect endpoints, add the `[Authorize]` attribute. You can also specify roles using `[Authorize(Roles = "Admin")]`.
    
    // Example: Protect the PUT and DELETE endpoints for products
    [HttpPut("{id}")]
    [Authorize] // Requires authentication
    public async Task PutProduct(int id, UpdateProductDto updateProductDto)
    {
        // ... implementation ...
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Requires authentication and Admin role
    public async Task DeleteProduct(int id)
    {
        // ... implementation ...
    }
    


---

## 7. Developing API Endpoints for Shopping Cart Management

The shopping cart is a core feature of any e-commerce platform. Users need to be able to add items, view their cart, update quantities, and remove items. We will implement these functionalities using a dedicated `CartController`.

**Cart DTOs:**

We'll define DTOs to represent the shopping cart and its items.
    
    // ECommerce.Api/DTOs/CartDtos.cs
    using System.Collections.Generic;
    using System.Linq;
    
    namespace ECommerce.Api.DTOs
    {
        public class CartItemDto
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public string ImageUrl { get; set; }
            public decimal Subtotal => Price * Quantity;
        }
    
        public class ShoppingCartDto
        {
            public List Items { get; set; } = new List();
            public decimal TotalAmount => Items.Sum(item => item.Subtotal);
            public int TotalItems => Items.Count;
        }
    
        public class AddToCartDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; } = 1;
        }
    
        public class UpdateCartItemDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }

**CartController:**

This controller will manage the user's shopping cart. For simplicity in this example, we'll simulate the cart using a dictionary in memory, keyed by user ID. In a real-world application, you might store this in a database or a dedicated caching service.
    
    // ECommerce.Api/Controllers/CartController.cs
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ECommerce.Api.Data;
    using ECommerce.Api.DTOs;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    
    namespace ECommerce.Api.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        [Authorize] // All cart operations require authentication
        public class CartController : ControllerBase
        {
            private readonly ECommerceDbContext _context;
            // In-memory store for carts (replace with database/cache in production)
            private static readonly Dictionary> _userCarts = new Dictionary>();
    
            public CartController(ECommerceDbContext context)
            {
                _context = context;
            }
    
            private int GetCurrentUserId()
            {
                // Get user ID from JWT claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
                // Fallback or throw error if user ID cannot be determined
                throw new UnauthorizedAccessException("User not authenticated.");
            }
    
            [HttpGet]
            public async Task> GetCart()
            {
                var userId = GetCurrentUserId();
                var cart = GetOrCreateUserCart(userId);
    
                var cartDto = new ShoppingCartDto();
                var productIds = cart.Keys.ToList();
    
                // Fetch product details efficiently
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id, p => new { p.Id, p.Name, p.Price, p.ImageUrl });
    
                foreach (var productId in cart.Keys)
                {
                    if (products.TryGetValue(productId, out var productInfo))
                    {
                        cartDto.Items.Add(new CartItemDto
                        {
                            ProductId = productId,
                            ProductName = productInfo.Name,
                            Price = productInfo.Price,
                            Quantity = cart[productId],
                            ImageUrl = productInfo.ImageUrl
                        });
                    }
                }
    
                return Ok(cartDto);
            }
    
            [HttpPost("add")]
            public async Task AddItemToCart(AddToCartDto addToCartDto)
            {
                var userId = GetCurrentUserId();
                var product = await _context.Products.FindAsync(addToCartDto.ProductId);
    
                if (product == null)
                {
                    return NotFound("Product not found.");
                }
    
                if (product.StockQuantity < addToCartDto.Quantity)
                {
                    return BadRequest("Insufficient stock.");
                }
    
                var cart = GetOrCreateUserCart(userId);
    
                if (cart.ContainsKey(addToCartDto.ProductId))
                {
                    // If item already exists, update quantity
                    cart[addToCartDto.ProductId] += addToCartDto.Quantity;
                }
                else
                {
                    // Add new item to cart
                    cart.Add(addToCartDto.ProductId, addToCartDto.Quantity);
                }
    
                // Update stock quantity immediately (or handle this during checkout)
                // For simplicity, we'll decrement stock here. A more robust solution might use transactions.
                product.StockQuantity -= addToCartDto.Quantity;
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
    
                return Ok(); // Or return the updated cart
            }
    
            [HttpPut("update")]
            public async Task UpdateCartItem(UpdateCartItemDto updateCartItemDto)
            {
                var userId = GetCurrentUserId();
                var cart = GetOrCreateUserCart(userId);
    
                if (!cart.ContainsKey(updateCartItemDto.ProductId))
                {
                    return NotFound("Item not found in cart.");
                }
    
                var product = await _context.Products.FindAsync(updateCartItemDto.ProductId);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }
    
                var currentQuantity = cart[updateCartItemDto.ProductId];
                var quantityDifference = updateCartItemDto.Quantity - currentQuantity;
    
                if (product.StockQuantity < quantityDifference)
                {
                    return BadRequest("Insufficient stock for the updated quantity.");
                }
    
                if (updateCartItemDto.Quantity <= 0)
                {
                    // Remove item if quantity is zero or less
                    cart.Remove(updateCartItemDto.ProductId);
                    // Adjust stock back
                    product.StockQuantity += currentQuantity;
                }
                else
                {
                    // Update quantity and stock
                    cart[updateCartItemDto.ProductId] = updateCartItemDto.Quantity;
                    product.StockQuantity -= quantityDifference;
                }
    
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
    
                return Ok(); // Or return the updated cart
            }
    
            [HttpDelete("remove/{productId}")]
            public async Task RemoveItemFromCart(int productId)
            {
                var userId = GetCurrentUserId();
                var cart = GetOrCreateUserCart(userId);
    
                if (!cart.ContainsKey(productId))
                {
                    return NotFound("Item not found in cart.");
                }
    
                var currentQuantity = cart[productId];
                cart.Remove(productId);
    
                // Adjust stock back
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    product.StockQuantity += currentQuantity;
                    _context.Entry(product).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
    
                return NoContent(); // Successful removal
            }
    
            [HttpDelete("clear")]
            public async Task ClearCart()
            {
                var userId = GetCurrentUserId();
                if (_userCarts.ContainsKey(userId))
                {
                    // Restore stock for all items in the cart before clearing
                    var cart = _userCarts[userId];
                    foreach (var cartItem in cart)
                    {
                        var product = await _context.Products.FindAsync(cartItem.Key);
                        if (product != null)
                        {
                            product.StockQuantity += cartItem.Value;
                            _context.Entry(product).State = EntityState.Modified;
                        }
                    }
                    await _context.SaveChangesAsync();
                    _userCarts.Remove(userId);
                }
                return NoContent();
            }
    
            private Dictionary GetOrCreateUserCart(int userId)
            {
                if (!_userCarts.TryGetValue(userId, out var cart))
                {
                    cart = new Dictionary();
                    _userCarts.Add(userId, cart);
                }
                return cart;
            }
        }
    }

**Important Considerations:**

           * **State Management:** The in-memory dictionary is a simplification. For production, use a database table (e.g., `Cart` and `CartItem` entities) or a distributed cache like Redis.
           * **Stock Management:** Decrementing stock immediately upon adding to the cart can lead to race conditions or incorrect stock counts if the order isn't completed. A more robust approach involves reserving stock during checkout or using transactional operations.
           * **Concurrency:** Implement proper concurrency control mechanisms (e.g., using EF Core's concurrency tokens) if multiple requests can modify the same product stock simultaneously.


---

## 8. Implementing Order Processing Logic

The culmination of backend development in an e-commerce application is the order processing logic. This involves taking items from the shopping cart, creating an `Order` and associated `OrderItem` records, and finalizing the transaction. This process should be atomic and handle potential failures gracefully to ensure data integrity.

### **Order Processing Service**

We'll create a service to encapsulate the order processing logic. This service will interact with the `ECommerceDbContext` and potentially other services (like payment processing, though we will simulate that here).

#### **IOrderService Interface**
    
    // ECommerce.Api/Services/IOrderService.cs
    using ECommerce.Api.DTOs;
    using System.Threading.Tasks;
    
    namespace ECommerce.Api.Services
    {
        public interface IOrderService
        {
            Task<OrderDto> CreateOrderAsync(int userId, string shippingAddress, Dictionary<int, int> cartItems);
            Task<OrderDto> GetOrderByIdAsync(int orderId, int userId);
            Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);
        }
    }
    

#### **OrderService Implementation**
    
    // ECommerce.Api/Services/OrderService.cs
    using Microsoft.EntityFrameworkCore;
    using ECommerce.Api.Data;
    using ECommerce.Api.Models;
    using ECommerce.Api.DTOs;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    namespace ECommerce.Api.Services
    {
        public class OrderService : IOrderService
        {
            private readonly ECommerceDbContext _context;
    
            public OrderService(ECommerceDbContext context)
            {
                _context = context;
            }
    
            public async Task<OrderDto> CreateOrderAsync(int userId, string shippingAddress, Dictionary<int, int> cartItems)
            {
                // Use a transaction to ensure atomicity
                using var transaction = await _context.Database.BeginTransactionAsync();
    
                try
                {
                    var order = new Order
                    {
                        UserId = userId,
                        OrderDate = DateTime.UtcNow,
                        ShippingAddress = shippingAddress,
                        OrderStatus = "Pending", // Initial status
                        TotalAmount = 0 // Will be calculated
                    };
    
                    decimal totalOrderAmount = 0;
                    var orderItems = new List<OrderItem>();
    
                    foreach (var cartItem in cartItems)
                    {
                        var productId = cartItem.Key;
                        var quantity = cartItem.Value;
    
                        var product = await _context.Products.FindAsync(productId);
                        if (product == null)
                        {
                            throw new Exception($"Product with ID {productId} not found.");
                        }
    
                        if (product.StockQuantity < quantity)
                        {
                            throw new Exception($"Insufficient stock for product {product.Name}. Available: {product.StockQuantity}, Requested: {quantity}.");
                        }
    
                        var priceAtPurchase = product.Price;
                        var orderItemSubtotal = priceAtPurchase * quantity;
    
                        orderItems.Add(new OrderItem
                        {
                            ProductId = productId,
                            Quantity = quantity,
                            PriceAtPurchase = priceAtPurchase,
                            Order = order // Link OrderItem to the Order
                        });
    
                        totalOrderAmount += orderItemSubtotal;
                        product.StockQuantity -= quantity; // Decrement stock
                        _context.Entry(product).State = EntityState.Modified;
                    }
    
                    order.OrderItems = orderItems;
                    order.TotalAmount = totalOrderAmount;
    
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();
    
                    // Clear the user's cart after successful order creation
                    // Note: This assumes cart is managed externally or cleared by the controller
    
                    await transaction.CommitAsync();
    
                    // Return a DTO representation of the created order
                    return await GetOrderDtoAsync(order.Id, userId);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Log the exception
                    throw new Exception("Order processing failed: " + ex.Message);
                }
            }
    
            public async Task<OrderDto> GetOrderByIdAsync(int orderId, int userId)
            {
                // Ensure the user owns the order
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
    
                if (order == null)
                {
                    return null; // Or throw NotFoundException
                }
    
                return MapOrderToDto(order);
            }
    
            public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
            {
                var orders = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .ToListAsync();
    
                return orders.Select(MapOrderToDto);
            }
    
            private OrderDto MapOrderToDto(Order order)
            {
                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    OrderStatus = order.OrderStatus,
                    ShippingAddress = order.ShippingAddress,
                    TotalAmount = order.TotalAmount,
                    UserId = order.UserId,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        PriceAtPurchase = oi.PriceAtPurchase,
                        Subtotal = oi.PriceAtPurchase * oi.Quantity
                    }).ToList()
                };
                return orderDto;
            }
    
            // Helper to get OrderDto, useful for CreateOrderAsync return
            private async Task<OrderDto> GetOrderDtoAsync(int orderId, int userId)
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
    
                if (order == null) return null;
                return MapOrderToDto(order);
            }
        }
    }
    

### **Order DTOs**

These are the data transfer objects (DTOs) used for transferring order-related data between the backend and frontend.
    
    // ECommerce.Api/DTOs/OrderDtos.cs
    using System;
    using System.Collections.Generic;
    
    namespace ECommerce.Api.DTOs
    {
        public class OrderItemDto
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal PriceAtPurchase { get; set; }
            public decimal Subtotal => PriceAtPurchase * Quantity;
        }
    
        public class OrderDto
        {
            public int Id { get; set; }
            public DateTime OrderDate { get; set; }
            public string OrderStatus { get; set; }
            public string ShippingAddress { get; set; }
            public decimal TotalAmount { get; set; }
            public int UserId { get; set; }
            public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        }
    
        public class CreateOrderRequestDto
        {
            public string ShippingAddress { get; set; }
            // Cart items will be fetched from the user's session/cart service
        }
    }
    

### **Orders Controller**

Create an `OrdersController` to expose order-related API endpoints.
    
    // ECommerce.Api/Controllers/OrdersController.cs
    using Microsoft.AspNetCore.Mvc;
    using ECommerce.Api.Services;
    using ECommerce.Api.DTOs;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    
    namespace ECommerce.Api.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        [Authorize] // All order operations require authentication
        public class OrdersController : ControllerBase
        {
            private readonly IOrderService _orderService;
            private readonly CartController _cartController; // Inject CartController to access cart data
    
            public OrdersController(IOrderService orderService, CartController cartController)
            {
                _orderService = orderService;
                _cartController = cartController;
            }
    
            private int GetCurrentUserId()
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
                throw new UnauthorizedAccessException("User not authenticated.");
            }
    
            [HttpPost("create")]
            public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderRequestDto requestDto)
            {
                var userId = GetCurrentUserId();
    
                // Get current cart items from the CartController
                var cartResult = await _cartController.GetCart();
                if (cartResult.Result is NotFoundResult || cartResult.Value == null)
                {
                    return BadRequest("Your shopping cart is empty.");
                }
                var cartDto = cartResult.Value;
    
                if (!cartDto.Items.Any())
                {
                    return BadRequest("Your shopping cart is empty.");
                }
    
                // Convert cart items to the format expected by OrderService
                var cartItemsForOrder = new Dictionary<int, int>();
                foreach (var item in cartDto.Items)
                {
                    cartItemsForOrder.Add(item.ProductId, item.Quantity);
                }
    
                try
                {
                    var createdOrder = await _orderService.CreateOrderAsync(userId, requestDto.ShippingAddress, cartItemsForOrder);
    
                    // Clear the cart after successful order creation
                    await _cartController.ClearCart();
    
                    return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
                }
                catch (Exception ex)
                {
                    // Log the exception details
                    return StatusCode(500, new { message = "An error occurred during order processing.", details = ex.Message });
                }
            }
    
            [HttpGet("{orderId}")]
            public async Task<ActionResult<OrderDto>> GetOrderById(int orderId)
            {
                var userId = GetCurrentUserId();
                var order = await _orderService.GetOrderByIdAsync(orderId, userId);
    
                if (order == null)
                {
                    return NotFound("Order not found or you do not have access to it.");
                }
    
                return Ok(order);
            }
    
            [HttpGet]
            public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrders()
            {
                var userId = GetCurrentUserId();
                var orders = await _orderService.GetUserOrdersAsync(userId);
                return Ok(orders);
            }
        }
    }
    

### **Registration in Program.cs**

Make sure to register the `OrderService` in `Program.cs`.
    
    // ... inside Program.cs
    builder.Services.AddScoped<IOrderService, OrderService>();
    // Note: CartController is typically not registered directly as a service for injection.
    // Its functionality is accessed via its API endpoints. For direct service access,
    // you would create a CartService and inject that instead.
    

### **Key Aspects of Order Processing**

           * **Atomicity:** Using database transactions ensures that either all operations (stock update, order creation, order item creation) succeed, or none of them do, preventing data inconsistencies.

           * **Error Handling:** Catching exceptions and rolling back the transaction is crucial for maintaining data integrity.

           * **Stock Management:** The stock is decremented as part of the transaction. If the transaction fails, the stock is effectively unchanged due to the rollback.

           * **Cart Clearing:** The shopping cart is cleared only after the order is successfully created and committed to the database.


---

## 9. Hands-On Lab: Building Core Backend Services and Repositories

This section guides you through the practical implementation of the core backend services and repositories discussed. You will translate the theoretical concepts into working code within your ASP.NET Core Web API project.

**Objective:** To build the foundational backend components for data persistence, product management, and user authentication.

**Steps:**

           1. **Project Setup:** Ensure your ASP.NET Core Web API project is created with the correct .NET SDK version and necessary NuGet packages (EF Core, JWT Bearer).
           2. **Database Context and Entities:** Define your `DbContext`, entity classes (`Product`, `Category`, `User`, `Order`, `OrderItem`), and register the `DbContext` in `Program.cs`. Run initial migrations (`Add-Migration InitialCreate`, `Update-Database`).
           3. **Repository Pattern (Optional but Recommended):** For better separation of concerns, consider implementing a basic repository pattern. Create interfaces (e.g., `IProductRepository`) and concrete implementations (e.g., `ProductRepository`) that wrap your `DbContext` operations. Register these repositories with the DI container.
           4. **DTOs:** Create DTOs for products, users (for auth), cart items, and orders.
           5. **Product CRUD API:** Implement the `ProductsController` with endpoints for `GET` (all and by ID), `POST`, `PUT`, and `DELETE` operations. Ensure you use DTOs for request/response bodies and handle potential errors (e.g., product not found, invalid input).
           6. **Authentication Service & Controller:** Implement the `AuthService` for user registration and login, including password hashing. Configure JWT authentication in `Program.cs`. Create the `AuthController` to handle registration and login requests, returning JWTs upon success.
           7. **Cart Service & Controller:** Implement the logic for managing the shopping cart. For this lab, use the in-memory dictionary approach. Create the `CartController` with endpoints for adding, updating, removing items, and viewing the cart. Ensure these endpoints are protected by `[Authorize]`.
           8. **Order Service & Controller:** Implement the `OrderService` with transactional logic for creating orders, updating stock, and clearing the cart. Create the `OrdersController` with endpoints for creating orders (which will utilize the cart data), retrieving a specific order, and listing user orders.
           9. **Testing:** Use Swagger UI extensively to test each API endpoint. Verify that data is correctly persisted, authentication works, cart operations are reflected, and orders can be created successfully. Test edge cases like insufficient stock, invalid inputs, and unauthorized access.

**Code Snippets:** Refer to the detailed code examples provided in the previous sections for guidance on implementing each component.


---

## 10. Hands-On Lab: Developing and Testing Authentication and Product Management APIs

This lab focuses on the practical development and rigorous testing of the authentication and product management APIs. You will ensure these critical components are robust and function as expected.

**Objective:** To build and thoroughly test the user authentication/authorization system and the product catalog API endpoints.

**Steps:**

           1. **User Registration and Login Testing:**
              * Use Swagger UI to send `POST` requests to the `/api/Auth/register` endpoint with valid and invalid user data (e.g., duplicate usernames, missing fields). Verify the responses (success messages, error messages).
              * Use Swagger UI to send `POST` requests to the `/api/Auth/login` endpoint with correct and incorrect credentials. Observe the returned JWT tokens for successful logins and appropriate error responses (e.g., `401 Unauthorized`) for failed attempts.
           2. **Product Management API Testing:**
              * **Create Products:** Use Swagger UI to send `POST` requests to `/api/Products` to create several products. Ensure you include valid data, including `CategoryId`.
              * **Retrieve Products:** Send `GET` requests to `/api/Products` to fetch all products and to `/api/Products/{id}` to fetch individual products. Verify the returned data matches what was created.
              * **Update Products:** Send `PUT` requests to `/api/Products/{id}` to modify existing products. Test updating various fields. Ensure you use a valid JWT in the `Authorization` header for this protected endpoint. Verify the changes by fetching the product again.
              * **Delete Products:** Send `DELETE` requests to `/api/Products/{id}` to remove products. Ensure you are authenticated with an appropriate role (if role-based access is implemented for deletion). Verify the product is no longer retrievable.
              * **Error Handling:** Test scenarios like trying to update or delete a non-existent product, or attempting to create a product with an invalid `CategoryId`. Ensure appropriate `404 Not Found` or `400 Bad Request` responses are returned.
              * **Authorization Testing:** Attempt to access protected endpoints (like `PUT` and `DELETE` on products) without a valid JWT. Confirm you receive a `401 Unauthorized` response. If role-based authorization is implemented, test accessing endpoints that require specific roles (e.g., Admin) with a user who does not have that role, expecting a `403 Forbidden` response.
           3. **Code Review and Refinement:** Review your code for clarity, efficiency, and adherence to best practices. Ensure proper use of async/await, dependency injection, and DTOs.

**Tools:**

           * Visual Studio 2022
           * Swagger UI (integrated with ASP.NET Core)
           * Postman or similar API testing tools (optional, for more complex scenarios)


---

