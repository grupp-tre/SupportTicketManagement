# Support Ticket Management - Grupp 3

A Windows desktop application for managing customers and support tickets.
Built with C#, .NET 10 and WinUI

## Features

- Register customers and support tickets.
- View ticket details and add comments.
- Update ticket status and assignment.
- List tickets with the newest tickets first.
- Search by full or partial ticket titles and customer names, regardless of letter casing.
- Filter tickets by status and combine filtering with the search.
- View counts of new, in-progress and solved tickets.
- Save application data locally in JSON files.

## Requirements

- Windows
- .NET 10 SDK
- Visual Studio with support for .NET 10 and WinUI development.

## Initial setup

1. Clone the repository.
2. Open `%LOCALAPPDATA%` in File Explorer.
3. Create a folder named `SupportTicketManagement` if it does not exist.
4. Copy `sample-data/admins.json` from the repository into that folder.

The application loads ticket administrators from the local `admins.json`.
Cloning the repository does not automatically copy the sample file
to the application's data folder.

Do not overwrite an existing `admins.json` containing your own data.
The sample file contains fictional administrators for demonstration.

## Run the application

1. Open `SupportTicketManagement.slnx` in Visual Studio.
2. Restore the NuGet packages if they are not restored automatically.
3. Set `SupportTicketManagement.Presentation` as the startup project.
4. Select the appropriate platform for your computer, normally `x64`.
5. Select the `SupportTicketManagement.Presentation (Package)` launch profile.
6. Start the application.

## Using the application

### Customers

Create a customer before registering a ticket for that customer.

### Tickets

Use the search field to find tickets by title or customer name.
Select a status to narrow the results, then apply the search.

To show all tickets again, clear the search field and select `All statuses`, then apply the search.

### Overview

The overview displays the number of tickets with the statuses `New`, `In progress` and `Solved`.

The counts are loaded again when returning to the overview.

## Project structure

| Project | Responsibility |
| --- | --- |
| `SupportTicketManagement.Domain` | Domain models, business rules and repository interfaces. |
| `SupportTicketManagement.Application` | Application services, requests and operation results. |
| `SupportTicketManagement.Infrastructure` | JSON file storage and repository implementations. |
| `SupportTicketManagement.Presentation` | WinUI views, view models and navigation. |
| `SupportTicketManagement.Tests` | Automated unit and integration tests. |

The presentation layer uses MVVM to separate the views from
presentation logic. Dependencies are registered in `App.xaml.cs`.

## Data storage

Application data is stored locally in:

```text
%LOCALAPPDATA%\SupportTicketManagement
```

The application uses these files:

- `customers.json`
- `tickets.json`
- `admins.json`

## Run the tests

Open a terminal in the folder containing
`SupportTicketManagement.slnx`.

Run all tests:

```powershell
dotnet test .\SupportTicketManagement.Tests\SupportTicketManagement.Tests.csproj
```

Run only unit tests:

```powershell
dotnet test .\SupportTicketManagement.Tests\SupportTicketManagement.Tests.csproj --filter "Category=Unit"
```

Run only integration tests:

```powershell
dotnet test .\SupportTicketManagement.Tests\SupportTicketManagement.Tests.csproj --filter "Category=Integration"
```

The test project uses xUnit and NSubstitute.