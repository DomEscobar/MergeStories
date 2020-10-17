namespace API.Exceptions
{
  public abstract class ExceptionBase
  {
    public abstract ErrorCode Code { get; }

    public string Message { get; }
  }
}
