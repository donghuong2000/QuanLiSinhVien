using System.Collections.Generic;

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
        public string Name { get; set; }
        public int? Credits { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<StudentSubject> StudentSubject { get; set; }
    }
}
