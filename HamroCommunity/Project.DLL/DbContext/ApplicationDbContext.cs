using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
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


        public DbSet<Signature> Signatures { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<Citizenship> Citizenships { get; set; }

        public DbSet<CertificateImages> CertificateImages { get; set; }

        public DbSet<CitizenshipImages> CitizenshipImages { get; set; }




        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<VDC> Vdc { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Signature and Documents(1:m)
            builder.Entity<Signature>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SignatureURL).IsRequired(false);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasMany(s => s.Documents)
                      .WithOne(d => d.Signature)
                      .HasForeignKey(d => d.SignitureId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Documents and Signiture(1:m)
            builder.Entity<Documents>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DocumentType).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedBy).IsRequired(false);

                entity.HasOne(d => d.Signature)
                      .WithMany(s => s.Documents)
                      .HasForeignKey(d => d.SignitureId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Citizenship and Documents(1:m)
            builder.Entity<Citizenship>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasMany(x => x.Documents)
                .WithOne(x => x.Citizenship)
                .HasForeignKey(x => x.CitizenshipId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            #endregion

            #region Documents and Citizenship(m:1)
            builder.Entity<Documents>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(s=>s.Citizenship)
                .WithMany(s => s.Documents)
                .HasForeignKey(x=>x.CitizenshipId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region Certificate and CertificateImages(1:m)
            builder.Entity<Certificate>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Grade).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired(false);
                entity.Property(e => e.Type).IsRequired(false);
                entity.Property(e => e.Board).IsRequired();

                entity.HasMany(d => d.CertificateImages)
                      .WithOne(e => e.Certificate)
                      .HasForeignKey(d => d.CertificateId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region CertificateImages and Certificate(m:1)
            builder.Entity<CertificateImages>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CertificateImgURL).IsRequired();
                entity.Property(e => e.CertificateId).IsRequired();

                entity.HasOne(e => e.Certificate)
                      .WithMany(e => e.CertificateImages)
                      .HasForeignKey(e => e.CertificateId); // Corrected foreign key configuration
            });
            #endregion

            #region Citizenship and CitizenshipImages(1:m)
            builder.Entity<Citizenship>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IssuedDate).IsRequired();
                entity.Property(e => e.IssuedDistrict).IsRequired();
                entity.Property(e => e.VdcOrMunicipality).IsRequired();
                entity.Property(e => e.WardNumber).IsRequired();
                entity.Property(e => e.DOB).IsRequired();
                entity.Property(e => e.CitizenshipNumber).IsRequired();


                entity.HasMany(d => d.CitizenshipImages)
                .WithOne(e => e.Citizenship)
                .HasForeignKey(d => d.CitizenshipId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region CitizenshipImages and dCitizenship(m:1)
            builder.Entity<CitizenshipImages>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl);
                entity.Property(e => e.CreatedAt);
                entity.Property(e => e.CitizenshipId);


                entity.HasOne(e => e.Citizenship)
                .WithMany(e => e.CitizenshipImages)
                .HasForeignKey(e => e.CitizenshipId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Certificate and Documents(m:m)
            builder.Entity<CertificateDocuments>()
                .HasKey(ud => new { ud.DocumentsId, ud.CertificateId });

            builder.Entity<CertificateDocuments>()
                .HasOne(e => e.Certificate)
                .WithMany(e => e.CertificateDocuments)
                .HasForeignKey(ud => ud.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CertificateDocuments>()
                .HasOne(e => e.Documents)
                .WithMany(e => e.certificateDocuments)
                .HasForeignKey(ud => ud.DocumentsId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion



            ////Configure many-to-many relationship
            //builder.Entity<UserDepartment>()
            //    .HasKey(ud => new { ud.UserId, ud.DepartentId });

            //builder.Entity<UserDepartment>()
            //.HasOne(ud => ud.User)
            //.WithMany(u => u.UserDepartments)
            //.HasForeignKey(ud => ud.UserId);

            //builder.Entity<UserDepartment>()
            //    .HasOne(ud => ud.Department)
            //    .WithMany(d => d.UserDepartments)
            //    .HasForeignKey(x => x.DepartentId);



            //#region Documents and Certificate(1:m)
            //builder.Entity<Documents>()
            //    .HasMany(d => d.Certificates)
            //    .WithOne(c => c.Documents)
            //    .HasForeignKey(c => c.DocumentsId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //#endregion
            //#region Documents and Citizenship(1:1)
            //builder.Entity<Documents>()
            //    .HasOne(d => d.Citizenship)
            //    .WithOne(c => c.Documents)
            //    .HasForeignKey<Citizenship>(d => d.DocumentsId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //#endregion
            //#region Document and Signature(1:1)
            //builder.Entity<Documents>()
            //    .HasOne(x => x.Signature)
            //    .WithOne(c => c.Documents)
            //    .HasForeignKey<Signature>(d => d.SignatureId)
            //    .OnDelete(DeleteBehavior.Cascade);
            //#endregion

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
