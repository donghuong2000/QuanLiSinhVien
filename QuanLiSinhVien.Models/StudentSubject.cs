namespace QuanLiSinhVien.Models
{
    public partial class StudentSubject
    {
        public string StudentId { get; set; }
        public string SubjectId { get; set; }
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
