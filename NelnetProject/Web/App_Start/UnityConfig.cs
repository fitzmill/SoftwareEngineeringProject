using Accessors;
using Core.Interfaces;
using Engines;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Configuration;
using Unity;
using Unity.Injection;
using Web.Managers;

namespace Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            //database accessors
            var connectionStringConstructor = new InjectionConstructor(ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString);
            container.RegisterType<IGetTransactionAccessor, GetTransactionAccessor>(connectionStringConstructor);
            container.RegisterType<IGetUserInfoAccessor, GetUserInfoAccessor>(connectionStringConstructor);
            container.RegisterType<ISetTransactionAccessor, SetTransactionAccessor>(connectionStringConstructor);
            container.RegisterType<ISetUserInfoAccessor, SetUserInfoAccessor>(connectionStringConstructor);
            container.RegisterType<IGetReportAccessor, GetReportAccessor>(connectionStringConstructor);
            container.RegisterType<ISetReportAccessor, SetReportAccessor>(connectionStringConstructor);

            //http client builder for payment spring accessors
            HttpClientBuilder httpClientBuilder = new HttpClientBuilder(
                ConfigurationManager.AppSettings["PaymentSpringPublicKey"],
                ConfigurationManager.AppSettings["PaymentSpringPrivateKey"]
            );
            container.RegisterInstance<HttpClientBuilder>(httpClientBuilder);

            //payment spring accessors
            var paymentSpringConstructor = new InjectionConstructor(httpClientBuilder, ConfigurationManager.AppSettings["PaymentSpringApiUrl"]);
            container.RegisterType<IGetPaymentInfoAccessor, GetPaymentInfoAccessor>(paymentSpringConstructor);
            container.RegisterType<ISetPaymentInfoAccessor, SetPaymentInfoAccessor>(paymentSpringConstructor);
            container.RegisterType<IChargePaymentAccessor, ChargePaymentAccessor>(paymentSpringConstructor);

            //email accessor
            container.RegisterType<IEmailAccessor, EmailAccessor>(new InjectionConstructor(
                ConfigurationManager.AppSettings["SenderEmail"],
                ConfigurationManager.AppSettings["SenderUsername"],
                ConfigurationManager.AppSettings["SenderPassword"],
                int.Parse(ConfigurationManager.AppSettings["Port"])
            ));

            //engines
            container.RegisterType<IGetTransactionEngine, GetTransactionEngine>();
            container.RegisterType<IGetReportEngine, GetReportEngine>();
            container.RegisterType<ISetReportEngine, SetReportEngine>();
            container.RegisterType<INotificationEngine, NotificationEngine>();
            container.RegisterType<IGetUserInfoEngine, GetUserInfoEngine>();
            container.RegisterType<IPaymentEngine, PaymentEngine>();
            container.RegisterType<ISetUserInfoEngine, SetUserInfoEngine>();

            //payment manager
            container.RegisterType<PaymentManager>(new InjectionConstructor(
                double.Parse(ConfigurationManager.AppSettings["TimerInterval"]),
                int.Parse(ConfigurationManager.AppSettings["ChargingHour"]),
                container.Resolve<IGetTransactionEngine>(),
                container.Resolve<IPaymentEngine>(),
                container.Resolve<INotificationEngine>()
            ));
            container.Resolve<PaymentManager>();
        }
    }
}