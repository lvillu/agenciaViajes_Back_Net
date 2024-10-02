

namespace Aplication.Helpers
{
  public static class PasswordHelper
  {
    public static string EncryptPassword(string password) =>
      BCrypt.Net.BCrypt.HashPassword(password);

    public static bool VerifyPassword(string password, string hashedPassword) =>
        BCrypt.Net.BCrypt.Verify(password, hashedPassword);

  }
}
