using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class ExceptionFilterAttribute :
        Attribute,
        IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch(context.Exception)
            {
                case ConflictException:
                    context.Result = new ConflictObjectResult(context.Exception.Message);
                    break;

                case NotFoundException:
                    context.Result = new NotFoundObjectResult(context.Exception.Message);
                    break;

                case ForbiddenException:
                    context.Result = new ForbidResult("BasicAuthentication");
                    break;

                case InvalidOperationException:
                    context.Result = new BadRequestObjectResult(context.Exception.Message);
                    break;

                default:
                    context.ExceptionHandled = false;
                    return;
            }

            context.ExceptionHandled = true;
        }
    }
}
