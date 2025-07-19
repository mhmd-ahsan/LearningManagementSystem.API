# 🎓 Learning Management System - Backend API (ASP.NET Core + Dapper + MySQL)

This is the **backend API** for a complete **Learning Management System (LMS)** built using **ASP.NET Core**, **Dapper**, and MySQL.  
It supports role-based access and powers features for Admins, Teachers, and Students.

---

## 📌 Features

### 🔐 Authentication & Authorization
- JWT-based secure login
- Role-based access: Admin, Teacher, Student
- Custom JWT token generation via helper class

### 👨‍💼 Admin Features
- Create teachers, students, and assign roles
- Create and manage courses, batches, and subjects
- Assign teachers to courses
- View system-wide data

### 👩‍🏫 Teacher Features
- Upload lessons (PDFs, Videos)
- Post quizzes and questions
- Mark attendance for students
- View enrolled students per course

### 👨‍🎓 Student Features
- View enrolled courses and lessons
- Attempt quizzes
- View quiz results and attendance

---

## 🛠️ Tech Stack

| Layer         | Technology               |
|---------------|---------------------------|
| Backend       | ASP.NET Core Web API      |
| ORM           | Dapper (Micro ORM)        |
| Database      | MySQL (via Workbench) |
| Authentication| JWT Bearer Token          |
| Architecture  | Layered + DTOs            |

---

## 🗂️ Project Structure

```

LearningManagementSystem.API/
│
├── Controllers/        # API endpoints (Teacher, Admin, Student)
├── Dtos/               # Request/Response DTOs
├── Models/             # MySQL database models
├── Repositories/       # Dapper DB logic (Interfaces & Implementations)
├── Helpers/            # JWT token generation & REsponseHelper for Better responses
├── Data/               # DapperContext and DB connection
├── Program.cs          # Entry point & middleware setup
└── appsettings.json    # JWT keys and MySQL connection string

````

---

## 🗃️ MySQL Database Tables

| Table Name            | Description |
|-----------------------|-------------|
| `Users`               | All system users (Admin, Teacher, Student) |
| `Roles`               | Available roles in the system |
| `UserRoles`           | User-role mapping table |
| `Students`            | Student records |
| `StudentProfiles`     | Extra profile info (optional) |
| `TeacherProfiles`     | Teacher bio, education, experience |
| `Courses`             | Course metadata |
| `Lessons`             | Uploaded lesson content |
| `Enrollments`         | Which students are in which courses |
| `Attendance`          | Attendance data linked to course/student |
| `Quizzes`             | Quiz definitions |
| `QuizQuestions`       | Questions inside each quiz |
| `QuizResults`         | Student responses and results |

---

## 🚀 Getting Started

### 🔧 Requirements
- Visual Studio 2022 or later
- .NET 7 SDK or later
- MySQL Workbench

### 🧰 Setup Steps

### 1. Clone the repo:
   
   git clone: https://github.com/your-username/LearningManagementSystem.API.git
   

### 2. Open in Visual Studio.

3. Update `appsettings.json`:

   "ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;user=root;password=yourpassword;database=lms_db;"
},
"JwtSettings": {
  "SecretKey": "your_super_secret_key",
  "Issuer": "LMS",
  "Audience": "LMSUsers"
}


4. Build and run the project.

---

## 🔐 Sample JWT Response

After logging in, you receive a token like:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI...",
  "expiresIn": "3600"
}
```

Use this token in the **Authorization** header when calling secure APIs:

```
Authorization: Bearer {your_token_here}
```

---

## 💡 Usage Notes

* All endpoints are protected and require login
* Dapper is used with stored procedures and SQL queries
* Role-based routing handled through claims

---

## 👨‍💻 Developer Info

**Muhammad Ahsan Tahir**  
.NET Core & Angular Developer  

📧 Email: [ahsantahirmuhammad@gmail.com](mailto:ahsantahirmuhammad@gmail.com)  
🌐 LinkedIn: [linkedin.com/in/muhammad-ahsan-tahir-84392a351](https://www.linkedin.com/in/muhammad-ahsan-tahir-84392a351)

---

## 📄 License

This project is open-source and free to use for educational or commercial purposes.

---
