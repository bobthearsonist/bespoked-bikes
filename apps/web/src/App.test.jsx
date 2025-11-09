import { render, screen } from '@testing-library/react';
import App from './App';

// Mock the CreateSaleForm component
jest.mock('./components/CreateSaleForm', () => {
  return function MockCreateSaleForm() {
    return <div data-testid="create-sale-form">Create Sale Form</div>;
  };
});

describe('App', () => {
  it('renders the app header', () => {
    render(<App />);
    expect(screen.getByText(/bespoked bikes - sales management/i)).toBeInTheDocument();
  });

  it('renders the CreateSaleForm component', () => {
    render(<App />);
    expect(screen.getByTestId('create-sale-form')).toBeInTheDocument();
  });
});
