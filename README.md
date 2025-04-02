# UserManagementSystem
This repository contains a basic **User Management System** with:
- **Backend** in **ASP.NET Core 7 (C#)** using **MongoDB** for data storage
- **Frontend** in **React + TypeScript**
- **Authentication** via **JWT**
- **Password hashing** via **BCrypt**
- **Unit & Integration tests** for the Backend

The system enables:
- **User Registration** (`POST /api/auth/register`) – sending `{ fullName, email, password }`.
- **User Login** (`POST /api/auth/login`) – sending `{ email, password }`; returns `{ token }`.
- **User Info** (`GET /api/auth/me`) – using **JWT Bearer Token** in the `Authorization` header.

## Backend Setup
### Prerequisites
- [.NET 7 SDK]
- [MongoDB](https://www.mongodb.com/) running locally 

### Installation & Run
# 1. Clone the repository
git clone https://github.com/oripeles/UserManagementSystem.git

# 2. Navigate to the backend folder
cd UserManagementSystem/backend

# 3. Restore dependencies
dotnet restore

# 4. Run the server
dotnet run

## Frontend Setup
### Prerequisites
- [Node.js]
- npm 

### Installation & Run
# 1. Navigate to the frontend folder
cd ../frontend

# 2. Install dependencies
npm install

# 3. Start the development server
npm start