namespace QuanLiSinhVien.Models
{
    public partial class ExamScore
    {
        public string StudentId { get; set; }
        public string SubjectId { get; set; }
        public string ExamTypeId { get; set; }
        public int? Score { get; set; }

        public virtual ExamType ExamType { get; set; }
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
