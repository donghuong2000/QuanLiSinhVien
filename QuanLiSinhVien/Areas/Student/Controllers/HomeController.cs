using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLiSinhVien.Data;
using QuanLiSinhVien.Models;

namespace QuanLiSinhVien.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult GetAllSubject()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var obj = _db.StudentSubject
                .Include(x => x.Subject)
                .Where(x => x.StudentId == userId)
                .Select(x => new
                {
                    subjectid = x.SubjectId,
                    studentid = x.StudentId,
                    subjectName = x.Subject.Name,
                    score = _db.ExamScore.Include(y => y.ExamType).Where(y => y.SubjectId == x.SubjectId && y.StudentId == userId)
                                         .Select(y => new
                                         {
                                             examtype = y.ExamType.Name,
                                             mark = y.Score
                                         })
                                         .ToList(),
                    avgScore = _db.ExamScore
                    .Where(s => s.SubjectId == x.SubjectId && s.StudentId == userId).Select(x => x.Score).Average()
                }).ToList();
            return Json(obj);
        }
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.SubjectList = new SelectList(_db.Subjects.Include(x => x.StudentSubject).Where(x => x.TeacherId != null && x.StudentSubject.Any(x => x.StudentId == userId) == false).ToList(),"Id","Name");
            return View();
        }
        public IActionResult RegisterSubject(string id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var re = new StudentSubject() {StudentId = userId,SubjectId = id };
                _db.StudentSubject.Add(re);
                _db.SaveChanges();
                return Json(new { success = true, message = "Register Subject success" });
            }
            catch (Exception e )
            {

                return Json(new { success = false, message = e.InnerException.Message });

            }

        }


        public IActionResult CancelSubject(string id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var obj = _db.StudentSubject.Where(x => x.StudentId == userId && x.SubjectId == id).FirstOrDefault();
                _db.StudentSubject.Remove(obj);
                _db.SaveChanges();
                return Json(new { success = true, message = "Canncel Subject success" });
            }
            catch (Exception e )
            {

                return Json(new { success = false, message = e.InnerException.Message });
            }
        }
    }
}
