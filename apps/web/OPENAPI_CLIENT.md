# OpenAPI Client Generation

This frontend uses an auto-generated API client from the OpenAPI specification, similar to how the backend uses NSwag to generate controllers and DTOs.

## Why Generate the Client?

1. **Single Source of Truth**: The `openapi.yaml` file defines the API contract
2. **Type Safety**: Generated TypeScript types catch errors at compile time
3. **Consistency**: Frontend and backend stay in sync with the API spec
4. **Reduced Maintenance**: No manual API client code to maintain
5. **Auto-completion**: IDEs provide better autocomplete and inline documentation

## How It Works

### Generation Tool

We use [`openapi-typescript-codegen`](https://github.com/ferdikoomen/openapi-typescript-codegen) which:
- Reads the OpenAPI 3.0 specification from `openapi.yaml`
- Generates TypeScript types for all DTOs (models)
- Generates service classes with methods for each API endpoint
- Includes proper error handling and type checking

### Generated Structure

```
src/generated/
├── index.ts              # Main exports
├── core/                 # HTTP request handling
│   ├── OpenAPI.ts       # Configuration
│   ├── request.ts       # Core request logic
│   └── ...
├── models/              # TypeScript types for DTOs
│   ├── CustomerDto.ts
│   ├── EmployeeDto.ts
│   ├── ProductDto.ts
│   ├── SaleDto.ts
│   └── ...
└── services/            # API service methods
    └── DefaultService.ts # All API endpoints
```

### Usage in Code

**Before (Manual):**
```javascript
// Manual fetch with no type safety
const response = await fetch(`${API_BASE_URL}/customers`);
const customers = await response.json(); // any type
```

**After (Generated):**
```javascript
// Type-safe generated client
import { DefaultService } from '../generated';

// Returns Promise<CustomerDto[]> with full type checking
const customers = await DefaultService.searchCustomers({});
```

## Regenerating the Client

### When to Regenerate

Regenerate the API client whenever:
- The `openapi.yaml` specification is updated
- New endpoints are added
- Request/response schemas change
- After pulling changes that modify the API

### How to Regenerate

```bash
npm run generate:api
```

This command runs:
```bash
openapi-typescript-codegen --input ../../openapi.yaml --output ./src/generated --client fetch
```

### CI/CD Integration

The generated files are gitignored, so they must be regenerated:

1. **Locally**: Run `npm run generate:api` after `npm install`
2. **CI/CD**: Add to build script:
   ```json
   "prebuild": "npm run generate:api"
   ```

## Configuration

### Base URL

The API base URL is configured in `src/api/generatedClient.js`:

```javascript
import { OpenAPI } from '../generated';

// Configure from environment variable
OpenAPI.BASE = import.meta.env.VITE_API_URL || 'http://localhost:5000';
```

### Client Options

The generated client supports various options via the `OpenAPI` config object:
- `BASE`: Base URL for API
- `WITH_CREDENTIALS`: Include credentials in requests
- `TOKEN`: Authentication token
- `HEADERS`: Custom headers

## Wrapper Pattern

We use a thin wrapper (`src/api/client.js`) around the generated client:

```javascript
import { apiService } from './generatedClient';

export const api = {
  async getCustomers() {
    return apiService.searchCustomers({});
  },
  // ... other methods
};
```

This provides:
- Consistent interface for components
- Ability to add custom logic (caching, transforms, etc.)
- Easier to mock in tests

## Benefits Observed

1. **Caught Type Errors**: TypeScript catches mismatched property names and types
2. **Better IDE Support**: Autocomplete for all API methods and DTOs
3. **Documentation**: JSDoc comments generated from OpenAPI descriptions
4. **Maintainability**: Changes to API spec automatically reflected in client
5. **Consistency**: Matches backend exactly (both generated from same spec)

## Comparison with Backend

| Backend (C#) | Frontend (TypeScript) |
|-------------|---------------------|
| NSwag | openapi-typescript-codegen |
| Generates Controllers + DTOs | Generates Services + Models |
| Uses `nswag.json` config | Uses npm script |
| Output: `BespokedBikes.Application/Generated/` | Output: `src/generated/` |

Both sides generate from the same `openapi.yaml`, ensuring frontend and backend stay synchronized.
