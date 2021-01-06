using System.ComponentModel.DataAnnotations;

namespace QuanLiSinhVien.Models
{
    public partial class ExamType
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
