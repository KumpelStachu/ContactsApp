using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class PaginatedList<T>(IEnumerable<T> items, int pageIndex, int totalPages)
    {
        [Required]
        public List<T> Items { get; } = items.ToList();
        [Required]
        public int PageIndex { get; } = pageIndex;
        [Required]
        public int TotalPages { get; } = totalPages;
        [Required]
        public bool HasPreviousPage => PageIndex > 1;
        [Required]
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedList<TNew> Select<TNew>(Func<T, TNew> map) => new PaginatedList<TNew>(Items.Select(map), PageIndex, TotalPages);
    }
}