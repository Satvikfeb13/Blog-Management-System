using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Services
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync(int? categoryId, string? searchQuery, bool includeUnpublished = false)
        {
            var query = _context.Posts
                .Include(p => p.Category)
                .AsNoTracking();

            if (!includeUnpublished)
            {
                query = query.Where(p => p.IsPublished && p.Status == PostStatus.Published);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                query = query.Where(p => p.Title.ToLower().Contains(searchQuery) || 
                                         p.Content.ToLower().Contains(searchQuery) ||
                                         (p.Category != null && p.Category.Name.ToLower().Contains(searchQuery)));
            }

            return await query.OrderByDescending(p => p.PublishedDate).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(string userId)
        {
            return await _context.Posts
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.CreatedByUserId == userId)
                .OrderByDescending(p => p.PublishedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPendingPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.CreatedByUser)
                .AsNoTracking()
                .Where(p => p.Status == PostStatus.PendingReview)
                .OrderByDescending(p => p.PublishedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPostsForAdminAsync()
        {
            return await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.CreatedByUser)
                .AsNoTracking()
                .OrderByDescending(p => p.PublishedDate)
                .ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreatePostAsync(Post post)
        {
            post.PublishedDate = DateTime.Now;
            
            // Calculate read time based on word count
            var wordCount = post.Content.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            post.ReadTime = (int)Math.Ceiling(wordCount / 200.0); // Assuming 200 words per minute
            
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            var wordCount = post.Content.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            post.ReadTime = (int)Math.Ceiling(wordCount / 200.0);
            
            post.LastModified = DateTime.Now;
            
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task ApprovePostAsync(int id, string adminId)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                post.Status = PostStatus.Published;
                post.IsPublished = true;
                post.ApprovedByAdminId = adminId;
                post.ApprovedDate = DateTime.Now;
                post.PublishedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task RejectPostAsync(int id, string reason)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                post.Status = PostStatus.Rejected;
                post.IsPublished = false;
                post.RejectedReason = reason;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }
        
        public async Task IncrementViewCountAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                post.ViewCount++;
                await _context.SaveChangesAsync();
            }
        }
    }
}
