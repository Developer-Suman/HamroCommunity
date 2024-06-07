using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Nashu> Nashu { get; set; }
        public DbSet<UserDepartment> UserDepartments { get; set; }


        public DbSet<Signature> signatures { get; set; }
        public DbSet<Certificate> certificates { get; set; }
        public DbSet<CertificatesDocumentsImages> CertificatesDocumentsImages { get; set; }
        public DbSet<Documents> Documents { get; set; }






        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<VDC> Vdc { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Configure many-to-many relationship
            builder.Entity<UserDepartment>()
                .HasKey(ud => new { ud.UserId, ud.DepartentId });

            builder.Entity<UserDepartment>()
            .HasOne(ud => ud.User)
            .WithMany(u => u.UserDepartments)
            .HasForeignKey(ud => ud.UserId);

            builder.Entity<UserDepartment>()
                .HasOne(ud => ud.Department)
                .WithMany(d => d.UserDepartments)
                .HasForeignKey(x => x.DepartentId);



            #region Documents and Certificate(1:m)
            builder.Entity<Documents>()
                .HasMany(d => d.Certificates)
                .WithOne(c => c.Documents)
                .HasForeignKey(c => c.DocumentsId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
            #region Documents and Citizenship(1:1)
            builder.Entity<Documents>()
                .HasOne(d => d.Citizenship)
                .WithOne(c => c.Documents)
                .HasForeignKey<Citizenship>(d => d.DocumentsId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
            #region Document and Signature(1:1)
            builder.Entity<Documents>()
                .HasOne(x => x.Signature)
                .WithOne(c => c.Documents)
                .HasForeignKey<Signature>(d => d.SignatureId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            //#region Certificate and DocumentsImages(m:m)
            //builder.Entity<CertificatesDocumentsImages>()
            //    .HasKey(cdi => new { cdi.CertificatesId, cdi.DocumentsImagesId });

            //builder.Entity<CertificatesDocumentsImages>()
            //    .HasOne(cdi => cdi.Certificate)
            //    .WithMany(c => c.CertificatesDocumentsImage)
            //    .HasForeignKey(cdi => cdi.CertificatesId)
            //    .OnDelete(DeleteBehavior.Cascade);


            //builder.Entity<CertificatesDocumentsImages>()
            //    .HasOne(cdi => cdi.DocumentImages)
            //    .WithMany(c => c.CertificatesDocumentsImage)
            //    .HasForeignKey(cdi => cdi.DocumentsImagesId)
            //    .OnDelete(DeleteBehavior.Cascade);


            //#endregion

            #region Citizenship and DocumentsImages(1:m)
            builder.Entity<Citizenship>()
                .HasMany(c => c.CitizenshipImages)
                .WithOne(cd => cd.Citizenship)
                .HasForeignKey(x => x.CitizenshipId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

        }
    }
}
