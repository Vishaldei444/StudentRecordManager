using System;

namespace StudentRecordManager
{
    public class Student
    {
        public int RollNo { get; set; }
        public string Name { get; set; } = "";
        public DateTime DOB { get; set; }
        public double Marks { get; set; }

        public int CalculateAge()
        {
            DateTime today = DateTime.Today;
            int age = today.Year - DOB.Year;

            if (DOB.Date.AddYears(age) > today)
            {
                age--;
            }

            return age;
        }
    }
}