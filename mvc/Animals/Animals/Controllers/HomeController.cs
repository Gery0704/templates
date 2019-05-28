using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Animals.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Animals.Controllers
{
    public class HomeController : BaseController
    {
        private readonly MyDbContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public HomeController(MyDbContext context, ApplicationState applicationState, IHostingEnvironment environment)
            : base(applicationState)
        {   
            _context = context;
            _hostingEnvironment = environment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Animals.Where(s => s.free == true).ToListAsync());
        }
        public async Task<IActionResult> PictureList()
        {
            return View(await _context.AnimalPictures.ToListAsync());
        }

        public async Task<IActionResult> Details(int AnimalID)
        {
            return View(await _context.Animals.Where(s => s.ID == AnimalID).ToListAsync());
        }
        [HttpGet]
        public IActionResult Contact()
        {
            /*lenyiló lista saját értékekből*/
                ViewBag.List = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Dalmata", Value="Dalmata"},
                new SelectListItem() {Text="Palotapincsi", Value="Palotapincsi"},
                new SelectListItem() {Text="Németjuhász", Value="Németjuhász"},
                new SelectListItem() {Text="Bulldog", Value="Bulldog"},
            };
            return View();
        }

        [HttpPost]
        public IActionResult Contact(Notify not)
        {
            if (!ModelState.IsValid)
            {/*lenyiló lista saját értékekből*/

                ViewBag.List = new List<SelectListItem>()
                {
                    new SelectListItem() {Text="Dalmata", Value="Dalmata"},
                    new SelectListItem() {Text="Palotapincsi", Value="Palotapincsi"},
                    new SelectListItem() {Text="Németjuhász", Value="Németjuhász"},
                    new SelectListItem() {Text="Bulldog", Value="Bulldog"},
                };
            }
            _context.Notify.Add(not);
            _context.SaveChanges();

            return RedirectToAction("Index","Home");
        }


        [HttpGet]
        public IActionResult PictureUpload()
        {
            /*Lenyiló lista adatbázis tábla értékeiből*/
            ViewBag.List = new SelectList(_context.AnimalType, "ID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PictureUpload(AnimalPicModel animalpic)
        {
            AnimalPictures anp = new AnimalPictures();
            anp.Name = animalpic.Name;
            anp.AnimalType = animalpic.AnimalType;

            if (animalpic.Picture.Length > 0)
            {
                string ex = Path.GetExtension(animalpic.Picture.FileName);  /*csekkoljuk a kiterjesztést hogy kép e*/
                if (ex != ".png" && ex != ".jpg")
                {
                    ViewBag.List = new SelectList(_context.AnimalType, "ID", "Name");
                    return RedirectToAction("Index");
                }
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");  /*ide menti, wwwrootban kell Uploads mappa hozzá vagy amit meg adsz helyette*/
                var upfilePath = Path.Combine(uploads, animalpic.Picture.FileName);
                using (var fileStream = new FileStream(upfilePath, FileMode.Create))
                {
                    await animalpic.Picture.CopyToAsync(fileStream);
                }
            }
            anp.PicturePath = animalpic.Picture.FileName;

            _context.AnimalPictures.Add(anp);
            _context.SaveChanges();

            return RedirectToAction("Index");
                       
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public FileResult ImageForBuilding(Int32? AnimalID)
        { /*kép betöltése hogy látszódjon az oldalon*/
            if (AnimalID == null) 
                return File("~/images/NoImage.png", "image/png");
            
            Byte[] imageContent = _context.Animals
                .Where(a => a.ID == AnimalID)
                .Select(a => a.Picture)
                .FirstOrDefault();

            if (imageContent == null) 
                return File("~/images/NoImage.png", "image/png");

            return File(imageContent, "image/png");
        }
        public FileResult ImageForAnimal(Int32? AnimalID)
        {
            string path = _context.AnimalPictures
                .Where(a => a.ID == AnimalID)
                .Select(a => a.PicturePath)
                .FirstOrDefault();

            path = @"f:\waf\animalMVC\Animals\Animals\wwwroot\Uploads\" + path;   /*innen töltse le*/

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string fileName = "myfile.jpg"; /*ezen a néven töltse  le*/
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
