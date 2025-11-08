using Nexora.Core.Common.Attributes;

namespace Nexora.Core.Common.Models
{
    public sealed class DropdownDataWithEncryptIdModel
    {
        public string Key { get; set; }
        [EncryptedId]
        public long Value { get; set; }
    }
}