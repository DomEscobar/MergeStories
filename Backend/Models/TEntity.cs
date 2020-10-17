using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PublicTimeAPI.Models
{
  public class TEntity
  {
    public virtual int Id { get; set; }

    [IgnoreDataMember]
    [JsonIgnore]
    public virtual string ClientId { get; set; }
  }
}
