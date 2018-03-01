using Accessors;
using Core.Interfaces;
using Engines;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Configuration;
using Unity;
using Unity.Injection;

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
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            var constructor = new InjectionConstructor(ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString);

            /**
             * So since the accessor takes a string in the constructor instead of an interface, we need to pass that in manually.
             * The way we do that is through an InjectionConstructor, which takes the accessor's constructor's parameters as arguments.
             **/
            container.RegisterType<IGetTransactionAccessor, GetTransactionAccessor>(constructor);

            //This is how you register a type that takes an already registered interface as an argument
            container.RegisterType<IGetTransactionEngine, GetTransactionEngine>();

            container.RegisterType<ISetUserInfoAccessor, SetUserInfoAccessor>(constructor);

            container.RegisterType<IGetReportAccessor, GetReportAccessor>(constructor);

            container.RegisterType<IGetReportEngine, GetReportEngine>();

            container.RegisterType<ISetReportAccessor, SetReportAccessor>(constructor);

            container.RegisterType<ISetReportEngine, SetReportEngine>();

            container.RegisterType<INotificationEngine, NotificationEngine>();

            container.RegisterType<IEmailAccessor, EmailAccessor>(new InjectionConstructor(
                ConfigurationManager.AppSettings["SenderEmail"],
                ConfigurationManager.AppSettings["SenderUsername"],
                ConfigurationManager.AppSettings["SenderPassword"],
                int.Parse(ConfigurationManager.AppSettings["Port"])
            ));

        }
    }
}