// Wrapper for the generated OpenAPI client
import { OpenAPI, DefaultService } from '../generated';

// Configure the OpenAPI client with environment-specific base URL
const baseUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000';
OpenAPI.BASE = baseUrl;
OpenAPI.WITH_CREDENTIALS = false;
OpenAPI.CREDENTIALS = 'same-origin';

// Export the service for direct use
export const apiService = DefaultService;
