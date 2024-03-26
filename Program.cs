using CourseManagement;
using CourseManagement.Middlewares;
using CourseManagement.Services;
using CourseManagement.Mapper;
using NSwag;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSwag.Generation.Processors.Security;
using CourseManagement.Models;
using CourseManagement.Repositories;
using Microsoft.Extensions.Caching.Memory;
using NJsonSchema;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => {
    options.Filters.Add<GlobalExceptionFilter>();
}).AddJsonOptions(options => {
    options.JsonSerializerOptions.IgnoreNullValues = true;
});

// Auto Mapper Configs
var mapperConfig = new AutoMapper.MapperConfiguration(cfg => {
    cfg.AddProfile(typeof(MappingProfile));
}).CreateMapper();

builder.Services.AddSingleton(mapperConfig);
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<AuthorRepository, AuthorRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<EnrollmentService>();
builder.Services.AddEndpointsApiExplorer();

// Add memory caching
builder.Services.AddMemoryCache();
builder.Services.AddHostedService<CacheWorker>();

builder.Services.AddOpenApiDocument(options =>
{
    options.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        Description = "Copy 'Bearer token' into field",
        In = OpenApiSecurityApiKeyLocation.Header
    });
    options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
    options.PostProcess = document =>
    {
        document.Info = new OpenApiInfo
        {
            Version = "v1",
            Title = "Course API",
            Description = "An ASP.NET Core Web API for managing Courses",
            TermsOfService = "https://localhost:7189/terms",
            Contact = new OpenApiContact
            {
                Name = "Example Contact",
            },
            License = new OpenApiLicense
            {
                Name = "Example License",
            }
        };
        foreach (var operation in document.Operations)
        {
            if (operation.Path == "/Course" && operation.Method == "post")
            {
                operation.Operation.OperationId = "CreateCourse";
                operation.Operation.Summary = "Create a new course";
                operation.Operation.Description = "Create Course. Return HTTP 409 Conflicts if course with name already exists";
                operation.Operation.Tags = new List<string> { "Course" };
                operation.Operation.RequestBody = new NSwag.OpenApiRequestBody
                {
                    Description = "Course details to create",
                    Content =  {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = JsonSchema.FromType<CourseRequest>()
                        }
                    }
                };
            }
        };
    };
});

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod()));
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetValue<string>("JWT:ValidateAudience"),
        ValidIssuer = builder.Configuration.GetValue<string>("JWT:ValidateIssuer"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

var app = builder.Build();

app.UseRequestLogging();

// Add and configure NSwag middleware
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();