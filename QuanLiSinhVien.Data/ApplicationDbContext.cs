using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLiSinhVien.Models;

namespace QuanLiSinhVien.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<ExamScore> ExamScore { get; set; }
        public virtual DbSet<ExamType> ExamType { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<StudentSubject> StudentSubject { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<ExamScore>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ExamTypeId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.ExamType)
                    .WithMany()
                    .HasForeignKey(d => d.ExamTypeId)
                    .HasConstraintName("FK_ExamScore_ExamType");

                entity.HasOne(d => d.Student)
                    .WithMany()
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_ExamScore_Student");

                entity.HasOne(d => d.Subject)
                    .WithMany()
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_ExamScore_Subject");
            });

            modelBuilder.Entity<ExamType>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Address).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<StudentSubject>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.SubjectId })
                    .HasName("PK__StudentS__A80491A3166AF5E6");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentSubject)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subject_Student");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.StudentSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_Subject");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__Students__AA2FFBE5487AA4B4");

                entity.HasIndex(e => e.StudentCode)
                    .HasName("UQ__Students__1FC8860498D23824")
                    .IsUnique();

                entity.Property(e => e.PersonId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.ClassId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Student_Class");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Students)
                    .HasForeignKey<Student>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Student_Person");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(450);

                entity.Property(e => e.TeacherId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK_Subjects_Teachers");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__Teachers__AA2FFBE5DD0C1303");

                entity.Property(e => e.PersonId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.FacultyId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.FacultyId)
                    .HasConstraintName("FK_Teachers_Faculties");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Teachers)
                    .HasForeignKey<Teacher>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Teachers_Person");
            });


        }


    }
}
