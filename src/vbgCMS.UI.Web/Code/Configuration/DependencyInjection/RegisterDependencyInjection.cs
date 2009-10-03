﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using StructureMap;
using StructureMap.Configuration.DSL;
using System.Web.Routing;
using vbgCMS.UI.Web.Code.Configuration.Mvc;
using System.Web.Mvc;

namespace vbgCMS.UI.Web.Code.Configuration.DependencyInjection
{
    public class RegisterDependencyInjection : IRegister
    {
        private class MvcRegistry : Registry
        {
            public MvcRegistry()
            {
                var x = this;

                x.ForRequestedType<RouteCollection>().TheDefault.IsThis(RouteTable.Routes);
                x.ForRequestedType<ControllerBuilder>().TheDefault.IsThis(ControllerBuilder.Current);

                x.ForRequestedType<IRegister>()
                    .AddConcreteType<RegisterRoutes>()
                    .AddConcreteType<RegisterControllerFactory>();
            }
        }

        public class StructureMapServiceLocator : ServiceLocatorImplBase
        {

            protected override object DoGetInstance(Type serviceType, string key)
            {
                return string.IsNullOrEmpty(key)
                           ? ObjectFactory.GetInstance(serviceType)
                           : ObjectFactory.GetNamedInstance(serviceType, key);
            }

            protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
            {
                return ObjectFactory.GetAllInstances(serviceType).Cast<object>().AsEnumerable();
            }
        }

        #region IRegister Members

        public void Execute()
        {
            ObjectFactory.Initialize(x => {
                x.AddRegistry<MvcRegistry>();
            });

            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator());
        }

        #endregion
    }
}