using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLiSinhVien.Data;
using QuanLiSinhVien.Models;
using QuanLiSinhVien.Models.ViewModels;
using QuanLiSinhVien.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiSinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherController : Controller
    {

        private readonly UserManager<Person> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public TeacherController(UserManager<Person> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.ListFaculty = new SelectList(_db.Faculties.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TeacherCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new Person()
                {
                    UserName = vm.Username,
                    Name = vm.Name,
                    Email = vm.Name,
                    PhoneNumber = vm.Sdt,

                };
                var result = await _userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, SD.Teacher);
                    var s = await _userManager.FindByNameAsync(user.UserName);
                    var teacher = new Models.Teacher() { PersonId = s.Id, FacultyId = vm.FaculityId };
                    _db.Teachers.Add(teacher);
                    _db.SaveChanges();
                    return RedirectToAction("index", "Teacher", new { area = "admin" });
                }
                else
                {
                    foreach (var er in result.Errors)
                    {

                        ModelState.AddModelError("", er.Description);
                    }
                }

            }
            ViewBag.ListFaculty = new SelectList(_db.Faculties.ToList(), "Id", "Name");
            return View(vm);
        }
        public IActionResult GetAll()
        {
            var obj = _db.Teachers.Include(x => x.Person).Include(x => x.Faculty).Select(x => new
            {
                id = x.Person.Id,

                username = x.Person.UserName,
                email = x.Person.Email,
                sdt = x.Person.PhoneNumber,
                fa = x.Faculty.Name,
                lockout = _userManager.IsLockedOutAsync(x.Person).Result
            });
            return Json(new { data = obj });
        }
        public async Task<IActionResult> LockUnLock(string id)
        {
            var obj = await _userManager.FindByIdAsync(id);
            var IsLock = await _userManager.IsLockedOutAsync(obj);
            if (IsLock)
            {
                obj.LockoutEnd = DateTimeOffset.Now;
                await _userManager.UpdateAsync(obj);
                return Json(new { success = true, message = "đã mở khóa tài khoản" });
            }
            else
            {
                obj.LockoutEnd = DateTimeOffset.Now.AddYears(1);
                await _userManager.UpdateAsync(obj);
                return Json(new { success = true, message = "đã khóa tài khoản" });
            }


        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var obj = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(obj);
                if (result.Succeeded)
                {

                    return Json(new { success = true, message = "đã xóa tài khoản thành công" });
                }
                else
                {

                    return Json(new { success = false, message = "Lỗi Khi thực hiện thao tác này" });
                }
            }
            catch (Exception e)
            {

                return Json(new { success = false, message = e.InnerException.Message });
            }




        }
    }
}
