using System;
using System.Net.Sockets;
using System.Web.Mvc;
using System.Web.Routing;

namespace Teamworks.Web.Attributes.Mvc
{
    public class NoDbExceptionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ExceptionHandled && context.Exception != null)
            {
                Exception e = context.Exception;
                var socketException = e.InnerException as SocketException;
                if (socketException != null)
                {
                    switch (socketException.SocketErrorCode)
                    {
                        case SocketError.AddressNotAvailable:
                        case SocketError.NetworkDown:
                        case SocketError.NetworkUnreachable:
                        case SocketError.ConnectionAborted:
                        case SocketError.ConnectionReset:
                        case SocketError.TimedOut:
                        case SocketError.ConnectionRefused:
                        case SocketError.HostDown:
                        case SocketError.HostUnreachable:
                        case SocketError.HostNotFound:
                            context.Result = new ViewResult()
                                                 {
                                                     ViewData = new ViewDataDictionary(),
                                                     TempData = new TempDataDictionary(),
                                                     ViewName = "NoDb",
                                                     ViewEngineCollection = ViewEngines.Engines
                                                 };
                            context.ExceptionHandled = true;
                            break;
                    }
                }
            }

            base.OnActionExecuted(context);
        }
    }
}