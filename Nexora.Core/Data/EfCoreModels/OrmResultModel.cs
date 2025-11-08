using Nexora.Core.Common.Extensions;

namespace Nexora.Core.Data.EfCoreModels
{
    public sealed class OrmResultModel<T>
    {
        public T? Data { get; set; }
        public Exception? Error { get; set; }
        public bool IsSuccess => !Error.HasValue();
    }
}