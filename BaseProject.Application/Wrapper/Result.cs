namespace BaseProject.Application.Wrapper;

public class Result : IResult
{
    public List<string>? Messages { get; set; }
    public ErrorInfo? ErrorInfo { get; set; }
    public bool Succeeded { get; set; }
    
    public static IResult Fail()
    {
        return new Result { Succeeded = false };
    }
    public static IResult Fail(ErrorInfo errorInfo)
    {
        return new Result { Succeeded = false, ErrorInfo = errorInfo};
    }

    public static IResult Fail(string message)
    {
        return new Result { Succeeded = false, Messages = new List<string> { message } };
    }

    public static IResult Fail(List<string> messages)
    {
        return new Result { Succeeded = false, Messages = messages };
    }

    public static Task<IResult> FailAsync()
    {
        return Task.FromResult(Fail());
    }
    public static  Task<IResult> FailAsync(ErrorInfo errorInfo)
    {
        return  Task.FromResult(Fail(errorInfo));
    }

    public static Task<IResult> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public static Task<IResult> FailAsync(List<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }
	
    public static IResult Success()
    {
        return new Result { Succeeded = true };
    }

    public static IResult Success(string message)
    {
        return new Result { Succeeded = true, Messages = new List<string> { message } };
    }

    public static IResult Success(List<string> messages)
    {
        return new Result { Succeeded = true, Messages = messages };
    }

    public static Task<IResult> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<IResult> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }

    public static Task<IResult> SuccessAsync(List<string> messages)
    {
        return Task.FromResult(Success(messages));
    }
}

public class ErrorInfo
{
    public string? ErrorId { get; set; }
    public List<string>? Message { get; set; }
    public string? Code { get; set; }
}

public class Result<T> : Result, IResult<T>
{
	public T? Data { get; set; }

	public static new Result<T> Fail()
	{
		return new() { Succeeded = false };
	}
	public static new Result<T> Fail(ErrorInfo errorInfo)
	{
		return new() { Succeeded = false, ErrorInfo = errorInfo};
	}

	public static new Result<T> Fail(string message)
	{
		return new() { Succeeded = false, Messages = new List<string> { message } };
	}

	public static new Result<T> Fail(List<string> messages)
	{
		return new() { Succeeded = false, Messages = messages };
	}

	public static new Task<Result<T>> FailAsync()
	{
		return Task.FromResult(Fail());
	}

	public static new Task<Result<T>> FailAsync(string message)
	{
		return Task.FromResult(Fail(message));
	}

	public static new Task<Result<T>> FailAsync(ErrorInfo errorInfo)
	{
		return Task.FromResult(Fail(errorInfo));
	}
	

	public static new Task<Result<T>> FailAsync(List<string> messages)
	{
		return Task.FromResult(Fail(messages));
	}

	public static new Result<T> Success()
	{
		return new() { Succeeded = true };
	}

	public static new Result<T> Success(string message)
	{
		return new() { Succeeded = true, Messages = new List<string> { message } };
	}

	public static new Result<T> Success(List<string> messages)
	{
		return new() { Succeeded = true, Messages = messages };
	}

	public static Result<T> Success(T data)
	{
		return new() { Succeeded = true, Data = data };
	}

	public static Result<T> Success(T data, string message)
	{
		return new() { Succeeded = true, Data = data, Messages = new List<string> { message } };
	}

	public static Result<T> Success(T data, List<string> messages)
	{
		return new() { Succeeded = true, Data = data, Messages = messages };
	}

	public static new Task<Result<T>> SuccessAsync()
	{
		return Task.FromResult(Success());
	}

	public static new Task<Result<T>> SuccessAsync(string message)
	{
		return Task.FromResult(Success(message));
	}

	public static new Task<Result<T>> SuccessAsync(List<string> messages)
	{
		return Task.FromResult(Success(messages));
	}

	public static Task<Result<T>> SuccessAsync(T data)
	{
		return Task.FromResult(Success(data));
	}

	public static Task<Result<T>> SuccessAsync(T data, string message)
	{
		return Task.FromResult(Success(data, message));
	}

	public static Task<Result<T>> SuccessAsync(T data, List<string> messages)
	{
		return Task.FromResult(Success(data, messages));
	}
}