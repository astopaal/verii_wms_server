using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Mappings;
using WMS_WEBAPI.Repositories;
using WMS_WEBAPI.Services;
using WMS_WEBAPI.UnitOfWork;
using WMS_WEBAPI.Middleware;
using WMS_WEBAPI.Hubs;
using System.Security.Claims;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// SignalR Configuration
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

// Entity Framework Configuration - Using SQL Server for WMS
builder.Services.AddDbContext<WmsDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=wms.db";
    options.UseSqlServer(connectionString);
});

// ERP Database Configuration - Using SQL Server
builder.Services.AddDbContext<ErpDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ErpConnection");
    options.UseSqlServer(connectionString);
});

// AutoMapper Configuration - Automatically discover all mapping profiles in the assembly
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Hangfire Configuration
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

// Add Hangfire server
builder.Services.AddHangfireServer(options =>
{
    options.Queues = new[] { "default", "email", "reset-pass-mail" };
});

// Register Core Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register Authentication & Authorization Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserAuthorityService, UserAuthorityService>();

// Register Localization Services
builder.Services.AddScoped<ILocalizationService, LocalizationService>();

// Register ERP Services
builder.Services.AddScoped<IErpService, ErpService>();

// Register Platform Services
builder.Services.AddScoped<IPlatformPageGroupService, PlatformPageGroupService>();
builder.Services.AddScoped<ISidebarmenuHeaderService, SidebarmenuHeaderService>();
builder.Services.AddScoped<ISidebarmenuLineService, SidebarmenuLineService>();
builder.Services.AddScoped<IPlatformUserGroupMatchService, PlatformUserGroupMatchService>();

// Register Mobile Services
builder.Services.AddScoped<IMobilePageGroupService, MobilePageGroupService>();
builder.Services.AddScoped<IMobileUserGroupMatchService, MobileUserGroupMatchService>();
builder.Services.AddScoped<IMobilemenuHeaderService, MobilemenuHeaderService>();
builder.Services.AddScoped<IMobilemenuLineService, MobilemenuLineService>();

// Register Good Receipt Services
builder.Services.AddScoped<IGrHeaderService, GrHeaderService>();
builder.Services.AddScoped<IGrHeaderService, GrHeaderService>();
builder.Services.AddScoped<IGrLineService, GrLineService>();
builder.Services.AddScoped<IGrImportDocumentService, GrImportDocumentService>();
builder.Services.AddScoped<IGrImportLService, GrImportLService>();
builder.Services.AddScoped<IGrRouteService, GrRouteService>();
builder.Services.AddScoped<IGrLineSerialService, GrLineSerialService>();


// Register Warehouse Transfer Services (kept disabled to avoid build issues)
builder.Services.AddScoped<IWtFunctionService,WtFunctionService>();
builder.Services.AddScoped<IWtHeaderService,WtHeaderService>();
builder.Services.AddScoped<IWtLineService, WtLineService>();
builder.Services.AddScoped<IWtImportLineService, WtImportLineService>();
builder.Services.AddScoped<IWtLineSerialService, WtLineSerialService>();
builder.Services.AddScoped<IWtRouteService, WtRouteService>();
builder.Services.AddScoped<IWtTerminalLineService, WtTerminalLineService>();


// Register Product Transfer Services
builder.Services.AddScoped<IPtHeaderService, PtHeaderService>();
builder.Services.AddScoped<IPtLineService, PtLineService>();
builder.Services.AddScoped<IPtImportLineService, PtImportLineService>();
builder.Services.AddScoped<IPtRouteService, PtRouteService>();
builder.Services.AddScoped<IPtTerminalLineService, PtTerminalLineService>();
builder.Services.AddScoped<IPtLineSerialService, PtLineSerialService>();

// Register Production Services
builder.Services.AddScoped<IPrHeaderService, PrHeaderService>();
builder.Services.AddScoped<IPrLineService, PrLineService>();
builder.Services.AddScoped<IPrImportLineService, PrImportLineService>();
builder.Services.AddScoped<IPrRouteService, PrRouteService>();
builder.Services.AddScoped<IPrTerminalLineService, PrTerminalLineService>();
builder.Services.AddScoped<IPrLineSerialService, PrLineSerialService>();

// Register Subcontracting Issue Transfer Services
builder.Services.AddScoped<ISitHeaderService, SitHeaderService>();
builder.Services.AddScoped<ISitLineService, SitLineService>();
builder.Services.AddScoped<ISitImportLineService, SitImportLineService>();
builder.Services.AddScoped<ISitRouteService, SitRouteService>();
builder.Services.AddScoped<ISitTerminalLineService, SitTerminalLineService>();
builder.Services.AddScoped<ISitLineSerialService, SitLineSerialService>();

// Register Subcontracting Receipt Transfer Services
builder.Services.AddScoped<ISrtHeaderService, SrtHeaderService>();
builder.Services.AddScoped<ISrtLineService, SrtLineService>();
builder.Services.AddScoped<ISrtImportLineService, SrtImportLineService>();
builder.Services.AddScoped<ISrtRouteService, SrtRouteService>();
builder.Services.AddScoped<ISrtTerminalLineService, SrtTerminalLineService>();
builder.Services.AddScoped<ISrtFunctionService, SrtFunctionService>();
builder.Services.AddScoped<ISrtLineSerialService, SrtLineSerialService>();

// Register Warehouse Outbound Services
builder.Services.AddScoped<IWoHeaderService, WoHeaderService>();
builder.Services.AddScoped<IWoLineService, WoLineService>();
builder.Services.AddScoped<IWoImportLineService, WoImportLineService>();
builder.Services.AddScoped<IWoRouteService, WoRouteService>();
builder.Services.AddScoped<IWoTerminalLineService, WoTerminalLineService>();
builder.Services.AddScoped<IWoLineSerialService, WoLineSerialService>();

// Register Warehouse Inbound Services
builder.Services.AddScoped<IWiHeaderService, WiHeaderService>();
builder.Services.AddScoped<IWiLineService, WiLineService>();
builder.Services.AddScoped<IWiImportLineService, WiImportLineService>();
builder.Services.AddScoped<IWiRouteService, WiRouteService>();
builder.Services.AddScoped<IWiTerminalLineService, WiTerminalLineService>();
builder.Services.AddScoped<IWiLineSerialService, WiLineSerialService>();

// Register Shipping Services
builder.Services.AddScoped<IShHeaderService, ShHeaderService>();
builder.Services.AddScoped<IShFunctionService, ShFunctionService>();
builder.Services.AddScoped<IShLineService, ShLineService>();
builder.Services.AddScoped<IShImportLineService, ShImportLineService>();
builder.Services.AddScoped<IShRouteService, ShRouteService>();
builder.Services.AddScoped<IShTerminalLineService, ShTerminalLineService>();
builder.Services.AddScoped<IShLineSerialService, ShLineSerialService>();


// Register Function Services
builder.Services.AddScoped<IGoodReciptFunctionsService, GoodReciptFunctionsService>();
builder.Services.AddScoped<IWoFunctionService, WoFunctionService>();
builder.Services.AddScoped<IWiFunctionService, WiFunctionService>();

// Register Background Job Services
builder.Services.AddScoped<BackgroundJobService>();

// Register Notification Services
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IResetPasswordEmailJob, WMS_WEBAPI.Services.Jobs.ResetPasswordEmailJob>();

// Add HttpContextAccessor for accessing HTTP context in services
builder.Services.AddHttpContextAccessor();

// Localization Configuration
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");



// Request Localization Configuration
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("tr-TR"),
        new CultureInfo("de-DE"),
        new CultureInfo("fr-FR"),
        new CultureInfo("es-ES"),
        new CultureInfo("it-IT")
    };

    options.DefaultRequestCulture = new RequestCulture("tr-TR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// CORS Configuration (Allow all)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });

    options.AddPolicy("DevCors", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173",
                "http://127.0.0.1:5173",
                "https://127.0.0.1:5173"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "WMS_API",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "WMS_Client",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["Jwt:SecretKey"] ?? "WMS_SECRET_KEY_FOR_JWT_TOKEN_GENERATION_2024")),
        ClockSkew = TimeSpan.Zero
    };
    
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            
            // SignalR Hub için token yakala
            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/authHub") || path.StartsWithSegments("/notificationHub")))
            {
                context.Token = accessToken;
            }
            
            return Task.CompletedTask;
        },
        OnTokenValidated = async context =>
        {
            var db = context.HttpContext.RequestServices.GetRequiredService<WmsDbContext>();
            var claims = context.Principal?.Claims;
            var userId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
            {
                context.Fail("Token geçersiz: eksik kullanıcı ID");
                return;
            }
            
            // Kullanıcının aktif session'ı var mı kontrol et
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string? rawToken = null;
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                rawToken = authHeader.Substring("Bearer ".Length).Trim();
            }
            string? tokenHash = null;
            if (!string.IsNullOrEmpty(rawToken))
            {
                using var sha256Hash = System.Security.Cryptography.SHA256.Create();
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
                var builderStr = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builderStr.Append(bytes[i].ToString("x2"));
                }
                tokenHash = builderStr.ToString();
            }

            var session = await db.UserSessions
                .FirstOrDefaultAsync(s => s.UserId.ToString() == userId && s.RevokedAt == null && (tokenHash != null && s.Token == tokenHash));
            
            if (session == null)
            {
                context.Fail("Token geçersiz veya oturum kapandı");
            }
        }
    };
});

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WMS API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Seed data initialization
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WmsDbContext>();
    SeedData.Initialize(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WMS API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseRouting();
app.UseCors("DevCors");

// Hangfire Dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new WMS_WEBAPI.HangfireAuthorizationFilter() }
});

// X-Language header'ını okuyacak custom middleware
app.UseMiddleware<LanguageMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Endpoint mapping
app.MapControllers();
app.MapHub<AuthHub>("/authHub");
app.MapHub<NotificationHub>("/notificationHub");

app.Run();
