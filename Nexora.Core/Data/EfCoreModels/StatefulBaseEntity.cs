using Nexora.Core.Common.Enumerations;
using Nexora.Core.Data.Interfaces;

namespace Nexora.Core.Data.EfCoreModels
{
    public abstract class StatefulBaseEntity : BaseEntity, IStateful
    {
        public StatusType Status { get; set; }
    }
}