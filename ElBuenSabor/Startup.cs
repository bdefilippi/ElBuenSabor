using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ElBuenSabor.Models;
using ElBuenSabor.Controllers;
using System.Globalization;
using Newtonsoft.Json;
using ElBuenSabor.Services;
using ElBuenSabor.Models.Common;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ElBuenSabor.Hubs;


namespace ElBuenSabor
{
    public class Startup
    {
        public const String ConnectionString = "Server=den1.mssql8.gear.host;Database=elbuensabordb;" +
                "Trusted_Connection=False;User Id=elbuensabordb; Password=#Base007;";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup()
        {
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ElBuenSaborContext>(
                options => options.UseSqlServer(ConnectionString));

            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddCors();

            //-----------jwt
            var jwtSettings = new JwtSettings();
            Configuration.Bind(key: nameof(jwtSettings), jwtSettings);

            services.AddSingleton(jwtSettings);

            services.AddAuthentication(configureOptions: x => 
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                //this will help us validate request as they come to our controllers
                x.TokenValidationParameters = new TokenValidationParameters { 
                //this wil validate the last bit of our jwt is using the secret 
                //and make sure is authentic
                    ValidateIssuerSigningKey=true,
                    //we need a bytearray, and the secret is a string, so we need to use enconding
                    IssuerSigningKey= new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secreto)),
                    //Basic jwt authentication
                    ValidateIssuer = false,
                    ValidateAudience=false,
                    RequireExpirationTime=false,
                    ValidateLifetime=true
                };
            }
             );

            /* Google y su OAuthorization */

            // Es buena practica crear un objeto que tome los valores de appsetting.json
            // y luego acceder a esos valores por medio del objeto, parece que por seguridad
            var GoogleAuthSettings = new GoogleAuthSettings();
            Configuration.Bind(key: nameof(GoogleAuthSettings), GoogleAuthSettings);

            services.AddSingleton(GoogleAuthSettings);

            //Password comun para los usuarios logeados con google
            var CommonPassSettings = new CommonPassSettings();
            Configuration.Bind(key: nameof(CommonPassSettings), CommonPassSettings);

            services.AddSingleton(CommonPassSettings);

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                IConfigurationSection googleAuthNSection =
                Configuration.GetSection("Authentication:Google"); //mmm ver, me parece que no apunta a nada

                
                googleOptions.ClientId = GoogleAuthSettings.ClientId; // ver appsettings.json
                googleOptions.ClientSecret = GoogleAuthSettings.ClientSecret; 
            });

            /*
             Permite que mediante el contructor de una clase, se pueda inyectar la dependencia
            IAuthService
            The AddScoped method registers the service with a scoped lifetime,
            the lifetime of a single request. 
             */
            services.AddScoped<IAuthService, AuthService>();

            //-----------jwt


            /*I have increased the size for Singal R and that fixed the issue for now but this is not a proper solution.
              The proper solution is to implement your own hub between client and server and process in chunks and stick it together.
              refer to : https://docs.microsoft.com/en-us/aspnet/core/signalr/streaming?view=aspnetcore-3.1
             */

            SignalRGroups signalRGroups = new();

            services.AddSingleton(signalRGroups);

            services.AddSignalR(e => {
                e.MaximumReceiveMessageSize = 102400000;
                e.EnableDetailedErrors = true;
            });

            //Din't work to avoid object cycles
            //services.AddControllers().AddJsonOptions(options =>
            //    options.JsonSerializerOptions.MaxDepth = 2
            //);
            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //En caso de problemas con las comas, descomentar esto!!

            //var cultureInfo = new CultureInfo("en-US");
            //cultureInfo.NumberFormat.NumberGroupSeparator = ",";

            //CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            //CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseDefaultFiles();

            app.UseStaticFiles();

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(options => {
                options.WithOrigins("http://localhost:8080");
                //options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowCredentials(); //no se puede habilitar cualquier credencial y cualquier origen al mismo tiempo. No se por qué
            });

            app.UseAuthentication(); //necesario para poder usar el jwt agregado authorize a los controllers
            app.UseAuthorization(); //deben estar en este orden para que no tire error

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificacionesAClienteHub>("/notificacionesHub");
                endpoints.MapControllers();
            });


        }
    }
}
