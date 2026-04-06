# PharmaTrack

I built this project as a backend system for pharmacy management. The core problem it solves is that pharmacies constantly lose money on near-expiry medications — they either trash them or sell them at a loss with no system in place. So I built something that monitors inventory automatically, sends alerts before it's too late, and provides an internal marketplace where pharmacies can sell their surplus stock to other pharmacies instead of wasting it.

The project is built on **Microservices Architecture** with ASP.NET Core 10, where each service has one responsibility and one responsibility only.

---

## What does the system do?

- A pharmacy registers itself and adds its inventory (manually or via QR scan)
- A background worker runs every 24 hours and picks up all medications expiring within 90 days
- It sends the pharmacist a real-time alert on the dashboard and an email notification
- The pharmacist can list that medication on the Marketplace with an automatic 20% discount
- Another pharmacy that needs it can place a request, and the system matches them together

---

## Services

The project has 7 services that communicate through an API Gateway:

**API Gateway** — the single entry point for the entire system. It validates the JWT token before any request reaches the downstream services, and applies rate limiting to protect against abuse.

**Identity Service** — handles registration, login, and JWT token issuance. Passwords are stored with BCrypt, tokens expire after 60 minutes, and there's a refresh token mechanism to keep sessions alive.

**Pharmacy Service** — manages pharmacy profiles and branches. Every new pharmacy starts with a `PendingApproval` status and only a SuperAdmin can activate it.

**Inventory Service** — full inventory management. Each product can have multiple batches, and every batch gets a QR code generated automatically when added.

**Notification Service** — sends alerts across 4 channels: dashboard via SignalR, email via SendGrid, SMS via Twilio, and push notifications via Firebase. Every notification is logged to the database with its status (sent/failed).

**Marketplace Service** — one pharmacy lists a medication, another requests it, and the system matches them. The discount is always 20%, applied automatically on every listing.

**Expiry Tracker Worker** — not an API, this is a background service that runs every 24 hours. It reads directly from the Inventory database and calls the Notification Service to fire the alerts.

---

## Tech Stack

- **ASP.NET Core 10 / .NET 10**
- **PostgreSQL** — single database instance with separate schemas per service
- **Entity Framework Core** with Npgsql
- **CQRS + MediatR** — every operation is either a Command or a Query
- **FluentValidation** — validation runs in the pipeline before reaching the handler
- **YARP** — the Reverse Proxy powering the Gateway
- **SignalR** — real-time notifications
- **Serilog** — structured logging
- **Docker + Docker Compose** — run everything with a single command
- **QRCoder** — QR code generation
- **SendGrid / Twilio / Firebase** — notification channels

---

## Project Structure

```
PharmaTrack/
├── .env                          # Environment variables — never commit this
├── docker-compose.yml
└── services/
    ├── ApiGateway/
    ├── IdentityService/
    ├── PharmacyService/
    ├── InventoryService/
    ├── NotificationService/
    ├── MarketplaceService/
    └── ExpiryTrackerService/
```

Each service follows the same internal structure:
```
ServiceName/
├── ServiceName.API/           → Controllers, Middleware, Program.cs
├── ServiceName.Application/   → Commands, Queries, DTOs, Interfaces
├── ServiceName.Domain/        → Entities, Enums, Exceptions
└── ServiceName.Infrastructure/→ DbContext, Repositories, External services
```

---

## Running the Project

### Requirements
- Docker Desktop

### Steps

**1. Clone the repo**
```bash
git clone https://github.com/islamallam962-stack/pharmatrack.git
cd pharmatrack
```

**2. Set up your `.env` file**

Copy the example file and fill in your values:
```bash
cp .env.example .env
```

Open `.env` and update at least these:
```env
POSTGRES_PASSWORD=pick_a_strong_password
JWT_SECRET=any_random_string_at_least_32_chars
SENDGRID_API_KEY=your_sendgrid_key
```

**3. Start everything**
```bash
docker-compose up --build
```

Let it finish. It will build all services, spin them up, and the database schemas get created automatically on first run.

**4. Try it out**

| Service | URL |
|---|---|
| API Gateway (all requests go here) | http://localhost:5000 |
| Identity Service | http://localhost:5001/scalar |
| Pharmacy Service | http://localhost:5002/scalar |
| Inventory Service | http://localhost:5003/scalar |
| Notification Service | http://localhost:5004/scalar |
| Marketplace Service | http://localhost:5005/scalar |

---

## Running Without Docker (for development)

If you want to run a single service on its own:

```bash
cd services/IdentityService/IdentityService.API
dotnet run
```

You'll need PostgreSQL running locally on port 5432 first.

---

## Authentication

Every request (except register and login) requires a JWT token.

**1. Register a user**
```
POST http://localhost:5000/api/auth/register
{
  "fullName": "Your Name",
  "email": "email@example.com",
  "password": "Password123",
  "role": 2
}
```

Available roles:
- `1` = SuperAdmin
- `2` = PharmacyAdmin
- `3` = Pharmacist

**2. Login and get your token**
```
POST http://localhost:5000/api/auth/login
```

You'll get back an `accessToken` — attach it to every request:
```
Authorization: Bearer <accessToken>
```

Tokens expire after 60 minutes. Refresh them with:
```
POST http://localhost:5000/api/auth/refresh
```

---

## Core Endpoints

All requests go through the Gateway at `http://localhost:5000`

### Pharmacies
```
POST   /api/pharmacies              → Register a pharmacy
GET    /api/pharmacies/{id}         → Get pharmacy details
GET    /api/pharmacies              → List all pharmacies (SuperAdmin only)
PUT    /api/pharmacies/{id}         → Update pharmacy info
PATCH  /api/pharmacies/{id}/status  → Activate or suspend (SuperAdmin only)
```

### Inventory
```
POST   /api/inventory/manual              → Add a product manually
POST   /api/inventory/scan                → Look up a product by QR
GET    /api/inventory/{pharmacyId}        → Get full inventory for a pharmacy
PATCH  /api/inventory/batch/{id}/stock    → Update batch quantity
DELETE /api/inventory/product/{id}        → Delete a product
```

### Marketplace
```
POST  /api/marketplace/listings             → List a medication for sale
GET   /api/marketplace/listings             → Browse available listings
POST  /api/marketplace/requests             → Request a medication
POST  /api/marketplace/match                → Match a listing to a request
```

### Real-time Notifications
The frontend connects to:
```
ws://localhost:5000/hubs/notifications?access_token=<JWT>
```

Then joins the pharmacy's group:
```js
connection.invoke("JoinPharmacyGroup", pharmacyId)
```

And listens for alerts on the `ReceiveAlert` event.

---

## Important Notes

**QR Code scanning:** The scan endpoint expects the already-decoded string from the QR, not the image itself. Decoding happens on the mobile/frontend side. The format is:
```
PHARMA|{productId}|{batchId}|{batchNumber}
```

**New pharmacies:** Any newly registered pharmacy starts with `PendingApproval` status. A SuperAdmin needs to activate it before it can be used:
```
PATCH /api/pharmacies/{id}/status
body: "activate"
```

**Expiry Tracker:** Runs once immediately when Docker starts, then every 24 hours after that. It checks all batches expiring within 90 days and sends one alert per batch per day — no duplicates.

---

## Environment Variables

| Variable | Description |
|---|---|
| `POSTGRES_DB` | Database name |
| `POSTGRES_USER` | Database user |
| `POSTGRES_PASSWORD` | Database password |
| `JWT_SECRET` | JWT signing key (minimum 32 characters) |
| `JWT_ISSUER` | Token issuer (PharmaTrack) |
| `JWT_AUDIENCE` | Token audience (PharmaTrackClients) |
| `SENDGRID_API_KEY` | SendGrid API key for emails |
| `TWILIO_ACCOUNT_SID` | Twilio account SID for SMS |
| `TWILIO_AUTH_TOKEN` | Twilio auth token |
| `TWILIO_FROM_PHONE` | Twilio sender phone number |
| `FIREBASE_CREDENTIAL_PATH` | Path to Firebase credentials file |
