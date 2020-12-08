using System;
using System.Collections.Generic;

namespace QuanLiSinhVien.Models
{
    public partial class Student
    {
        public Student()
        {
            StudentSubject = new HashSet<StudentSubject>();
        }

        public string PersonId { get; set; }
        public string StudentCode { get; set; }
        public string ClassId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<StudentSubject> StudentSubject { get; set; }
    }
}
