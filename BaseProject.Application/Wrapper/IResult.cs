namespace BaseProject.Application.Wrapper;

public interface IResult
{
    List<string>? Messages { get; set; }
    ErrorInfo? ErrorInfo { get; set; }

    bool Succeeded { get; set; }
}

public interface IResult<out T> : IResult
{
    T? Data { get; }
}