using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLiSinhVien.Models.ViewModels
{
    public class InfoSubjectViewModel
    {
        public string studentId { get; set; }
        public string subjectId { get; set; }
        public string studentName { get; set; }
        public double? avgScore { get; set; }

        public List<ExamScore> score { get; set; }
    }
}
