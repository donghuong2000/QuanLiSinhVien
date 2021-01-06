using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLiSinhVien.Models
{
    public partial class Student
    {
        public Student()
        {
            StudentSubject = new HashSet<StudentSubject>();
        }

        [Required]
        public string PersonId { get; set; }
        [Required]
        public string StudentCode { get; set; }
        [Required]
        public string ClassId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<StudentSubject> StudentSubject { get; set; }
    }
}
