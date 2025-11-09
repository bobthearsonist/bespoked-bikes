import { useState, useEffect } from 'react';
import { api } from '../api/client';

function CreateSaleForm() {
  const [customers, setCustomers] = useState([]);
  const [employees, setEmployees] = useState([]);
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);
  
  const [formData, setFormData] = useState({
    customerId: '',
    soldByEmployeeId: '',
    productId: '',
    salePrice: '',
    location: 'STORE',
    saleDate: new Date().toISOString().slice(0, 16),
  });

  const [selectedProduct, setSelectedProduct] = useState(null);
  const [calculatedCommission, setCalculatedCommission] = useState(0);

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    if (formData.productId && formData.salePrice) {
      const product = products.find(p => p.id === formData.productId);
      if (product) {
        setSelectedProduct(product);
        const profit = parseFloat(formData.salePrice) - product.costPrice;
        const commission = profit * (product.commissionPercentage / 100);
        setCalculatedCommission(commission);
      }
    } else {
      setSelectedProduct(null);
      setCalculatedCommission(0);
    }
  }, [formData.productId, formData.salePrice, products]);

  const loadData = async () => {
    try {
      setLoading(true);
      const [customersData, employeesData, productsData] = await Promise.all([
        api.getCustomers(),
        api.getEmployees(),
        api.getProducts(),
      ]);
      setCustomers(customersData);
      setEmployees(employeesData);
      setProducts(productsData);
      setError(null);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setSuccess(null);

    try {
      const saleData = {
        ...formData,
        salePrice: parseFloat(formData.salePrice),
      };
      const result = await api.createSale(saleData);
      setSuccess(`Sale created successfully! Commission: $${result.commissionAmount.toFixed(2)}`);
      
      // Reset form
      setFormData({
        customerId: '',
        soldByEmployeeId: '',
        productId: '',
        salePrice: '',
        location: 'STORE',
        saleDate: new Date().toISOString().slice(0, 16),
      });
      setSelectedProduct(null);
      setCalculatedCommission(0);
    } catch (err) {
      setError(err.message);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value,
    }));
  };

  if (loading) {
    return <div className="loading">Loading...</div>;
  }

  return (
    <div className="create-sale-form">
      <h2>Create Sale</h2>
      
      {error && <div className="error-message" role="alert">{error}</div>}
      {success && <div className="success-message" role="status">{success}</div>}

      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="customerId">Customer *</label>
          <select
            id="customerId"
            name="customerId"
            value={formData.customerId}
            onChange={handleChange}
            required
          >
            <option value="">Select a customer</option>
            {customers.map(customer => (
              <option key={customer.id} value={customer.id}>
                {customer.name}
              </option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="soldByEmployeeId">Salesperson *</label>
          <select
            id="soldByEmployeeId"
            name="soldByEmployeeId"
            value={formData.soldByEmployeeId}
            onChange={handleChange}
            required
          >
            <option value="">Select a salesperson</option>
            {employees.map(employee => (
              <option key={employee.id} value={employee.id}>
                {employee.name}
              </option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="productId">Product *</label>
          <select
            id="productId"
            name="productId"
            value={formData.productId}
            onChange={handleChange}
            required
          >
            <option value="">Select a product</option>
            {products.map(product => (
              <option key={product.id} value={product.id}>
                {product.name} - ${product.retailPrice.toFixed(2)}
              </option>
            ))}
          </select>
        </div>

        {selectedProduct && (
          <div className="product-info">
            <p><strong>Retail Price:</strong> ${selectedProduct.retailPrice.toFixed(2)}</p>
            <p><strong>Cost Price:</strong> ${selectedProduct.costPrice.toFixed(2)}</p>
            <p><strong>Commission Rate:</strong> {selectedProduct.commissionPercentage}%</p>
          </div>
        )}

        <div className="form-group">
          <label htmlFor="salePrice">Sale Price *</label>
          <input
            type="number"
            id="salePrice"
            name="salePrice"
            value={formData.salePrice}
            onChange={handleChange}
            step="0.01"
            min="0"
            required
          />
        </div>

        {calculatedCommission > 0 && (
          <div className="commission-preview">
            <p><strong>Estimated Commission:</strong> ${calculatedCommission.toFixed(2)}</p>
          </div>
        )}

        <div className="form-group">
          <label htmlFor="saleDate">Sale Date *</label>
          <input
            type="datetime-local"
            id="saleDate"
            name="saleDate"
            value={formData.saleDate}
            onChange={handleChange}
            required
          />
        </div>

        <button type="submit" className="submit-button">
          Create Sale
        </button>
      </form>
    </div>
  );
}

export default CreateSaleForm;
