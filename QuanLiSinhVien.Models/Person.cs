using Microsoft.AspNetCore.Identity;

namespace QuanLiSinhVien.Models
{
    public class Person : IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual Student Students { get; set; }
        public virtual Teacher Teachers { get; set; }
    }
}
