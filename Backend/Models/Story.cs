using System;
using System.Collections.Generic;

namespace PublicTimeAPI.Models
{
  public class Story : TEntity
  {
    public DateTime Date { get; set; }
    public string ImgCase { get; set; }
    public string ImgSolution { get; set; }
    public string Topics { get; set; }
  }
}