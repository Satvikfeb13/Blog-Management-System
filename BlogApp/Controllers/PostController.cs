using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Models.Enums;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BlogApp.Services.IPostService _postService;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly string[] allowedExtention = { ".jpg", ".jpeg", ".png" };

        public PostController(AppDbContext context, IWebHostEnvironment webHostEnvironment, BlogApp.Services.IPostService postService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _postService = postService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, string? search)
        {
            // Only return published posts for the public Index
            var posts = await _postService.GetAllPostsAsync(categoryId, search);
            ViewBag.Categories = await _postService.GetCategoriesAsync();
            return View(posts);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyBlogs()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
            var posts = await _postService.GetPostsByUserIdAsync(user.Id);
            return View(posts);
        }

        [HttpGet]
        [Authorize] // Allow both Admin and User to create
        public IActionResult Create()
        {
            var postviewmodel = new PostViewModel();
            postviewmodel.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(postviewmodel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostViewModel postViewModel, string submitAction)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (ModelState.IsValid)
            {
                var inptfileExtention = Path.GetExtension(postViewModel.FeatureImage?.FileName ?? "").ToLower();
                bool isAllowed = postViewModel.FeatureImage != null && allowedExtention.Contains(inptfileExtention);
                if (!isAllowed)
                {
                    ModelState.AddModelError("", "Invalid image format, allowed format are  .jpg, .jpeg, .png");
                    postViewModel.Categories = _context.Categories.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();
                    return View(postViewModel);
                }

                postViewModel.Post.FeatureImagePath = await UploadFileFolder(postViewModel.FeatureImage);
                postViewModel.Post.CreatedByUserId = user.Id;
                postViewModel.Post.Author = user.UserName;

                if (User.IsInRole("Admin"))
                {
                    // Admin posts are published immediately
                    postViewModel.Post.Status = PostStatus.Published;
                    postViewModel.Post.IsPublished = true;
                    postViewModel.Post.ApprovedByAdminId = user.Id;
                    postViewModel.Post.ApprovedDate = DateTime.Now;
                }
                else
                {
                    if (submitAction == "Submit for Review")
                    {
                        postViewModel.Post.Status = PostStatus.PendingReview;
                        postViewModel.Post.IsPublished = false;
                    }
                    else // Save as Draft
                    {
                        postViewModel.Post.Status = PostStatus.Draft;
                        postViewModel.Post.IsPublished = false;
                    }
                }

                await _postService.CreatePostAsync(postViewModel.Post);
                return RedirectToAction(User.IsInRole("Admin") ? "Index" : "MyBlogs");
            }
            
            postViewModel.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(postViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            
            // Allow if post is published, or if current user is the creator, or if current user is admin
            bool isCreator = user != null && post.CreatedByUserId == user.Id;
            bool isAdmin = User.IsInRole("Admin");

            if (!post.IsPublished && !isCreator && !isAdmin)
            {
                return NotFound(); // Prevent unauthorized viewing of drafts/pending/rejected
            }
            
            // Increment view count
            await _postService.IncrementViewCountAsync(id);

            return View(post);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var postfromdb = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (postfromdb == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            bool isCreator = user != null && postfromdb.CreatedByUserId == user.Id;
            bool isAdmin = User.IsInRole("Admin");

            if (!isCreator && !isAdmin)
            {
                return Forbid();
            }

            EditViewModel editViewModel = new EditViewModel
            {
                Post = postfromdb,
                Categories = _context.Categories.Select(c =>
                 new SelectListItem
                 {
                     Value = c.Id.ToString(),
                     Text = c.Name,
                 }
                ).ToList()
            };
            return View(editViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditViewModel editViewModel, string submitAction)
        {
            var postfromdb = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == editViewModel.Post.Id);
            if(postfromdb == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            bool isCreator = user != null && postfromdb.CreatedByUserId == user.Id;
            bool isAdmin = User.IsInRole("Admin");

            if (!isCreator && !isAdmin)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                editViewModel.Categories = _context.Categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                return View(editViewModel);
            }

            if(editViewModel.FeatureImage != null)
            {
                var inptfileExtention = Path.GetExtension(editViewModel.FeatureImage.FileName).ToLower();
                bool isAllowed = allowedExtention.Contains(inptfileExtention);
                if (!isAllowed)
                {
                    ModelState.AddModelError("", "Invalid image format,allowed format are .jpg,.jpeg.png");
                    editViewModel.Categories = _context.Categories.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();
                    return View(editViewModel);
                }
                if (!string.IsNullOrEmpty(postfromdb.FeatureImagePath))
                {
                    var existingFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images",
                        Path.GetFileName(postfromdb.FeatureImagePath));
                    if (System.IO.File.Exists(existingFilePath))
                    {
                       System.IO.File.Delete(existingFilePath);
                    }
                }

                editViewModel.Post.FeatureImagePath = await UploadFileFolder(editViewModel.FeatureImage);
            }
            else
            {
                editViewModel.Post.FeatureImagePath = postfromdb.FeatureImagePath; 
            }

            // Preserve author and ownership
            editViewModel.Post.CreatedByUserId = postfromdb.CreatedByUserId;
            editViewModel.Post.Author = postfromdb.Author;
            
            if (isAdmin)
            {
                // Admin edits preserve status or update if admin decides, but here we just keep it
                editViewModel.Post.Status = postfromdb.Status;
                editViewModel.Post.IsPublished = postfromdb.IsPublished;
                editViewModel.Post.ApprovedByAdminId = postfromdb.ApprovedByAdminId;
                editViewModel.Post.ApprovedDate = postfromdb.ApprovedDate;
                editViewModel.Post.RejectedReason = postfromdb.RejectedReason;
            }
            else
            {
                // If regular user edits a post, it goes back to draft or pending depending on action
                if (submitAction == "Submit for Review")
                {
                    editViewModel.Post.Status = PostStatus.PendingReview;
                    editViewModel.Post.IsPublished = false;
                }
                else
                {
                    editViewModel.Post.Status = PostStatus.Draft;
                    editViewModel.Post.IsPublished = false;
                }
            }

            await _postService.UpdatePostAsync(editViewModel.Post);
            return RedirectToAction(isAdmin ? "Index" : "MyBlogs");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var postFromDb = await _context.Posts.FindAsync(id);
            if (postFromDb == null)
            {
                return NotFound();
            }
            
            var user = await _userManager.GetUserAsync(User);
            bool isCreator = user != null && postFromDb.CreatedByUserId == user.Id;
            bool isAdmin = User.IsInRole("Admin");

            if (!isCreator && !isAdmin)
            {
                return Forbid();
            }
            
            return View(postFromDb);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            if (id < 0) return BadRequest();

            var postFromDb = await _context.Posts.FindAsync(id);
            if (postFromDb == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            bool isCreator = user != null && postFromDb.CreatedByUserId == user.Id;
            bool isAdmin = User.IsInRole("Admin");

            if (!isCreator && !isAdmin)
            {
                return Forbid();
            }

            if (!string.IsNullOrEmpty(postFromDb.FeatureImagePath))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", Path.GetFileName(postFromDb.FeatureImagePath));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            
            await _postService.DeletePostAsync(id);
            return RedirectToAction(isAdmin ? "Index" : "MyBlogs");
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> AddComment([FromBody] Comment comment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                comment.CreatedByUserId = user.Id;
                comment.UserName = user.FullName ?? user.UserName;
            }
            
            comment.CommentDate = DateTime.Now;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Json(new
            {
                username = comment.UserName,
                CommentDate = comment.CommentDate.ToString("MMMM dd, yyyy"),
                content = comment.Content
            });
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();
            
            var user = await _userManager.GetUserAsync(User);
            bool isCreator = user != null && comment.CreatedByUserId == user.Id;
            bool isAdmin = User.IsInRole("Admin");

            if (!isCreator && !isAdmin)
            {
                return Forbid();
            }
            
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> HideComment(int id)
        {
            if (!User.IsInRole("Admin")) return Forbid();
            
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();
            
            comment.IsHidden = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RestoreComment(int id)
        {
            if (!User.IsInRole("Admin")) return Forbid();
            
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();
            
            comment.IsHidden = false;
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ReportComment(int id, ReportReason reason, string? additionalDetails)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();
            
            var user = await _userManager.GetUserAsync(User);
            
            var report = new CommentReport
            {
                CommentId = id,
                Reason = reason,
                AdditionalDetails = additionalDetails,
                ReportedByUserId = user?.Id
            };
            
            comment.ReportedCount++;
            _context.CommentReports.Add(report);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task<string> UploadFileFolder(IFormFile file)
        {
            if (file == null) return "";
            var inputFileExtention = Path.GetExtension(file.FileName);
            var filename = Guid.NewGuid().ToString() + inputFileExtention;
            var wwwroothpath = _webHostEnvironment.WebRootPath;
            var imagesFolderPath = Path.Combine(wwwroothpath, "images");
            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }
            var filePath = Path.Combine(imagesFolderPath, filename);

            try
            {
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                return "Error Uploading Images " + ex.Message;
            }
            return "/images/" + filename;
        }
    }
}