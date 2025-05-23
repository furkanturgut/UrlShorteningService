
# URL Shortening Service

A full-stack web application for shortening URLs, built with ASP.NET Core backend and HTML/CSS/JavaScript frontend.

## Overview

This URL shortening service allows users to:
- Create shortened URLs from long URLs
- Create custom aliases for URLs
- Track and manage shortened URLs
- Redirect from shortened URLs to original destinations

## Features

- **URL Shortening**: Convert long URLs into short, manageable links
- **Custom Aliases**: Create custom short URLs instead of using auto-generated ones
- **URL Management**: View, copy, and delete your shortened URLs
- **API**: RESTful API for URL shortening operations
- **Responsive Design**: Works on desktop and mobile devices

## Tech Stack

### Backend
- **Framework**: ASP.NET Core
- **Database**: MySQL
- **Architecture**: MVC pattern with Repository and Service layers
- **API**: RESTful API endpoints

### Frontend
- **HTML/CSS/JavaScript**: Vanilla front-end implementation
- **Responsive Design**: Mobile-friendly interface
- **FontAwesome**: Icons for better user experience

## Project Structure

```
UrlShorteningService/
├── backend/
│   ├── Controllers/        # API controllers
│   ├── Data/               # Database context
│   ├── Dtos/               # Data Transfer Objects
│   ├── Interfaces/         # Service and repository interfaces
│   ├── Models/             # Domain models
│   ├── Repositories/       # Data access layer
│   ├── Services/           # Business logic layer
│   ├── Program.cs          # Application entry point
│   └── appsettings.json    # Configuration settings
└── frontend/
    ├── index.html          # Main HTML page
    ├── styles.css          # CSS styles
    └── app.js              # JavaScript functionality
```

## Getting Started

### Prerequisites

- .NET SDK 6.0 or later
- MySQL Server
- Node.js and npm (optional, for development)

### Backend Setup

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/UrlShorteningService.git
   cd UrlShorteningService/backend
   ```

2. Update the database connection string in appsettings.json:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "server=localhost;user=root;password=yourpassword;database=urlshortener"
     }
   }
   ```

3. Run database migrations:
   ```
   dotnet ef database update
   ```

4. Start the backend server:
   ```
   dotnet run
   ```
   The API will be available at http://localhost:5024

### Frontend Setup

1. Navigate to the frontend directory:
   ```
   cd ../frontend
   ```

2. Open `app.js` and update the API URL if needed:
   ```javascript
   const API_BASE_URL = 'http://localhost:5024';
   const REDIRECT_BASE_URL = 'http://localhost:5024';
   ```

3. Serve the frontend files:
   - Using Visual Studio Code's Live Server extension
   - Or any other static file server

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/url/create` | Create a shortened URL |
| GET | `/{shortUrl}` | Redirect to original URL |
| GET | `/url/all` | Get all URLs |
| DELETE | `/url/delete/{id}` | Delete a URL by ID |

### Request Examples

#### Create URL
```http
POST /url/create
Content-Type: application/json

{
  "originalUrl": "https://example.com/very/long/url/that/needs/shortening",
  "customAlias": "mylink"  // Optional
}
```

## Contributors

- Furkan Turgut

---

Feel free to contribute to this project by submitting issues or pull requests!
