using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLiSinhVien.Data;
using QuanLiSinhVien.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuanLiSinhVien.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SubjectController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()

        {
            ViewBag.SubjectList = new SelectList(_db.Subjects.Where(x => x.TeacherId == null).ToList(),"Id","Name");
            return View();
        }


        public IActionResult GetAll()
        {
            //lấy user ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


            // lấy danh sách các môn của giáo viên này
            var subjectsOfThisTeacher = _db.Subjects
                .Include(x => x.StudentSubject)
                .Where(x => x.TeacherId == userId)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    credits = x.Credits,
                    student = x.StudentSubject.Count()

                }).ToList();
            return Json(new { data = subjectsOfThisTeacher });
            
        }
        // đăng kí môn dạy
        public IActionResult Registration(string id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var subject = _db.Subjects.Find(id);
                subject.TeacherId = userId;
                _db.Subjects.Update(subject);
                _db.SaveChanges();
                return Json(new { success = true, message = "Registration Subject success" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });

            } 
        }


        public IActionResult Info(string id)
        {
            ViewBag.infoid = id;
            return View();
        }

        public IActionResult GetInfo(string id)
        {
            if (id == null)
                return NotFound();
            var obj = _db.StudentSubject
                .Where(x => x.SubjectId == id)
                .Include(x => x.Student).ThenInclude(x => x.Person)
                .Select(x => new
                {
                    studentId = x.SubjectId,
                    studentName = x.Student.Person.Name,
                    avgScore = _db.ExamScore.Where(y => x.SubjectId == id && y.StudentId == x.StudentId).Select(y => y.Score).Average(),
                    score = _db.ExamScore.Include(y => y.ExamType).Where(y => x.SubjectId == id && y.StudentId == x.StudentId)
                                         .Select(y => new
                                         {
                                             examtype = y.ExamType.Name,
                                             mark = y.Score
                                         })
                                         .ToList(),

                }).ToList();
                
            return Json(obj);
        }
    }
}
