# Visma Resource Shortage Management Console Application

This console application is designed to help manage resource shortages within an office environment. Users can register new shortages, list existing ones with various filters, and delete requests. The application was developed as part of an internship task for Visma.

## Features

* **Register New Shortage:** Users can add new shortage requests, providing details such as:
    * Title
    * Reporter's Name
    * Room (Meeting room, kitchen, bathroom)
    * Category (Electronics, Food, Other)
    * Priority (1-10)
* **Duplicate Handling:** If a shortage with the same title and room already exists:
    * A new request is not created if the new priority is not higher.
    * The existing request is overridden if the new priority is higher.
* **List Shortages:**
    * Administrators can see all requests.
    * Regular users can only see requests they created.
    * Results are always listed with higher priority items at the top.
    * **Filtering Options:**
        * Filter by Title (partial match, case-insensitive)
        * Filter by CreatedOn date range
        * Filter by Category
        * Filter by Room
* **Delete Shortage:**
    * Only the person who created the shortage or an administrator can delete a request.
* **Data Persistence:** All shortage information is stored in a `shortages.json` file in a `data` subfolder relative to the executable, allowing data to be retained between application restarts.
* **User Roles:** Simple role system where a user named "admin" (case-insensitive) has administrator privileges.

## Technical Details

* **Language:** C#
* **Framework:** .NET 9 (Please adjust if you used a different version, e.g., .NET 8)
* **Design:** Follows OOP principles with a focus on separation of concerns (UI, Services, Models).
* **Testing:** Includes unit tests for core service logic, user context, and input validation.

## Requirements

* .NET 9.0 SDK (or the .NET SDK version your project targets) to build and run.
* .NET 9.0 Runtime (or corresponding runtime) if running the framework-dependent published version.

## How to Use

### 1. Clone the Repository

```bash
git clone <https://github.com/EdwinKeizik/VismaResourceShortageManagement>
cd VismaResourceShortageManagement
```

### 2. Build the Solution
Navigate to the directory containing the .sln file. Based on your structure, this is the VismaResourceShortageManagement subfolder.

```bash
cd VismaResourceShortageManagement
dotnet build VismaResourceShortageManagement.sln
```