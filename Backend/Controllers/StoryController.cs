using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NinjaNye.SearchExtensions;
using PublicTimeAPI.Models;
using PublicTimeAPI.Repository;

namespace PublicTimeAPI.Controllers
{
  public class StoryController : ControllerBase<ApplicationDbContext, StoryController>
  {
    private IRequestService requestService;
    private IActionContextAccessor actionContextAccessor;
    private IDatetimeprovider datetimeprovider;

    public StoryController(ApplicationDbContext context, ILogger<StoryController> logger, IDatetimeprovider datetimeprovider, IRequestService requestService, IActionContextAccessor actionContextAccessor)
      : base(context, logger)
    {
      this.requestService = requestService;
      this.datetimeprovider = datetimeprovider;
    }

    [HttpPut]
    public async Task<ActionResult<Story>> CreateStory([FromBody] Story story)
    {
      story.ClientId = Request.GetClientId();
      Context.Stories.Add(story);
      await Context.SaveChangesAsync();

      return Ok(story);
    }

    [AllowAnonymous]
    [HttpGet("search/{searchValue}")]
    public async Task<ActionResult<IEnumerable<Story>>> GetStories(string searchValue)
    {
      return Ok(await Context.Stories.OrderByDescending(o => o.Date).ToListAsync());
    }
  }
}
