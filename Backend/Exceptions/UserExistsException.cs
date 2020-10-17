namespace API.Exceptions
{
  public class UserExistsException : ExceptionBase
  {
    public override ErrorCode Code => ErrorCode.UserExists;
  }
}
