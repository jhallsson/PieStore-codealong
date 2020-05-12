using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using codealong_pie_project.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace codealong_pie_project
{
    public class Startup
    {
        public IConfiguration Configuration { get; } //Represents a set of key/value application configuration properties.

        public Startup(IConfiguration configuration) //constructory injection? l�ser in fr�n appsettings.json
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Har sv�rt att f�rst� om jag kan skriva in samma som honom i appsettings och det funkar 
            //eller om jag beh�ver hoppa �ver steget localdb
            
            //g�r till configuration-fil, letar efter defaultconnection- connectionstr�ngen
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //enablar f�rdig funktionalitet (scaffolding) som har med User-login att g�ra (Identity)
            //basic funktionality for using identity in app 
            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IPieRepository, PieRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>(); //kan nu anv�nda riktiga konkreta implementationen ist f�r MockRepository
            services.AddScoped<IOrderRepository, OrderRepository>();

            //l�gger till servicen shoppingcart som skickar med samma service provider till metoden GetCart
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));
            
            //ger tillg�ng till httpcontext som ger tillg�ng  till sessionen
            services.AddHttpContextAccessor();
            services.AddSession();
            services.AddControllersWithViews(); //support f�r MVC (-struktur?)
            services.AddRazorPages();   //Anv�nds av scaffolded identity
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //middlewares
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();  //redirectar http-requests till https-req
            app.UseStaticFiles();       //f�r kunna anv�nda statiska objekt ex. bilder css/js-filer...
            app.UseSession();

            app.UseRouting();
            app.UseAuthentication();//vem �r registrerad
            app.UseAuthorization(); //vad �r till�ngligt f�r vem


            app.UseEndpoints(endpoints =>
            {
                //patternet f�r s�kv�gen
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //Egen enpoint f�r razor pages (med identiity sidor)
                endpoints.MapRazorPages();
            });
        }
    }
}
