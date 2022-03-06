using MinimalCRUDWebAPI.Models;
using MinimalCRUDWebAPI.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnetion"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Routes
async Task<List<UserModel>> GetUsers(DataContext context) =>
    await context.Users.ToListAsync();

app.MapGet("/", () => "Hello World! 🔥");

app.MapGet("/users", async (DataContext context) =>
    await context.Users.ToListAsync()
);

app.MapGet("/users/{id}", async (DataContext context, int id) =>
    await context.Users.FindAsync(id) is UserModel user ?
        Results.Ok(user) :
        Results.NotFound("Sorry, user not found. :/")
);

app.MapPost("/users", async (DataContext context, UserModel user) =>
{
    context.Users.Add(user);
    await context.SaveChangesAsync();
    return Results.Ok(await GetUsers(context));
});

app.MapPut("/users/{id}", async (DataContext context, UserModel user, int id) =>
{
    var dbUser = await context.Users.FindAsync(id);
    if (dbUser == null)
        return Results.NotFound("User not found. :/");

    dbUser.FirstName = user.FirstName;
    dbUser.LastName = user.LastName;

    await context.SaveChangesAsync();

    return Results.Ok(await GetUsers(context));
});

app.MapDelete("/users/{id}", async (DataContext context, int id) =>
{
    var dbUser = await context.Users.FindAsync(id);
    if (dbUser == null)
        return Results.NotFound("User not found. :/");

    context.Users.Remove(dbUser);
    await context.SaveChangesAsync();

    return Results.Ok(await GetUsers(context));
});

app.Run();