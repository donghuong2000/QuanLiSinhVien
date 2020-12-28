using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiSinhVien.Data;
using System.Linq;

namespace QuanLiSinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            ViewBag.NumStudent = _db.Students.Count();
            ViewBag.NumTeacher = _db.Teachers.Count();
            ViewBag.NumSubject = _db.Subjects.Count();
            return View();
        }
        public IActionResult Statistical_Student_Avg()
        {
            var a = _db.ExamScore.Include(x => x.Student)
                .GroupBy(x => new
                {
                    code = x.Student.StudentCode,
                    subject = x.SubjectId,
                })
                // lấy điểm trung bình theo môn
                .Select(x => new
                {
                    code = x.Key.code,
                    subject = x.Key.subject,
                    mark = x.Average(s => s.Score)

                })
                // lấy điểm trung bình theo sinh viên
                .GroupBy(x => x.code)
                .Select(x => new
                {
                    x.Key,
                    mark = x.Average(t => t.mark)
                })
                .ToList();

            var labels = a.Select(x => x.Key).ToArray();
            var values = a.Select(x => x.mark).ToArray();
            return Json(new {labels,values });
        }
    }
}
