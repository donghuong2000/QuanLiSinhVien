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
    public class ClassController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ClassController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
           var obj =  _db.Classes.Include(x => x.Students).Select(x => new {
                id = x.Id,
                name = x.Name,
                student = x.Students==null?"Empty":x.Students.Count + " students"
            })
            .ToList();
            return Json(new { data = obj });
        }
        public IActionResult Upsert(string id)
        {
           if(id==null)
           {
                return View(new Class());
           }
            var oldClass = _db.Classes.FirstOrDefault(x => x.Id == id);
            if(oldClass==null)
            {
                return NotFound();
            }
            return View(oldClass);
        }
        [HttpPost]
        public IActionResult Upsert(Class _class)
        {
            if (ModelState.IsValid)
            {
                if (_class.Id == null)
                {
                    _db.Classes.Add(new Class() { Id = Guid.NewGuid().ToString(), Name = _class.Name });
                    _db.SaveChanges();
                    return RedirectToAction("index");
                }
                var oldClass = _db.Classes.FirstOrDefault(x => x.Id == _class.Id);
                if(oldClass!= null)
                {
                    try
                    {
                        oldClass.Name = _class.Name;
                        _db.Classes.Update(oldClass);
                        _db.SaveChanges();
                        return RedirectToAction("index");
                    }
                    catch (Exception e )
                    {

                        ModelState.AddModelError("", e.Message);
                    }
                    
                }
                ModelState.AddModelError("", "Error");
            }
            return View(_class);
        }

        public IActionResult Delete(string id)
        {
            try
            {
                var obj = _db.Classes.Find(id);
                _db.Classes.Remove(obj);
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
