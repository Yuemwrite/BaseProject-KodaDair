using System.Net;

namespace BaseProject.Application.Common.Exceptions;

public class UserFriendlyException : CustomException
{
    public Enum ExceptionTypeEnum { get; set; }

    public UserFriendlyException(Enum exceptionTypeEnum, List<string>? errors = default, HttpStatusCode httpStatusCode = HttpStatusCode.NoContent)
        : base("Failures Occured.", errors, httpStatusCode)
    {
        ExceptionTypeEnum = exceptionTypeEnum;
    }
}