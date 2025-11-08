namespace Nexora.Core.Common.Models
{
    public sealed class PasswordResetMailHashModel : BaseMailHashModel
    {
        public long UserId { get; set; }
    }
}