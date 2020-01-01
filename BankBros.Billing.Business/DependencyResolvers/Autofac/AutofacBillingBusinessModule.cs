using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Extras.DynamicProxy;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Concrete;
using BankBros.Backend.Business.DependencyResolvers.Autofac;
using BankBros.Backend.Core.Utilities.Interceptors;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.DataAccess.Concrete.EntityFramework;
using BankBros.Billing.Business.Abstract;
using BankBros.Billing.Business.Concrete;
using Castle.DynamicProxy;

namespace BankBros.Billing.Business.DependencyResolvers.Autofac
{
    public class AutofacBillingBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BillingManager>().As<IBillingService>();
            builder.RegisterType<EfBillDal>().As<IBillDal>();
            builder.RegisterType<EfBillOrganizationDal>().As<IBillOrganizationDal>();
            builder.RegisterType<TransactionManager>().As<ITransactionService>();
            builder.RegisterType<CustomerManager>().As<ICustomerService>();
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<AccountManager>().As<IAccountService>();
            builder.RegisterType<SupportManager>().As<ISupportService>();

            builder.RegisterType<EfAccountDal>().As<IAccountDal>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();
            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>();
            builder.RegisterType<EfCustomerDetailDal>().As<ICustomerDetailDal>();
            builder.RegisterType<EfCustomerDal>().As<ICustomerDal>();
            builder.RegisterType<EfTransactionDal>().As<ITransactionDal>();
            builder.RegisterType<EfTransactionTypeDal>().As<ITransactionTypeDal>();
            builder.RegisterType<EfTransactionResultDal>().As<ITransactionResultDal>();
            builder.RegisterType<EfBalanceTypeDal>().As<IBalanceTypeDal>();
            
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
