[assembly: WebActivator.PreApplicationStartMethod(typeof(scrilla.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(scrilla.Web.App_Start.NinjectWebCommon), "Stop")]

namespace scrilla.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
	using scrilla.Data.EF;
	using scrilla.Data;
	using scrilla.Data.Repositories;
	using scrilla.Services;
	using scrilla.Data.EF.Repositories;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
			kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>().InRequestScope();
			kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
			kernel.Bind<IAccountRepository>().To<AccountRepository>();
			kernel.Bind<IAccountGroupRepository>().To<AccountGroupRepository>();
			kernel.Bind<ITransactionRepository>().To<TransactionRepository>();
			kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
			kernel.Bind<ICategoryGroupRepository>().To<CategoryGroupRepository>();
			kernel.Bind<IBudgetCategoryRepository>().To<BudgetCategoryRepository>();
			kernel.Bind<IVendorRepository>().To<VendorRepository>();
			kernel.Bind<IBillRepository>().To<BillRepository>();
			kernel.Bind<IBillGroupRepository>().To<BillGroupRepository>();
			kernel.Bind<IBillTransactionRepository>().To<BillTransactionRepository>();
			kernel.Bind<IImportDescriptionVendorMapRepository>().To<ImportDescriptionVendorMapRepository>();
			kernel.Bind<IAccountService>().To<AccountService>();
			kernel.Bind<ITransactionImporter>().To<TransactionImporter>();
        }        
    }
}
