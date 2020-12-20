using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Registration()
        {
            return View();
        }
    }
}
