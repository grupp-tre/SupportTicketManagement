# Support Ticket Management - Grupp 3

A Windows desktop application for managing customers and support tickets.
Built with C#, .NET 10 and WinUI

## Group and responsibilities

| Member | Main responsibility |
| ------ | ------------------- |
| Meron | Customer management |
| Oscar | Ticket registration |
| Jack | Ticket details, assignment, status changes and comments |
| Alejandro | Ticket overview, search, status filtering and related tests |

## Features and requirements checklist

- [x] Register customers and support tickets.
- [x] View ticket details and add comments.
- [x] Update ticket status and assignment.
- [x] List tickets with the newest tickets first.
- [x] Search by full or partial ticket titles and customer names,
      regardless of letter casing.
- [x] Filter tickets by status and combine filtering with search.
- [x] View counts of new, in-progress and solved tickets.
- [x] Save application data locally in JSON files.
- [x] Validate required fields and enforce assignment rules.
- [x] Navigate between the main views, ticket details and back.
- [x] Pass the required unit and integration tests.

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
6. Build the solution using Build > Build solution
7. Start the application using F5.

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

- Views bind to view models, which call application services.
- Services coordinate domain logic and use repository interfaces.
- Infrastructure implements these interfaces to read and write JSON files.
- Domain models contain the data and business rules.

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

### Using your own sample data

Copy the sample administrators as described in Initial setup.
Then use the application to register customers and create tickets linked to them. 
The data is saved in the folder listed above.

## Run the tests

Open a terminal in the folder containing
`SupportTicketManagement.slnx`.

Run all tests:

```powershell
dotnet test .\SupportTicketManagement.Tests\SupportTicketManagement.Tests.csproj
```

Run tests tagged as Unit:

```powershell
dotnet test .\SupportTicketManagement.Tests\SupportTicketManagement.Tests.csproj --filter "Category=Unit"
```

Run only integration tests:

```powershell
dotnet test .\SupportTicketManagement.Tests\SupportTicketManagement.Tests.csproj --filter "Category=Integration"
```

The test project uses xUnit and NSubstitute.

## Manual verification

The following checks passed using test customers and test tickets:

- [x] Register a customer and create a linked ticket.
- [x] Open details, assign an administrator, update status and save.
- [x] Add comments with correct text and timestamps.
- [x] Verify newest-first sorting, search and combined status filtering.
- [x] Verify the empty-result message and updated Overview counts.
- [x] Restart the application and confirm that data and links remain.

## Known limitations

The application is designed for one running instance on a shared Windows computer. 
Concurrent access from multiple instances is not supported.

No known functional issues were identified during the manual checks. 

## AI Usage

AI assistance was used for explanations, code suggestions and troubleshooting.
Suggestions were reviewed and checked through builds, manual testing and automated tests.