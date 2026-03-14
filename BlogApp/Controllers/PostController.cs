using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        private readonly string[] allowedExtention = { ".jpg", ".jpeg", ".png" };

        public PostController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
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
        [HttpGet]
        public IActionResult Index(int? categoryId)
        {
            var postQuery = _context.Posts.Include(p => p.Category).AsQueryable();
            if (categoryId.HasValue)
            {
                postQuery = postQuery.Where(p => p.Category.Id == categoryId);
            }
            var posts = postQuery.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(posts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                var inptfileExtention = Path.GetExtension(postViewModel.FeatureImage.FileName).ToLower();
                bool isAllowed = allowedExtention.Contains(inptfileExtention);
                if (!isAllowed)
                {
                    ModelState.AddModelError("", "Invalid image format,allowed format are  .jpg,.jpeg.png");
                    return View(postViewModel);
                }
                postViewModel.Post.FeatureImagePath = await UploadFileFolder(postViewModel.FeatureImage);
                await _context.Posts.AddAsync(postViewModel.Post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var postviewmodel = new PostViewModel();

            postviewmodel.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(postViewModel);



        }
        [Authorize]
        public JsonResult AddComment([FromBody] Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return Json(new
            {
                username = comment.UserName,
                CommentDate = comment.CommentDate.ToString("MMMM dd, yyyy"),
                content = comment.Content


            });


        }
        private async Task<string> UploadFileFolder(IFormFile file)
        {
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

            ;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _context.Posts.Include(p => p.Category).Include(p => p.Comments)
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }
            return View(post);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var Postfromdb= await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (Postfromdb == null)
            {
                return NotFound();
            }


            EditViewModel editViewModel = new EditViewModel
            {
                Post = Postfromdb,
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

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var postFromDb = await _context.Posts.FindAsync(id);
            if (postFromDb == null)
            {
                return NotFound();
            }
            return View(postFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var postFromDb = await _context.Posts.FindAsync(id);
            if (postFromDb == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(postFromDb.FeatureImagePath))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", Path.GetFileName(postFromDb.FeatureImagePath));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _context.Posts.Remove(postFromDb);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel editViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editViewModel);
            }
             var postfromdb= await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p=>p.Id == editViewModel.Post.Id);
            if(postfromdb == null)
            {
                return NotFound();
            }
            if(editViewModel.FeatureImage!=null)
            {
                var inptfileExtention = Path.GetExtension(editViewModel.FeatureImage.FileName).ToLower();
                bool isAllowed = allowedExtention.Contains(inptfileExtention);
                if (!isAllowed)
                {
                    ModelState.AddModelError("", "Invalid image format,allowed format are  .jpg,.jpeg.png");
                    return View(editViewModel);
                }
                var existingFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images",
                    Path.GetFileName(postfromdb.FeatureImagePath));
                if (System.IO.File.Exists(existingFilePath))
                {
                   System.IO.File.Delete(existingFilePath);
                }

                editViewModel.Post.FeatureImagePath = await 
                    UploadFileFolder(editViewModel.FeatureImage);

            }
            else
            {
                editViewModel.Post.FeatureImagePath= postfromdb.FeatureImagePath; 
            }
            _context.Posts.Update(editViewModel.Post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }




    }


}