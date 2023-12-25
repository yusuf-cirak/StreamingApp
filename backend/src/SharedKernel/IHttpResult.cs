namespace SharedKernel;

public interface IHttpResult
{
    IHttpResult CreateWith(Error error, int statusCode);
}