using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace UrlShortener.Infrastructure
{
    public class ExceptionLogger : IExceptionLogger
    {
        public async Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            Services.GetLogger(this).Error(context.Exception);
        }
    }
}