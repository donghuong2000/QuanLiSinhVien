using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiSinhVien.Data;
using QuanLiSinhVien.Models;
using QuanLiSinhVien.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuanLiSinhVien.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Person> _userManager;
        public ProfileController(ApplicationDbContext db, UserManager<Person> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var obj = _db.Teachers
                .Include(x => x.Person)
                .Include(x => x.Faculty)
                .FirstOrDefault(x => x.PersonId == userId);
            var vm = new TeacherProfileViewModel();
            vm.Teacher = obj;
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Index(TeacherProfileViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var teacher = _db.Teachers.Include(x => x.Person).Where(x => x.PersonId == vm.Teacher.PersonId).FirstOrDefault();
                teacher.Person.Name = vm.Teacher.Person.Name;
                teacher.Person.Address = vm.Teacher.Person.Address;
                teacher.Person.Email = vm.Teacher.Person.Email;

                _db.Update(teacher);
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
                            var result = await _userManager.ChangePasswordAsync(teacher.Person, vm.CurrentPassword, vm.Password);
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
                    return RedirectToAction("index", "home");
                }

            }
            return View(vm);
        }
    }
}
