using System;

namespace SchoolApp.Models
{
    public class Student
    {
        public int Id { get; set; }  // This is the primary key
        public string Name { get; set; }
        public string RollNumber { get; set; }
        public string GradeLevel { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
