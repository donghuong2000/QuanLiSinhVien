using Microsoft.AspNetCore.Mvc;
using QuanLiSinhVien.Data;
using QuanLiSinhVien.Models;
using System;
using System.Linq;

namespace QuanLiSinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExamTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ExamTypeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            var obj = _db.ExamType.ToList();
            return Json(new { data = obj });
        }
        public IActionResult Upsert(string id)
        {
            if (id == null)
                return View(new ExamType());
            var ex = _db.ExamType.FirstOrDefault(x => x.Id == id);
            if (ex == null)
                return NotFound();
            return View(ex);

        }
        [HttpPost]
        public IActionResult Upsert(ExamType et)
        {
            if (ModelState.IsValid)
            {
                if (et.Id == null)
                {
                    _db.ExamType.Add(new ExamType() { Id = Guid.NewGuid().ToString(), Name = et.Name });
                    _db.SaveChanges();
                    return RedirectToAction("index");
                }
                var oldEt = _db.ExamType.FirstOrDefault(x => x.Id == et.Id);
                if (oldEt != null)
                {
                    try
                    {
                        oldEt.Name = et.Name;
                        _db.ExamType.Update(oldEt);
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
            return View(et);

        }

        public IActionResult Delete(string id)
        {
            try
            {
                var obj = _db.ExamType.Find(id);
                _db.ExamType.Remove(obj);
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
