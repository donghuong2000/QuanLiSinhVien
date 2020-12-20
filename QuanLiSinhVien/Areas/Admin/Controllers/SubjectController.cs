using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiSinhVien.Data;
using QuanLiSinhVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiSinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubjectController : Controller
    {

        private readonly ApplicationDbContext _db;
        public SubjectController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            var subjectlist = _db.Subjects
                .Include(x => x.Teacher).ThenInclude(x=>x.Person)
                .Include(x => x.StudentSubject)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    credits = x.Credits,
                    teach = x.Teacher==null? "No Assign Yet":x.Teacher.Person.Name,
                    student = x.StudentSubject == null ? "0 Student" : x.StudentSubject.Count() + " Student"

                }).ToList();
            return Json(new { data = subjectlist });
        }

        public IActionResult Upsert(string id)
        {
            Subject sub = new Subject();
            if (id == null)
                return View(sub);

            sub = _db.Subjects.FirstOrDefault(x => x.Id == id);
            if (sub == null)
                return NotFound();

            return View(sub);



        }
        [HttpPost]
        public IActionResult Upsert(Subject subject)
        {
           if(ModelState.IsValid)
            {
                // add
                if(subject.Id == null)
                {
                    subject.Id = Guid.NewGuid().ToString();
                    _db.Subjects.Add(subject);
                    _db.SaveChanges();
                    return RedirectToAction("index");
                }
                //update
                var oldSubject = _db.Subjects.FirstOrDefault(x => x.Id == subject.Id);
                if (oldSubject == null)
                    return NotFound();

                oldSubject.Name = subject.Name;
                oldSubject.Credits = subject.Credits;
                _db.Subjects.Update(oldSubject);
                _db.SaveChanges();
                return RedirectToAction("index");
           }     
            return View(subject);
        }

        public IActionResult Delete(string id)
        {
            try
            {
                var subject = _db.Subjects.Find(id);
                _db.Subjects.Remove(subject);
                _db.SaveChanges();
                return Json(new { success = true, message = " this subject delete success" });
            }
            catch (Exception e)
            {

                return Json(new { success = false, message = e.Message });
            }
        }
    }
}
