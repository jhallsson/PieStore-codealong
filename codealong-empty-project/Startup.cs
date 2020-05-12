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

        public Startup(IConfiguration configuration) //constructory injection? läser in från appsettings.json
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Har svårt att förstå om jag kan skriva in samma som honom i appsettings och det funkar 
            //eller om jag behöver hoppa över steget localdb
            
            //går till configuration-fil, letar efter defaultconnection- connectionsträngen
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //enablar färdig funktionalitet (scaffolding) som har med User-login att göra (Identity)
            //basic funktionality for using identity in app 
            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IPieRepository, PieRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>(); //kan nu använda riktiga konkreta implementationen ist för MockRepository
            services.AddScoped<IOrderRepository, OrderRepository>();

            //lägger till servicen shoppingcart som skickar med samma service provider till metoden GetCart
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));
            
            //ger tillgång till httpcontext som ger tillgång  till sessionen
            services.AddHttpContextAccessor();
            services.AddSession();
            services.AddControllersWithViews(); //support för MVC (-struktur?)
            services.AddRazorPages();   //Används av scaffolded identity
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
            app.UseStaticFiles();       //för kunna använda statiska objekt ex. bilder css/js-filer...
            app.UseSession();

            app.UseRouting();
            app.UseAuthentication();//vem är registrerad
            app.UseAuthorization(); //vad är tillängligt för vem


            app.UseEndpoints(endpoints =>
            {
                //patternet för sökvägen
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //Egen enpoint för razor pages (med identiity sidor)
                endpoints.MapRazorPages();
            });
        }
    }
}
