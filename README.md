# Starstay

Starstay is an alien-inspired lodging marketplace built with ASP.NET Core Razor Pages. Think of it as an interstellar Airbnb for starship suites, orbiting villas, bioluminescent habitats, and luxury alien stays.

![Starstay Zeta Reticulan Banner]([docs/images/starstay-zeta-reticulan-banner.png](https://github.com/gavogt/Xeno.Starstay/blob/master/Xeno.Starstay/docs/images/starstay-zeta-reticulan-banner.png?raw=true))

## What It Does

- Public traveler marketplace with live starship listings
- Account registration and login with ASP.NET Core Identity
- Session-backed signed-in greeting and logout flow
- Host console for creating and managing starship stays
- Listing details for atmosphere, gravity, oxygen, silicon-lifeform support, alien pets, and other themed amenities
- Real booking flow with arrival/departure dates and total cost calculation
- Booking conflict protection so overlapping reservations cannot be created
- Host safeguards that prevent deleting a listing once active traveler bookings exist
- Neon alien visual theme designed around the Starstay brand

## Tech Stack

- ASP.NET Core Razor Pages
- .NET 10
- ASP.NET Core Identity
- Entity Framework Core
- SQL Server / SQL Server Express
- Custom CSS theme with a neon sci-fi visual direction

## Run It Locally

### 1. Configure the connection string

This repo keeps the real local configuration out of source control.

- Use `changeme_appsettings.json` as the template
- Create a local `appsettings.json`
- Update `ConnectionStrings:AuthConnection` to point to your SQL Server instance

Example:

```json
{
  "ConnectionStrings": {
    "AuthConnection": "Server=YOURMACHINE\\SQLEXPRESS;Database=StarstayAuthDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### 2. Restore and build

```bash
dotnet restore
dotnet build
```

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. Run the site

```bash
dotnet run
```

## Main User Flows

### Travelers

- Browse all live listings from the public `Starports` page
- View starship stay details without logging in
- Sign in to reserve a stay for a specific date range
- See upcoming bookings and reservation windows

### Hosts

- Sign in and open the `Add Starship` host console
- Create a listing with image, location, price, summary, and alien habitat details
- Review all hosted listings in one place
- Delete listings only when no traveler bookings exist

## Database Notes

The app uses EF Core migrations for schema management. Current schema areas include:

- ASP.NET Identity tables
- `StarshipListings`
- `StarshipBookings`

## Project Structure

```text
Data/          EF Core DbContext
Migrations/    Entity Framework Core migrations
Models/        Identity, listings, and booking models
Pages/         Razor Pages for auth, hosting, browsing, and booking
wwwroot/       CSS, images, and static assets
```

## Current Experience

Starstay currently supports:

- traveler sign-up and login
- public listing discovery
- host-created inventory
- booking by travel window
- themed alien marketplace presentation

## Next Good Expansions

- traveler booking history and cancellation
- host-side booking management
- listing details pages with image galleries
- availability calendars
- search and filtering by species compatibility
- payments and checkout

## Configuration Template

The repository includes `changeme_appsettings.json` for GitHub-safe configuration sharing. The real `appsettings.json` stays local and is intentionally ignored by git.
