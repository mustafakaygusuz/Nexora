using Nexora.Core.Common.Enumerations;

namespace Nexora.Core.Common.Helpers
{
    public static class StaticTextKeyHelper
    {
        public static string? Get(StaticTextKeyType type)
        {
            return StaticTextKeys.GetValueOrDefault(type);
        }

        public static Dictionary<StaticTextKeyType, string> List(List<StaticTextKeyType> types)
        {
            return StaticTextKeys.Where(x => types.Contains(x.Key)).ToDictionary();
        }

        private static readonly Dictionary<StaticTextKeyType, string> StaticTextKeys = new()
        {
            {StaticTextKeyType.IntrnlSrvrErr, "SystemText_Error_InternalServer"},
            {StaticTextKeyType.IntrnlSrvrErrTtl, "SystemText_Error_InternalServerTitle"},
            {StaticTextKeyType.DbExInsrt, "Database_Exception_Insert"},
            {StaticTextKeyType.DbExUpdt, "Database_Exception_Update"},
            {StaticTextKeyType.DbExDlt, "Database_Exception_Delete"},
            {StaticTextKeyType.MlHshExTknExprd, "MailHash_Exception_TokenExpired"},
            {StaticTextKeyType.MlHshExWrngHsh, "MailHash_Exception_WrongHash"},
            {StaticTextKeyType.AuthExEmlAlrdyExst, "Auth_Exception_EmailAlreadyExist"},
            {StaticTextKeyType.AuthExUsrNtFnd, "Auth_Exception_UserNotFound"},
            {StaticTextKeyType.AuthExInvldPsswrd, "Auth_Exception_InvalidPassword"},
            {StaticTextKeyType.AuthExInvldRfshTkn, "Auth_Exception_InvalidRefreshToken"},
            {StaticTextKeyType.AuthExRfshTknExprd, "Auth_Exception_InvalidRefreshTokenExpired"},

            // Validation
            {StaticTextKeyType.AuthExInvalidEmail, "Auth_Validation_InvalidEmail"},
            {StaticTextKeyType.AuthExPwdReqUpper, "Auth_Validation_PasswordUppercaseRequired"},
            {StaticTextKeyType.AuthExPwdReqLower, "Auth_Validation_PasswordLowercaseRequired"},
            {StaticTextKeyType.AuthExPwdReqDigit, "Auth_Validation_PasswordDigitRequired"},
            {StaticTextKeyType.AuthExPwdReqSpecial, "Auth_Validation_PasswordSpecialRequired"},
            {StaticTextKeyType.AuthExInvalidNickname, "Auth_Validation_InvalidNickname"},
            {StaticTextKeyType.AuthExInvalidName, "Auth_Validation_InvalidName"},
            {StaticTextKeyType.AuthExInvalidSurname, "Auth_Validation_InvalidSurname"},
            {StaticTextKeyType.AuthExInvalidBirthDate, "Auth_Validation_InvalidBirthDate"},
        };
    }
}