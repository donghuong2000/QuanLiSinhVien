﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLiSinhVien.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Subjects = new HashSet<Subject>();
        }

        public string PersonId { get; set; }
        [Required]
        public string FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
