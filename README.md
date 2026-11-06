# Student Record Manager

A menu-driven C# console application for managing student records.

## Features

- Add students with a roll number, name, date of birth, and marks.
- View all records with each student's calculated age.
- Search by full or partial name, regardless of capitalization.
- Delete a record by roll number.
- Save changes to `students.txt` and load records when the app starts.

## How to Run

1. Clone or download this repository.
2. Open `StudentRecordManager.slnx` in Visual Studio with the .NET 10 SDK installed.
3. Press **Ctrl + F5** to run the application.

## Menu

| Option | Action |
| --- | --- |
| 1 | Add Student |
| 2 | View All Students |
| 3 | Search Student by Name |
| 4 | Delete Student |
| 5 | Exit |

Enter the date of birth in `dd-MM-yyyy` format. Marks must be between 0 and 100.

The app stores records as JSON text in `students.txt` in the build output folder.
