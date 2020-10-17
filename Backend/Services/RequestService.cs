using Microsoft.AspNetCore.Http;

namespace API.Services
{
  public interface IRequestService
  {
  }

  public class RequestService : IRequestService
  {
    private IHttpContextAccessor httpContextAccessor;

    public RequestService(IHttpContextAccessor httpContextAccessor)
    {
      this.httpContextAccessor = httpContextAccessor;
    }
  }
}
