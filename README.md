# FindYOU

FindYOU is a personal knowledge manager built with ASP.NET Core MVC. I created it to solve a real problem I faced while learning programming and preparing for interviews: useful ChatGPT conversations were getting lost across many shared links, notes, and topics.

The app stores important chat links with a title, category, summary, and notes so they can be searched, filtered, and opened again when needed.

## Why I Built This

While studying topics like .NET, C#, operating systems, networking, Linux, and interview preparation, I often had long and valuable conversations with AI tools. Finding those conversations later became difficult.

FindYOU organizes those links into one simple web application, making it easier to keep learning material structured and reusable.

## Features

- Create, read, update, and delete chat entries
- Save chat links with summaries and personal notes
- Categorize conversations by topic
- Search chat entries by title
- Filter entries by category
- View structured chat-entry details
- Session-based authentication
- PostgreSQL database storage

## Tech Stack

- ASP.NET Core MVC
- .NET 9
- C#
- Entity Framework Core
- PostgreSQL
- NeonDB
- HTML
- CSS
- Bootstrap
- Repository pattern
- Dependency injection
- Session management

## Main Screens

- Home page
- Login and registration
- Category management
- Chat entry management
- Chat entry detail modal
- Chat entry create and update forms

## Project Structure

```text
Controllers/        MVC controllers
Data/               Entity Framework database context
Implementation/     Repository implementations
Interfaces/         Repository contracts
Models/             Application models
Views/              Razor views
wwwroot/            CSS, JavaScript, and static files
Migrations/         Entity Framework migrations
```

## Getting Started

### Prerequisites

- .NET 9 SDK
- PostgreSQL database

### Setup

1. Clone the repository.

```bash
git clone https://github.com/your-username/FindYOU.git
cd FindYOU
```

2. Configure the database connection.

Do not commit real database passwords to GitHub. Use user secrets, environment variables, or a local `appsettings.Development.json` file for your connection string.

Connection string key:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=findyou;Username=postgres;Password=your-password"
  }
}
```

3. Restore packages.

```bash
dotnet restore
```

4. Apply database migrations.

```bash
dotnet ef database update
```

5. Run the project.

```bash
dotnet run
```

Open the local URL shown in the terminal.

## Learning Outcome

This project helped me practice MVC architecture, database design, Entity Framework migrations, repository pattern, dependency injection, session management, and cloud-hosted PostgreSQL databases.

## Notes

FindYOU is a personal-use project, but it can be extended into a larger knowledge-management tool with better sharing, tags, rich summaries, and AI-assisted organization.
