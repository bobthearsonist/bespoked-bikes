import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import CreateSaleForm from './CreateSaleForm';
import { api } from '../api/client';

// Mock the API client
jest.mock('../api/client', () => ({
  api: {
    getCustomers: jest.fn(),
    getEmployees: jest.fn(),
    getProducts: jest.fn(),
    createSale: jest.fn(),
  },
}));

describe('CreateSaleForm', () => {
  const mockCustomers = [
    { id: '1', name: 'John Doe' },
    { id: '2', name: 'Jane Smith' },
  ];

  const mockEmployees = [
    { id: '1', name: 'Alice Johnson' },
    { id: '2', name: 'Bob Williams' },
  ];

  const mockProducts = [
    {
      id: '1',
      name: 'Mountain Bike',
      retailPrice: 1000,
      costPrice: 600,
      commissionPercentage: 10,
    },
    {
      id: '2',
      name: 'Road Bike',
      retailPrice: 1500,
      costPrice: 900,
      commissionPercentage: 15,
    },
  ];

  beforeEach(() => {
    api.getCustomers.mockResolvedValue(mockCustomers);
    api.getEmployees.mockResolvedValue(mockEmployees);
    api.getProducts.mockResolvedValue(mockProducts);
    api.createSale.mockResolvedValue({
      id: '1',
      commissionAmount: 40,
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders the form with all required fields', async () => {
    render(<CreateSaleForm />);

    await waitFor(() => {
      expect(screen.getByRole('heading', { name: 'Create Sale' })).toBeInTheDocument();
    });

    expect(screen.getByLabelText(/customer/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/salesperson/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/product/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/sale price/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/sale date/i)).toBeInTheDocument();
  });

  it('loads customers, employees, and products on mount', async () => {
    render(<CreateSaleForm />);

    await waitFor(() => {
      expect(api.getCustomers).toHaveBeenCalled();
      expect(api.getEmployees).toHaveBeenCalled();
      expect(api.getProducts).toHaveBeenCalled();
    });
  });

  it('displays product information when a product is selected', async () => {
    const user = userEvent.setup();
    render(<CreateSaleForm />);

    await waitFor(() => {
      expect(screen.getByLabelText(/product/i)).toBeInTheDocument();
    });

    const productSelect = screen.getByLabelText(/product/i);
    const salePriceInput = screen.getByLabelText(/sale price/i);
    
    await user.selectOptions(productSelect, '1');
    await user.clear(salePriceInput);
    await user.type(salePriceInput, '1000');

    await waitFor(() => {
      const productInfo = document.querySelector('.product-info');
      expect(productInfo).toBeInTheDocument();
      expect(productInfo.textContent).toContain('Retail Price:');
      expect(productInfo.textContent).toContain('Cost Price:');
      expect(productInfo.textContent).toContain('Commission Rate:');
    });
  });

  it('calculates commission when product and sale price are entered', async () => {
    const user = userEvent.setup();
    render(<CreateSaleForm />);

    await waitFor(() => {
      expect(screen.getByLabelText(/product/i)).toBeInTheDocument();
    });

    const productSelect = screen.getByLabelText(/product/i);
    const salePriceInput = screen.getByLabelText(/sale price/i);

    await user.selectOptions(productSelect, '1');
    await user.clear(salePriceInput);
    await user.type(salePriceInput, '1000');

    await waitFor(() => {
      expect(screen.getByText(/estimated commission/i)).toBeInTheDocument();
      // (1000 - 600) * 0.10 = 40
      expect(screen.getByText(/\$40\.00/)).toBeInTheDocument();
    });
  });

  it('submits the form successfully', async () => {
    const user = userEvent.setup();
    render(<CreateSaleForm />);

    await waitFor(() => {
      expect(screen.getByLabelText(/customer/i)).toBeInTheDocument();
    });

    await user.selectOptions(screen.getByLabelText(/customer/i), '1');
    await user.selectOptions(screen.getByLabelText(/salesperson/i), '1');
    await user.selectOptions(screen.getByLabelText(/product/i), '1');
    await user.clear(screen.getByLabelText(/sale price/i));
    await user.type(screen.getByLabelText(/sale price/i), '1000');

    const submitButton = screen.getByRole('button', { name: /create sale/i });
    await user.click(submitButton);

    await waitFor(() => {
      expect(api.createSale).toHaveBeenCalled();
      expect(screen.getByRole('status')).toBeInTheDocument();
      expect(screen.getByText(/sale created successfully/i)).toBeInTheDocument();
    });
  });

  it('displays error message when API call fails', async () => {
    api.createSale.mockRejectedValue(new Error('Failed to create sale'));
    const user = userEvent.setup();
    render(<CreateSaleForm />);

    await waitFor(() => {
      expect(screen.getByLabelText(/customer/i)).toBeInTheDocument();
    });

    await user.selectOptions(screen.getByLabelText(/customer/i), '1');
    await user.selectOptions(screen.getByLabelText(/salesperson/i), '1');
    await user.selectOptions(screen.getByLabelText(/product/i), '1');
    await user.clear(screen.getByLabelText(/sale price/i));
    await user.type(screen.getByLabelText(/sale price/i), '1000');

    const submitButton = screen.getByRole('button', { name: /create sale/i });
    await user.click(submitButton);

    await waitFor(() => {
      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(/failed to create sale/i)).toBeInTheDocument();
    });
  });
});
