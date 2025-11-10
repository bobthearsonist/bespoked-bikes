// API client wrapper using OpenAPI-generated client
import { apiService } from './generatedClient';

export const api = {
  async getCustomers() {
    return apiService.searchCustomers({});
  },

  async getEmployees() {
    return apiService.listEmployees({});
  },

  async getProducts() {
    return apiService.listProducts();
  },

  async createSale(saleData) {
    return apiService.createSale({ requestBody: saleData });
  },

  async getProductById(id) {
    return apiService.getProductById({ id });
  },
};
