using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLiSinhVien.Models.ViewModels
{
    public class TeacherProfileViewModel
    {
        public Teacher Teacher { get; set; }

        public bool IsChangePassworld { get; set; }
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password no like")]
        public string ConfirmPassword { get; set; }
    }
}
