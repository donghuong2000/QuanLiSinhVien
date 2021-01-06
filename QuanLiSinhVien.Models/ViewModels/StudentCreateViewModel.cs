using System.ComponentModel.DataAnnotations;

namespace QuanLiSinhVien.Models.ViewModels
{
    public class StudentCreateViewModel
    {
        [Required(ErrorMessage = "Phải nhập tên cho User")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phải nhập Mã sinh viên")]
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

        [Required(ErrorMessage = "Phải chọn lớp")]
        public string ClassId { get; set; }


    }
}
