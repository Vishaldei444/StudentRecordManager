using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace StudentRecordManager
{
    internal class Program
    {
        static List<Student> students = new List<Student>();

        static readonly string filePath =
            Path.Combine(AppContext.BaseDirectory, "students.txt");

        static void Main()
        {
            if (!LoadRecords())
            {
                return;
            }

            while (true)
            {
                Console.WriteLine("\n===== STUDENT RECORD MANAGER =====");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. View All Students");
                Console.WriteLine("3. Search Student by Name");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");

                string? input = Console.ReadLine();
                if (input == null)
                {
                    return;
                }

                switch (input.Trim())
                {
                    case "1":
                        AddStudent();
                        break;

                    case "2":
                        ViewStudents();
                        break;

                    case "3":
                        SearchStudent();
                        break;

                    case "4":
                        DeleteStudent();
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Please enter a number from 1 to 5.");
                        break;
                }
            }
        }

        static void AddStudent()
        {
            Console.Write("Enter roll number: ");
            if (!int.TryParse(Console.ReadLine(), out int rollNo) || rollNo <= 0)
            {
                Console.WriteLine("Enter a valid positive roll number.");
                return;
            }

            if (students.Exists(student => student.RollNo == rollNo))
            {
                Console.WriteLine("This roll number already exists.");
                return;
            }

            Console.Write("Enter student name: ");
            string name = (Console.ReadLine() ?? "").Trim();

            if (name.Length == 0)
            {
                Console.WriteLine("Name cannot be blank.");
                return;
            }

            Console.Write("Enter DOB (dd-MM-yyyy): ");
            string dobInput = (Console.ReadLine() ?? "").Trim();

            if (!DateTime.TryParseExact(
                    dobInput,
                    "dd-MM-yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime dob) || dob > DateTime.Today)
            {
                Console.WriteLine("Enter a valid DOB in dd-MM-yyyy format.");
                return;
            }

            Console.Write("Enter marks (0 to 100): ");
            if (!double.TryParse(Console.ReadLine(), out double marks)
                || !double.IsFinite(marks) || marks < 0 || marks > 100)
            {
                Console.WriteLine("Marks must be between 0 and 100.");
                return;
            }

            Student student = new Student
            {
                RollNo = rollNo,
                Name = name,
                DOB = dob,
                Marks = marks
            };

            students.Add(student);

            if (SaveRecords())
            {
                Console.WriteLine("Student added successfully!");
            }
            else
            {
                students.Remove(student);
            }
        }

        static void ViewStudents()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No student records available.");
                return;
            }

            foreach (Student student in students)
            {
                PrintStudent(student);
            }
        }

        static void SearchStudent()
        {
            Console.Write("Enter name to search: ");
            string searchName = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (searchName.Length == 0)
            {
                Console.WriteLine("Please enter a name.");
                return;
            }

            bool found = false;

            foreach (Student student in students)
            {
                if (student.Name.ToUpperInvariant().Contains(searchName))
                {
                    PrintStudent(student);
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No matching student found.");
            }
        }

        static void DeleteStudent()
        {
            Console.Write("Enter roll number to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int rollNo) || rollNo <= 0)
            {
                Console.WriteLine("Enter a valid positive roll number.");
                return;
            }

            int index = students.FindIndex(student => student.RollNo == rollNo);

            if (index < 0)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Student removedStudent = students[index];
            students.RemoveAt(index);

            if (SaveRecords())
            {
                Console.WriteLine("Student deleted successfully!");
            }
            else
            {
                students.Insert(index, removedStudent);
            }
        }

        static void PrintStudent(Student student)
        {
            Console.WriteLine(
                $"Roll No: {student.RollNo} | Name: {student.Name} | " +
                $"DOB: {student.DOB:dd-MM-yyyy} | Age: {student.CalculateAge()} | " +
                $"Marks: {student.Marks}");
        }

        static bool SaveRecords()
        {
            try
            {
                string text = JsonSerializer.Serialize(
                    students, new JsonSerializerOptions { WriteIndented = true });

                string tempPath = filePath + ".tmp";
                File.WriteAllText(tempPath, text);
                File.Move(tempPath, filePath, overwrite: true);
                return true;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                Console.WriteLine("Could not save records: " + ex.Message);
                return false;
            }
        }

        static bool LoadRecords()
        {
            if (!File.Exists(filePath))
            {
                return true;
            }

            try
            {
                string text = File.ReadAllText(filePath);
                students = JsonSerializer.Deserialize<List<Student>>(text)
                    ?? throw new JsonException("Expected a list of student records.");

                return true;
            }
            catch (Exception ex) when (
                ex is IOException or UnauthorizedAccessException or JsonException)
            {
                Console.WriteLine("Could not load records: " + ex.Message);
                return false;
            }
        }
    }
}