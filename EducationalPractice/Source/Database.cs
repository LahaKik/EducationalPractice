using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EducationalPractice
{
    
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        [Required]
        public required string? NameClient { get; set; }
        [Required]
        public required string? NameDirector { get; set; }
        public string? Address { get; set; }
        public string? Theme { get; set; }
        public string? Content { get; set; }
        public string? Resolution { get; set; }
        public Status Status { get; set; }
        
        public string? Note { get; set; }
        [NotMapped]
        public string? QRPath { get; set; }
        public void CopyValues(User user)
        {
            NameClient = user.NameClient;
            NameDirector = user.NameDirector;
            Address = user.Address;
            Theme = user.Theme;
            Content = user.Content;
            Resolution = user.Resolution;
            Status = user.Status;
            Note = user.Note;
            QRPath = null;
        }
    }

    public class UsersDB : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        
        public UsersDB() : base() 
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Users.db");
        }
    }

    public enum Status
    {
        Created,
        Reviewed,
        Rejected
    }

    public class StringValidator : ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace((string)value))
                return new System.Windows.Controls.ValidationResult(false, "Поле не должно быть пустым.");
            else
                return System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
}
