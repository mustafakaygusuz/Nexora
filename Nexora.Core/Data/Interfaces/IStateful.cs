using Nexora.Core.Common.Enumerations;

namespace Nexora.Core.Data.Interfaces
{
    public interface IStateful
    {
        public StatusType Status { get; set; }
    }
}