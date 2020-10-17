using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PublicTimeAPI.Controllers
{
  [Route("api/[controller]")]
  public class ControllerBase<TContext, TController> : Controller
    where TContext : DbContext
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="ControllerBase{T, C}"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    public ControllerBase(TContext context, ILogger<TController> logger)
    {
      Context = context;
      Logger = logger;
    }

    /// <summary>
    /// Context to access the database.
    /// </summary>
    protected TContext Context { get; private set; }

    /// <summary>
    /// Logger used to log events
    /// </summary>
    protected ILogger<TController> Logger { get; private set; }
  }

}
