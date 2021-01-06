using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLiSinhVien.Models
{
    public partial class Class
    {
        public Class()
        {
            Students = new HashSet<Student>();
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
