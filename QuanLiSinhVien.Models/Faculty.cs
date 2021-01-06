using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLiSinhVien.Models
{
    public partial class Faculty
    {
        public Faculty()
        {
            Teachers = new HashSet<Teacher>();
        }

        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
