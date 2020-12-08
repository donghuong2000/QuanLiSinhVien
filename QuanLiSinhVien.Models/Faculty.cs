using System;
using System.Collections.Generic;

namespace QuanLiSinhVien.Models
{
    public partial class Faculty
    {
        public Faculty()
        {
            Teachers = new HashSet<Teacher>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
