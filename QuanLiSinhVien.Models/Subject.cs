using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLiSinhVien.Models
{
    public partial class Subject
    {
        public Subject()
        {
            StudentSubject = new HashSet<StudentSubject>();
        }

        public string Id { get; set; }
        public string TeacherId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int? Credits { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<StudentSubject> StudentSubject { get; set; }
    }
}
