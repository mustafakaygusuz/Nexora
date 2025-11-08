using System.Text.RegularExpressions;

namespace Nexora.Core.Common.Helpers
{
    public static class PasswordValidationHelper
    {
        public static bool IsPasswordValid(string password)
        {
            return !string.IsNullOrEmpty(password) &&
                       password.Length >= 8 &&
                       Regex.IsMatch(password, @"[A-Z]") &&
                       Regex.IsMatch(password, @"[a-z]") &&
                       Regex.IsMatch(password, @"\d") &&
                       Regex.IsMatch(password, @"[^\w\s]");
        }
    }
}