﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLiSinhVien.Models.ViewModels
{
    public class TeacherCreateViewModel
    {
        [Required(ErrorMessage = "Phải nhập tên cho User")]
        public string Name { get; set; }
        public string StudentCode { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Email không hợp lệ")]
        [Required(ErrorMessage = "Phải nhập Email")]
        public string Mail { get; set; }

        

        [Required(ErrorMessage = "Phải nhập sdt")]
        [MinLength(8, ErrorMessage = "SDT không hợp lệ")]
        public string Sdt { get; set; }

        [Required(ErrorMessage = "Phải nhập UserName")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Phải nhập Pass")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ComfirmPassword { get; set; }

        public string FaculityId { get; set; }


    }
}