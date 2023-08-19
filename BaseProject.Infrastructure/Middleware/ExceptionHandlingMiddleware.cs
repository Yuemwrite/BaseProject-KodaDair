using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using BaseProject.Application.Common.Exceptions;
using Domain.Base.Auditing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Infrastructure.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (System.Exception exception)
        {
            string errorId = Guid.NewGuid().ToString();
            ErrorResult errorResult = new ErrorResult()
            {
                Exception = exception.Message.Trim(),
                ErrorId = errorId,
            };
            
            errorResult.Messages!.Add(exception.Message);
            var response = context.Response;
            response.ContentType = "application/json";
            
            

            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }
            
            switch (exception)
            {
                case UserFriendlyException e:
                    errorResult.ErrorCode = Convert.ToInt32(e.ExceptionTypeEnum);
                    errorResult.ErrorMessage = e.ExceptionTypeEnum.GetAttribute<DisplayAttribute>()?.Name;
                    errorResult.StatusCode = response.StatusCode = e.ExceptionTypeEnum.GetAttribute<DisplayAttribute>().Order;
                    errorResult.Messages = e.ErrorMessages;
                    break;
                case CustomException e:
                    response.StatusCode = errorResult.StatusCode = (int) e.StatusCode;
                    if (e.ErrorMessages is not null)
                    {
                        errorResult.Messages = e.ErrorMessages;
                    }

                    break;

                default:
                    response.StatusCode = errorResult.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }
            
            string json = JsonSerializer.Serialize(errorResult);
            await response.WriteAsync(json);
        }
    }
}