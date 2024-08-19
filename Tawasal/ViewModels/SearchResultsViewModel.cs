using Tawasal.Models;

namespace Tawasal.ViewModels
{
    public class SearchResultsViewModel
    {
        public ICollection<Profile> SearchResults { get; set; } = new List<Profile>();
    }
}
