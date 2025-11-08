namespace Nexora.Core.Common.Models
{
    public abstract class SearchRequestBaseModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string OrderColumn { get; set; } = "Id";
        public string OrderType { get; set; } = "ASC";
    }
}