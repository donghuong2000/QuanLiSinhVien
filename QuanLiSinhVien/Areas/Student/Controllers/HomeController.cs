using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLiSinhVien.Data;
using QuanLiSinhVien.Models;
using QuanLiSinhVien.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuanLiSinhVien.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Person> _userManager;

        public HomeController(ApplicationDbContext db, UserManager<Person> userManager)
        {
            _db = db;
            _userManager = userManager;
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
                    .Where(s => s.SubjectId == x.SubjectId && s.StudentId == userId).Select(x => x.Score).Average()??0
                }).ToList();
            return Json(obj);
        }
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.SubjectList = new SelectList(_db.Subjects.Include(x => x.StudentSubject).Where(x => x.TeacherId != null && x.StudentSubject.Any(x => x.StudentId == userId) == false).ToList(), "Id", "Name");
            return View();
        }
        public IActionResult RegisterSubject(string id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var re = new StudentSubject() { StudentId = userId, SubjectId = id };
                _db.StudentSubject.Add(re);
                _db.SaveChanges();
                return Json(new { success = true, message = "Register Subject success" });
            }
            catch (Exception e)
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
            catch (Exception e)
            {

                return Json(new { success = false, message = e.InnerException.Message });
            }
        }


        public IActionResult Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var student = _db.Students.Include(x => x.Class).Include(x => x.Person).FirstOrDefault(x => x.PersonId == userId);
            StudentProfileViewModel vm = new StudentProfileViewModel();
            vm.Student = student;

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(StudentProfileViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var student = _db.Students.Include(x => x.Person).Where(x => x.PersonId == vm.Student.PersonId).FirstOrDefault();
                student.Person.Name = vm.Student.Person.Name;
                student.Person.Address = vm.Student.Person.Address;
                student.Person.Email = vm.Student.Person.Email;

                _db.Update(student);
                _db.SaveChanges();
                if (vm.IsChangePassworld)
                {// có đổi mk
                    if (vm.Password.Length < 4 || vm.CurrentPassword.Length < 4 || vm.ConfirmPassword.Length < 4)
                    {
                        ModelState.AddModelError("", "Mk ko hợp lệ");
                    }
                    else
                    {
                        try
                        {
                            var result = await _userManager.ChangePasswordAsync(student.Person, vm.CurrentPassword, vm.Password);
                            if (result.Succeeded)
                            {
                                return RedirectToAction("index");
                            }
                            else
                            {
                                foreach (var item in result.Errors)
                                {
                                    ModelState.AddModelError("", item.Description);
                                }

                            }
                        }
                        catch (Exception e)
                        {

                            ModelState.AddModelError("", e.Message);
                        }
                    }

                }
                else
                {
                    return RedirectToAction("index");
                }

            }
            return View(vm);
        }
    }
}
