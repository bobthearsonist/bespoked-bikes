# BeSpoked Bikes - Web Frontend

Extremely basic React frontend for the BeSpoked Bikes sales management system. This MVP validates the core functionality: creating a sale with commission calculation and inventory tracking.

## Features

- **Create Sales**: Form to create new sales with customer, salesperson, and product selection
- **Commission Calculation**: Real-time commission calculation based on product cost, sale price, and commission percentage
- **Inventory Tracking**: Integrated with backend API for product inventory management

## Technology Stack

- **React 19** with JSX
- **Vite** for fast development and building
- **Jest** and React Testing Library for testing
- **OpenAPI TypeScript Codegen** for generating type-safe API client from OpenAPI specification

## Getting Started

### Prerequisites

- Node.js 18 or higher
- Backend API running on `http://localhost:5000`

### Installation

```bash
npm install
```

### Development

Start the development server:

```bash
npm run dev
```

The app will be available at `http://localhost:5173` (or another port if 5173 is in use).

### Building

Build for production:

```bash
npm run build
```

The built files will be in the `dist` directory.

### Testing

Run the test suite:

```bash
npm test
```

Run tests in watch mode:

```bash
npm test -- --watch
```

### Linting

```bash
npm run lint
```

### Generating API Client

The API client is automatically generated from the OpenAPI specification (`openapi.yaml`). To regenerate after API changes:

```bash
npm run generate:api
```

This uses `openapi-typescript-codegen` to generate TypeScript types and services in `src/generated/`. The generated code is gitignored and should be regenerated locally or in CI/CD.

## Usage

1. Ensure the backend API is running at `http://localhost:5000`
2. Start the frontend with `npm run dev`
3. Open your browser to `http://localhost:5173`
4. Fill out the sale form:
   - Select a customer
   - Select a salesperson
   - Select a product (retail price, cost price, and commission rate will be displayed)
   - Enter a sale price (commission calculation will be shown)
   - Select sale date
5. Click "Create Sale" to submit

The commission is automatically calculated based on:
```
Commission = (Sale Price - Cost Price) × Commission Percentage
```

## API Integration

The frontend uses an **auto-generated API client** from the OpenAPI specification. The client is generated using `openapi-typescript-codegen` and provides:

- Type-safe API calls with TypeScript definitions
- Automatic request/response handling
- Error handling with typed error responses
- All endpoints defined in `openapi.yaml`

Key endpoints used:
- `GET /api/customers` - Fetch all customers
- `GET /api/employees` - Fetch all employees
- `GET /api/products` - Fetch all products
- `POST /api/sales` - Create a new sale

The base URL is configurable via the `VITE_API_URL` environment variable (defaults to `http://localhost:5000`).

## Project Structure

```
src/
├── api/
│   ├── client.js           # API client wrapper (uses generated client)
│   └── generatedClient.js  # Configuration for generated API client
├── generated/              # Auto-generated from openapi.yaml (gitignored)
│   ├── index.ts
│   ├── core/              # Core HTTP request handling
│   ├── models/            # TypeScript type definitions for DTOs
│   └── services/          # API service methods
├── components/
│   ├── CreateSaleForm.jsx # Main sale creation form
│   └── CreateSaleForm.test.jsx # Component tests
├── App.jsx                # Main app component
├── App.test.jsx           # App tests
├── App.css                # Styles
└── main.jsx               # Entry point
```

## MVP Scope

This is an extremely basic frontend focused solely on validating the MVP requirement: creating a sale with commission calculation and inventory tracking. It does not include:

- User authentication
- Sale history viewing
- Customer/employee/product management
- Reports and analytics
- Advanced UI features

These features can be added in future iterations based on requirements.
