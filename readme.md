# Media Inventory Manager - Submission Guide

This project is a full-stack media management application with an ASP.NET Core API and a Next.js frontend.

## 🚀 Quick Start

From the **root directory**, follow these steps to install dependencies and start the environment.

### 1. Install All Dependencies

Run these commands to ensure the Client, API, and Tests are ready:

```bash
# Install Frontend dependencies
cd client && npm install && cd ..

# Restore Backend API dependencies
dotnet restore api/api.csproj

# Restore Test dependencies
dotnet restore api.Tests/api.Tests.csproj
```

### 2. Running the Application

The root directory contains an orchestration script to launch both tiers simultaneously. Run:

```bash
npm run dev
```

- Frontend: http://localhost:3000
- Backend API: http://localhost:5001

### 3. Running Tests

To execute the backend unit tests:

```bash
dotnet test api.Tests/api.Tests.csproj
```

### 📂 Project Structure
- api: ASP.NET Core Web API (Controllers, Services, Data)
- api.Tests: XUnit/Moq Backend Test Project
- client: Next.js 15 Frontend (App Router, Tailwind 4, Shadcn UI)
- vention-lab-assignment.sln: Visual Studio Solution file

### 🛠 Tech Stack Details
- Frontend: Next.js, Tailwind CSS 4, Radix UI, React Hook Form, Zod.
- Backend: .NET 8, FluentValidation, IFormFile for image processing.
- Storage: Local file system for image uploads (stored in api/wwwroot/uploads).
