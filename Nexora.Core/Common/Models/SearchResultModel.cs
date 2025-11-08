namespace Nexora.Core.Common.Models
{
    public sealed class SearchResultModel<T> where T : new()
    {
        public SearchResultModel()
        {
            Result = new List<T>();
        }

        public long TotalCount { get; set; }
        public List<T> Result { get; set; }
    }
}