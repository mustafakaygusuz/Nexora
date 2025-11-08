namespace Nexora.Core.Common.Models
{
    public sealed class UserActivationMailHashModel : BaseMailHashModel
    {
        public long UserId { get; set; }
    }
}