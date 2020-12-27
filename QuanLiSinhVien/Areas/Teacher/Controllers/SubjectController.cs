using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLiSinhVien.Data;
using QuanLiSinhVien.Models;
using QuanLiSinhVien.Models.ViewModels;
using QuanLiSinhVien.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuanLiSinhVien.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
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
                .Select(x => new InfoSubjectViewModel
                {
                    studentId = x.StudentId,
                    subjectId = x.SubjectId,
                    studentName = x.Student.Person.Name,
                    avgScore = _db.ExamScore.Where(y => y.SubjectId == id && y.StudentId == x.StudentId).Select(y => y.Score).Average(),


                    

                }).ToList();
            foreach (var item in obj)
            {
                item.score = _db.ExamScore
                    .Include(x=>x.ExamType)
                    .Where(y => y.SubjectId == id && y.StudentId == item.studentId)
                    .ToList();
            }
            return Json(obj);
        }

        
        public IActionResult MarkStudent(string subjectId, string studentId)
        {
            ViewBag.ExamTypeList = new SelectList(_db.ExamType.ToList(), "Id", "Name");
           

            var isExits = _db.StudentSubject.Any(x => x.StudentId == studentId && x.SubjectId == subjectId);
            if(isExits)
            {
                ViewBag.Student = _db.Students.Find(studentId);
                ViewBag.Subject = _db.Subjects.Find(subjectId);
                var obj = _db.ExamScore
                    .Where(x => x.StudentId == studentId && x.SubjectId == subjectId)
                    .ToList();
                return View(obj);               
            }    

            return NotFound();
        }
        [HttpPost]
        public IActionResult MarkStudent(string subjectId,string studentId,string[] examType,int[] score)
        {
            var transaction = _db.Database.BeginTransaction();
            try
            {

                if(examType.Distinct().Count()<examType.Count())
                {
                    throw new Exception("duplicated examtype");
                }    
                List<ExamScore> listScore = new List<ExamScore>();
                for (int i = 0; i < examType.Length; i++)
                {
                    listScore.Add(new ExamScore() {ExamTypeId = examType[i],StudentId =studentId,SubjectId =subjectId,Score = score[i] });
                }
                


                _db.Database.ExecuteSqlRaw("Delete ExamScore WHERE StudentId={0} AND SubjectId={1}", studentId, subjectId);

                foreach (var item in listScore)
                {
                    _db.Database.ExecuteSqlRaw("INSERT INTO ExamScore VALUES ({0},{1},{2},{3})", item.StudentId, item.SubjectId, item.ExamTypeId,item.Score);
                }


                _db.SaveChanges();
                transaction.Commit();
                return RedirectToAction("Info",new {id = subjectId });
            }
            catch (Exception e)
            {
                ViewBag.ExamTypeList = new SelectList(_db.ExamType.ToList(), "Id", "Name");
                ViewBag.Student = _db.Students.Find(studentId);
                ViewBag.Subject = _db.Subjects.Find(subjectId); 
                ModelState.AddModelError("", e.Message);
                transaction.Rollback();
                var obj = _db.ExamScore
                   .Where(x => x.StudentId == studentId && x.SubjectId == subjectId)
                   .ToList();
                return View(obj);
            }



            
        }
    }
}
