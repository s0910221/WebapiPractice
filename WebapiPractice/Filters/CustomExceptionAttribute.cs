using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using WebapiPractice.Models;

namespace WebapiPractice.Filters
{
    public class CustomExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new ObjectContent<CustomHttpError>(new CustomHttpError()
                {
                    Error_Code = 1,
                    Error_Message = actionExecutedContext.Exception.Message

                },
                GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
        }


    }
}