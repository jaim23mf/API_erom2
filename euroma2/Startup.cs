using euroma2.Auth;
using euroma2.Models;
using euroma2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Reflection;
using System.Text;

namespace euroma2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private string MyAllowSpecificOrigins = "myCors";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            services.AddSingleton(token);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = token.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidAudience = token.Audience,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Auth",
                    Description = "API Auth",
                    Contact = new OpenApiContact
                    {
                        Name = @"Euroma2",
                        Email = string.Empty,
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath, false);

                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                // add Basic Authentication
                /*var basicSecurityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    Reference = new OpenApiReference { Id = "BasicAuth", Type = ReferenceType.SecurityScheme }
                };
                c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {basicSecurityScheme, new string[] { }}
                });*/
            });

            services.AddDbContext<DataContext>(options => options.UseMySql(
             Configuration.GetConnectionString("ConnectionString"),
                ServerVersion.AutoDetect(Configuration.GetConnectionString("ConnectionString"))
             )

            );

            services.AddScoped<IUserService, UserService>();


            var hostCors = Configuration.GetValue<string>("Cors");
            var origins = hostCors.Split(";");
            Console.WriteLine(origins);

            services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                //builder.WithOrigins("http://localhost:4200", "https://localhost:7260").AllowAnyMethod().AllowAnyHeader().AllowCredentials();

            }));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.DocumentTitle = "API Auth Demo";
                c.DefaultModelsExpandDepth(0);
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("corsapp");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}