using LaptopCart.Data;
using LaptopCart.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LaptopCart.Controllers
{
    public class AdminProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {

            return View(_context.Products.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if(product.Id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //If a new image is uploaded, handle the file upload.
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    //Generate a unique filename and save the new image (similar to Create)
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageFile.FileName);
                    var savePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                    // Ensure the images folder exists
                    var imagesDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    if (!Directory.Exists(imagesDir))
                    {
                        Directory.CreateDirectory(imagesDir);
                    }
                    //Save the new file
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }
                    //Save relative path to database
                    product.ImagePath = "/images/" + fileName;
                }
                //Update the product in the database
                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                //Delete the image file from the server if it exists and if needed
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImagePath.TrimStart('/').Replace("/", "\\"));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                //Remove the product from the database
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {

            if (product.ImageFile != null && product.ImageFile.Length > 0)
            {
                //Get the wwwroot path from the environment
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                //Clean and genertae a unique filename
                string originalFileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName).Replace(" ", "_");//Remove spaces
                string extension = Path.GetExtension(product.ImageFile.FileName);
                string uniqueFileName = $"{originalFileName}_{Guid.NewGuid():N}{extension}";

                //Ensure the /images folder exists
                string imageFolder = Path.Combine(wwwRootPath, "images");
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }
                //Path to save the image physically
                string filePath = Path.Combine(imageFolder, uniqueFileName);
                //Save file to server
                using(var stream = new FileStream(filePath,FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }
                //Save relative path(for Razor <img src=.....>)
                product.ImagePath = "/images/" + uniqueFileName;
                //[Optional] Verify file was saved - usful for debugging
                string confirmPath = Path.Combine(wwwRootPath, product.ImagePath.TrimStart('/'));
                if(!System.IO.File.Exists(confirmPath))
                {
                    throw new FileNotFoundException("Image not saved correctly", confirmPath);
                }

            }

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
            
        }
    }
}
