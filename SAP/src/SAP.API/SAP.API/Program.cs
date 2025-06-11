
using SAP.Application;
using SAP.Application.Model.User;
using SAP.Application.Service.lmpl;
using SAP.Application.Service;

namespace SAP.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplication(builder.Environment);
            builder.Services.AddSingleton<ISAPSessionStorage, SAPSessionStorage>();



            builder.Services.AddHttpClient();

            builder.Services.AddControllers();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
