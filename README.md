# CMCS Claims Management System 
**PROG6212 POE – Final Submission**  
**Student:** Isabelle Kathryn Devlin  
**Student Number:** ST10445500  
**Lecturer:** Mr. Kim  

## Project Overview
A fully functional .NET 8 MVC web application that automates the lecturer hourly claim process with strict role-based access using sessions and other features.

### Key Features
- Session-based authentication & authorization
- Four distinct user roles: **HR**, **Lecturer**, **Programme Coordinator**, **Academic Manager**
- Full claim workflow: Submit → Verify → Approve/Reject
- HR creates all accounts and sets lecturer hourly rates
- Real-time jQuery validation on claim submission
- Micro APIs with full Swagger documentation
- PDF report generation using QuestPDF
- Clean, professional blue/white theme
- Responsive design with Bootstrap 5

### Technologies Used
- .NET 8 MVC + Razor Views
- Entity Framework Core + SQL Server LocalDB
- Sessions (custom authentication)
- QuestPDF (PDF reports)
- Swashbuckle (Swagger API)
- Bootstrap 5 + Bootstrap Icons
- BCrypt.Net-Next (password hashing)

### Login Credentials
| Role                  | Name                    | Email                     | Password     | Hourly Rate |
|-----------------------|-------------------------|---------------------------|--------------|-------------|
| HR (Admin)            | HR Administrator        | hr@cmcs.ac.za             | Hr@2025      | N/A         |
| Lecturer              | John Smith              | john@cmcs.ac.za           | Lect123!     | R 550.00    |
| Lecturer              | Sarah Williams          | sarah@cmcs.ac.za          | Sarah456!    | R 620.00    |
| Coordinator           | Michael Brown           | coord@cmcs.ac.za          | Coord789!    | N/A         |
| Academic Manager      | Dr. Amanda Johnson      | manager@cmcs.ac.za        | Manager101!  | N/A         |

### API Documentation
Run the app → navigate to:  
**https://localhost:7xxxx/swagger**

### Live Demonstration Video
Link: https://youtu.be/JBa4RqtwSPQ 

### Setup Instructions
1. Open in Visual Studio 2022+
2. Run → database auto-creates via migrations
3. HR account auto-seeded on first run
4. Login as `hr@cmcs.ac.za` / `Hr@2025`
5. Create other users via HR dashboard
