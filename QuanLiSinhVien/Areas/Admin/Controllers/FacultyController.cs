using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiSinhVien.Data;
using QuanLiSinhVien.Models;

namespace QuanLiSinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FacultyController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FacultyController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var obj =  _db.Faculties.Include(x => x.Teachers).Select(x => new
            {
                id = x.Id,
                name = x.Name,
                teacher = x.Teachers == null ? "Empty" : x.Teachers.Count + " Teachers"

            }).ToList();
            return Json(new { data = obj });
        }
        public IActionResult Upsert(string id)
        {

            if (id == null)
            {
                return View(new Faculty());
            }
            var oldFaculty = _db.Faculties.FirstOrDefault(x => x.Id == id);
            if (oldFaculty == null)
            {
                return NotFound();
            }
            return View(oldFaculty);
        }
        [HttpPost]
        public IActionResult Upsert(Faculty faculty)
        {

            if (ModelState.IsValid)
            {
                if (faculty.Id == null)
                {
                    _db.Faculties.Add(new Faculty { Id = Guid.NewGuid().ToString(), Name = faculty.Name });
                    _db.SaveChanges();
                    return RedirectToAction("index");
                }
                var oldFaculty = _db.Faculties.FirstOrDefault(x => x.Id == faculty.Id);
                if (oldFaculty != null)
                {
                    try
                    {
                        oldFaculty.Name = faculty.Name;
                        _db.Faculties.Update(oldFaculty);
                        _db.SaveChanges();
                        return RedirectToAction("index");
                    }
                    catch (Exception e)
                    {

                        ModelState.AddModelError("", e.Message);
                    }

                }
                ModelState.AddModelError("", "Error");
            }
            return View(faculty);
        }


        [HttpDelete]
        public IActionResult Delete(string id)
        {

            try
            {
                var obj = _db.Faculties.Find(id);
                _db.Faculties.Remove(obj);
                _db.SaveChanges();
                return Json(new { success = true, message = "Remove Class Success" });
            }
            catch (Exception e)
            {

                return Json(new { success = false, message = e.Message });
            }
        }

    }
}
