using Autofac;
using Autofac.Extras.DynamicProxy;
using BusinessLayer.Abstrack;
using BusinessLayer.Concreate.WorkFlow;
using CoreLayer.Utilities.Aspect;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concreate;

namespace BusinessLayer.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<LogonWorkFlow>().As<ILogonWorkFlow>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerWorkFlow>().As<ICustomerWorkFlow>().InstancePerLifetimeScope();
            builder.RegisterType<AdressWorkFlow>().As<IAdressWorkFlow>().InstancePerLifetimeScope();
            builder.RegisterType<BranchWorkFlow>().As<IBranchWorkFlow>().InstancePerLifetimeScope();
            builder.RegisterType<MailWorkFlow>().As<IMailWorkFlow>().InstancePerLifetimeScope();
            builder.RegisterType<PhoneNumberWorkFlow>().As<IPhoneNumberWorkFlow>().InstancePerLifetimeScope();
            builder.RegisterType<CardWorkFlow>().As<ICardWorkFlow>().InstancePerLifetimeScope();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new Castle.DynamicProxy.ProxyGenerationOptions
                {
                    Selector = new AspectInterceptorSelector()
                }).InstancePerLifetimeScope();
        }
    }
}
