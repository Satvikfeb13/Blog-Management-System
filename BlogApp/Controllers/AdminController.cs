using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApp.Services;

namespace BlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPostService _postService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(AppDbContext context, IPostService postService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _postService = postService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var allPosts = await _context.Posts.ToListAsync();
            
            ViewBag.TotalPosts = allPosts.Count;
            ViewBag.PendingBlogs = allPosts.Count(p => p.Status == PostStatus.PendingReview);
            ViewBag.PublishedBlogs = allPosts.Count(p => p.Status == PostStatus.Published);
            ViewBag.RejectedBlogs = allPosts.Count(p => p.Status == PostStatus.Rejected);
            ViewBag.DraftBlogs = allPosts.Count(p => p.Status == PostStatus.Draft);
            
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalCategories = await _context.Categories.CountAsync();
            ViewBag.TotalComments = await _context.Comments.CountAsync();

            ViewBag.RecentPosts = await _context.Posts
                .Include(p => p.Category)
                .OrderByDescending(p => p.PublishedDate)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentComments = await _context.Comments
                .Include(c => c.Post)
                .OrderByDescending(c => c.CommentDate)
                .Take(5)
                .ToListAsync();

            return View();
        }
        
        public async Task<IActionResult> PendingPosts()
        {
            var pendingPosts = await _postService.GetPendingPostsAsync();
            return View(pendingPosts);
        }
        
        [HttpPost]
        public async Task<IActionResult> ApprovePost(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                await _postService.ApprovePostAsync(id, user.Id);
            }
            return RedirectToAction(nameof(PendingPosts));
        }
        
        [HttpPost]
        public async Task<IActionResult> RejectPost(int id, string reason)
        {
            await _postService.RejectPostAsync(id, reason);
            return RedirectToAction(nameof(PendingPosts));
        }
        
        public async Task<IActionResult> ReportedComments()
        {
            var reports = await _context.CommentReports
                .Include(cr => cr.Comment)
                    .ThenInclude(c => c.Post)
                .Include(cr => cr.ReportedByUser)
                .OrderByDescending(cr => cr.ReportDate)
                .ToListAsync();
            return View(reports);
        }
    }
}
