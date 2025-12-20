namespace Nexora.Core.Common.Enumerations
{
    public enum StaticTextKeyType
    {
        IntrnlSrvrErr,
        IntrnlSrvrErrTtl,
        DbExInsrt,
        DbExUpdt,
        DbExDlt,
        MlHshExTknExprd,
        MlHshExWrngHsh,
        MdlVldtnExNtVld,
        AuthExEmlAlrdyExst,
        AuthExUsrNtFnd,
        AuthExInvldPsswrd,
        AuthExInvldRfshTkn,
        AuthExRfshTknExprd,
        CnsmrExNtFnd,

        // Validation Keys
        AuthExInvalidEmail,
        AuthExPwdReqUpper,
        AuthExPwdReqLower,
        AuthExPwdReqDigit,
        AuthExPwdReqSpecial,
        AuthExInvalidNickname,
        AuthExInvalidName,
        AuthExInvalidSurname,
        AuthExInvalidBirthDate,
    }
}