using BlogApp.Models;

namespace BlogApp.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostsAsync(int? categoryId, string? searchQuery, bool includeUnpublished = false);
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(string userId);
        Task<IEnumerable<Post>> GetPendingPostsAsync();
        Task<IEnumerable<Post>> GetAllPostsForAdminAsync();
        Task<Post?> GetPostByIdAsync(int id);
        Task CreatePostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);
        Task ApprovePostAsync(int id, string adminId);
        Task RejectPostAsync(int id, string reason);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task IncrementViewCountAsync(int id);
    }
}
