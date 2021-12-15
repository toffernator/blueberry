using blueberry.Server.Common;
using blueberry.Server.Model;
using System.CommandLine;
using System.CommandLine.Invocation;

var connectionStringOption =
    new System.CommandLine.Option<string>(
        "--connectionString",
        description: "Optionally provide a connection string through commandline arguments instead of env-variable"
    );
connectionStringOption.AddAlias("--cs");

var seedCommand = new System.CommandLine.Command(
    "seed",
    description: "Seed the database with dummy data"
);

var rootCommand = new RootCommand {
    connectionStringOption,
    seedCommand
};

seedCommand.Handler = CommandHandler.Create<string?>((connectionStringFromArgs) =>
{
    var builder = WebApplication.CreateBuilder(args);

    string connectionString = ConnectionString.Read(builder.Configuration.GetConnectionString("Blueberry"), 
                                                    Environment.GetEnvironmentVariable("ConnectionString"),
                                                    connectionStringFromArgs);
    
    builder.Services.AddDbContext<BlueberryContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddScoped<IBlueberryContext, BlueberryContext>();
    builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ITagRepository, TagRepository>();
    builder.Services.AddScoped<ISearch, SearchProxy>();

    var app = builder.Build();

    app.Seed();
});

rootCommand.Handler = CommandHandler.Create<string?>((connectionStringFromArgs) =>
{
    var builder = WebApplication.CreateBuilder(args);
    
    string connectionString = ConnectionString.Read(builder.Configuration.GetConnectionString("Blueberry"), 
                                                    Environment.GetEnvironmentVariable("ConnectionString"),
                                                    connectionStringFromArgs);
    // Add services to the container.
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

    builder.Services.AddDbContext<BlueberryContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddScoped<IBlueberryContext, BlueberryContext>();
    builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ITagRepository, TagRepository>();
    builder.Services.AddScoped<ISearch, SearchProxy>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        // app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();


    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile("index.html");

    app.Run();
});

return rootCommand.Invoke(args);
