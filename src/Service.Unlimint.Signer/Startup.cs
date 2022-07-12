using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyJetWallet.ApiSecurityManager.Autofac;
using MyJetWallet.Sdk.GrpcSchema;
using MyJetWallet.Sdk.Service;
using Prometheus;
using Service.Unlimint.Signer.Grpc;
using Service.Unlimint.Signer.Modules;
using Service.Unlimint.Signer.Services;
using SimpleTrading.ServiceStatusReporterConnector;

namespace Service.Unlimint.Signer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.BindCodeFirstGrpc();

            services.AddHostedService<ApplicationLifetimeManager>();

            services.AddMyTelemetry("SP-", Program.Settings.ZipkinUrl);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMetricServer();

            app.BindServicesTree(Assembly.GetExecutingAssembly());

            app.BindIsAlive();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcSchema<CircleEncryptionKeyService, ICircleEncryptionKeyService>();
                endpoints.MapGrpcSchema<CircleCardsService, ICircleCardsService>();
                endpoints.MapGrpcSchema<CircleBankAccountsService, ICircleBankAccountsService>();
                endpoints.MapGrpcSchema<UnlimintPaymentsService, IUnlimintPaymentsService>();
                endpoints.MapGrpcSchema<CirclePayoutsService, ICirclePayoutsService>();
                endpoints.MapGrpcSchema<CircleDepositAddressService, ICircleDepositAddressService>();
                //endpoints.MapGrpcSchema<CircleTransfersService, ICircleTransfersService>();
                endpoints.MapGrpcSchema<CircleBusinessAccountService, ICircleBusinessAccountService>();
                endpoints.MapGrpcSchema<CircleChargebacksService, ICircleChargebacksService>();

                endpoints.MapGrpcSchemaRegistry();

                //security
                endpoints.RegisterGrpcServices();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<SettingsModule>();
            builder.RegisterModule<ServiceModule>();
        }
    }
}