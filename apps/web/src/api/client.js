const API_BASE_URL = 'http://localhost:5000/api';

export const api = {
  async getCustomers() {
    const response = await fetch(`${API_BASE_URL}/customers`);
    if (!response.ok) throw new Error('Failed to fetch customers');
    return response.json();
  },

  async getEmployees() {
    const response = await fetch(`${API_BASE_URL}/employees`);
    if (!response.ok) throw new Error('Failed to fetch employees');
    return response.json();
  },

  async getProducts() {
    const response = await fetch(`${API_BASE_URL}/products`);
    if (!response.ok) throw new Error('Failed to fetch products');
    return response.json();
  },

  async createSale(saleData) {
    const response = await fetch(`${API_BASE_URL}/sales`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(saleData),
    });
    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to create sale');
    }
    return response.json();
  },

  async getProductById(id) {
    const response = await fetch(`${API_BASE_URL}/products/${id}`);
    if (!response.ok) throw new Error('Failed to fetch product');
    return response.json();
  },
};
