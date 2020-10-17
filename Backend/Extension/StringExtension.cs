using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PublicTimeAPI.Models;
using PublicTimeAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.Extensions.StringExtension
{
  public static class StringExtension
  {
    public static void toLong(this string str, int maxlength)
    {
      if (str.Length > maxlength)
      {
        throw new System.Exception("Text to long");
      }
    }

    public static byte[] ToPassword(this string str)
    {
      ASCIIEncoding enc = new ASCIIEncoding();
      string passHashed = string.Empty;

      if (str == null || str.Equals(string.Empty))
      {
        return enc.GetBytes(string.Empty);
      }

      passHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: Encoding.Unicode.GetBytes("SaltySal4t895734895!"),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

      return enc.GetBytes(passHashed);
    }
  }
}
