
export class Helper
{
  public static createId(): string
  {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
  }

  public static sameDay(d1, d2)
  {
    if (d1 == null || d2 == null)
    {
      return false;
    }

    d1 = new Date(d1);
    d2 = new Date(d2);

    return d1.getFullYear() === d2.getFullYear() &&
      d1.getMonth() === d2.getMonth() &&
      d1.getDate() === d2.getDate();
  }

 
}