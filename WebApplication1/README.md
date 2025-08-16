# Task Manager Application

A simple task management application built with ASP.NET Core Razor Pages.

## Features

- Create, read, update, and delete tasks
- Mark tasks as completed
- Filter tasks by completion status
- View task details including due dates

## Project Structure

- **Models**
  - `TodoTask.cs`: Task model with properties for title, description, due date, and completion status
  - `TaskService.cs`: Service for managing tasks in memory

- **Pages**
  - `Index.cshtml`: Home page with a welcome message and task statistics
  - `Tasks/Index.cshtml`: Lists all tasks with filtering options
  - `Tasks/Create.cshtml`: Form for creating new tasks
  - `Tasks/Edit.cshtml`: Form for editing existing tasks
  - `Tasks/Delete.cshtml`: Confirmation page for deleting tasks

- **Styling**
  - The application uses a pastel purple and blue color scheme
  - Modern UI with cards, shadows, and responsive design
  - Sidebar navigation for easy access to different sections

## Technologies Used

- ASP.NET Core 8
- Razor Pages
- Bootstrap 5
- Font Awesome icons
- jQuery for AJAX functionality

## How to Use

1. Navigate to the Tasks page using the sidebar
2. Use the filter buttons to view all, completed, or pending tasks
3. Click the checkbox next to a task to mark it as completed
4. Use the action buttons to edit or delete tasks
5. Click "Nueva Tarea" to create a new task