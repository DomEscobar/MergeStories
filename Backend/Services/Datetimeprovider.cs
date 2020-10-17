using System;

namespace API.Services
{
  public interface IDatetimeprovider
  {
    DateTime Now { get; }
  }

  public class Datetimeprovider : IDatetimeprovider
  {
    public DateTime Now => DateTime.Now;
  }
}
