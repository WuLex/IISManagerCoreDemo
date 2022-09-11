using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IISManagerCore.Common
{
    public static class MyHttpContext
    {
        public static IServiceProvider serviceProvider;
        static MyHttpContext()
        {
        }


        public static HttpContext Current
        {
            get
            {    
                //var factory2 = serviceProvider.GetService<IHttpContextAccessor>();
                var factory = serviceProvider.GetService(typeof(IHttpContextAccessor));

                HttpContext context = ((IHttpContextAccessor)factory).HttpContext;
                return context;
            }
        }
    }
}
