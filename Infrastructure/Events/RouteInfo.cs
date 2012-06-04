using System;
using System.Reflection;

namespace Infrastructure.Events
{
    public class RouteInfo
    {
        private readonly Type handlerType;
        private readonly MethodInfo method;

        public Type HandlerType
        {
            get { return handlerType; }
        }

        public MethodInfo Method
        {
            get { return method; }
        }

        public RouteInfo(Type handlerType, MethodInfo method)
        {
            this.handlerType = handlerType;
            this.method = method;
        }
    }
}