using System.Text.RegularExpressions;

namespace Nexora.Core.Constants
{
    public static class RegexConstants
    {
        public static readonly Regex PhoneRegex = new(@"^\+?[1-9]\d{9,14}$", RegexOptions.Compiled);
        public static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        public static readonly Regex UniqueDocumentKeyRegex = new(@"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}", RegexOptions.Compiled);
    }
}