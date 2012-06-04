using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Events
{
    public class SimpleBus : ICommandSender, IEventPublisher
    {
        private readonly Func<Type, object> factory;
        private readonly ConcurrentDictionary<Type, List<RouteInfo>> routes = new ConcurrentDictionary<Type, List<RouteInfo>>();

        public SimpleBus(Func<Type,object> factory)
        {
            this.factory = factory;
        }

        public void RegisterRoute<TMessage,THandler>() where TMessage: IMessage
                                                       where THandler : IHandle<TMessage> 
        {
            var messageType = typeof (TMessage);
            var handlerType = typeof(THandler);
            RegisterRoute(handlerType, messageType);
        }

        private void RegisterRoute(Type handlerType, Type messageType)
        {
            var handlerInterface = typeof (IHandle<>).MakeGenericType(messageType);
            var method = handlerInterface.GetMethod("Handle");
            var routeInfo = new RouteInfo(handlerType, method);
            routes.AddOrUpdate(
                messageType,
                new List<RouteInfo> {routeInfo},
                (t, v) => {
                    v.Add(routeInfo);
                   return v;
                }
            );
        }

        public void RegisterHandlers(params System.Reflection.Assembly[] assemblies)
        {
            var toRegister = from assembly in assemblies
                             from t in assembly.GetExportedTypes()
                             from i in t.GetInterfaces()
                             where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandle<>)
                             select new { HandlerType = t, MessageType = i.GetGenericArguments().First() };

            foreach (var duo in toRegister)
            {
                RegisterRoute(duo.HandlerType, duo.MessageType);
            }
        }

        public void Send(ICommand command)
        {
            var type = command.GetType();
            List<RouteInfo> routesForMessage;
            if (!routes.TryGetValue(type, out routesForMessage)) return;
            if (routesForMessage.Count > 1) throw new InvalidOperationException("Too many handlers for this command");
            var routeInfo = routesForMessage.Single();
            routeInfo.Method.Invoke(factory(routeInfo.HandlerType), new object[]{command});
        }

        public void Publish(IEvent @event)
        {
            var type = @event.GetType();
            List<RouteInfo> routesForMessage;
            if (!routes.TryGetValue(type, out routesForMessage)) return;
            
            foreach (var routeInfo in routesForMessage)
            {
                routeInfo.Method.Invoke(factory(routeInfo.HandlerType), new object[] {@event});
            }
        }

        public int NumberOfRoutes(Type type)
        {
            return !routes.ContainsKey(type) ? 0 : routes[type].Count;
        }

        public int NumberOfRoutes<T>()
        {
            return NumberOfRoutes(typeof(T));
        }
    }
}
