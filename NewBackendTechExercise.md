# BeSpoked Bikes -- Backend API Design Exercise

BeSpoked Bikes is a high-end bicycle shop that wants to build a backend system to support a new sales tracking application. The goal is to track bicycle sales, calculate commissions for salespeople, and eventually support quarterly bonuses.

You are tasked with designing and implementing the API layer and supporting backend for this system. The client application will consume your API to display and manage data.

## High-Level Requirements

### Customer Management

- The API should support create, read, and update operations for customers.
- The application should store customer name and contact information
- Creating duplicate customers should be prevented.
- Error conditions should return handled error responses with a clear message indicating the problem.

### Salesperson Management

- The API should support create, read, and update operations for salespeople.
- Creating duplicate salespeople should be prevented.
- Error conditions should return handled error responses with a clear message indicating the problem.

### Bicycle Inventory

- The API should support create, read, and update operations for bicycles.
- Creating duplicate bicycles should be prevented.
- The store may choose to sell additional types of products at a future date
- The store will need to monitor the inventory of all of its products
- Bikes are purchased from a manufacturer at a specific price, which may be useful for understanding profit margins over time.
- Error conditions should return handled error responses with a clear message indicating the problem.

### Sales Tracking

- The API should support create and read operations for sales.
- A sale should include the bicycle, customer, salesperson, sale date, and sale price.
- Each product sold has a percentage of the profit that should be awarded to the salesperson as a commission.
- The API should support filtering sales by date range.
- Error conditions should return handled error responses with a clear message indicating the problem.

### Quarterly Reporting

- The API should support read-only access to a report of commissions earned by each salesperson.
- The report should group sales by calendar quarter and include total commission earned per salesperson.
- The report should be generated dynamically based on recorded sales data.

## Technical Expectations

- Design a RESTful API
- Implement a data layer to support your API. You may choose any database technology.
- Seed the system with sample data to demonstrate functionality.
- Consider performance and scalability in your design.
- You are not required to build a frontend.

## Deliverables

- Source code published to a public repository (e.g., GitHub).
- A swagger page or postman collection allowing us to easily test the application
- A brief README explaining:
  - Any design decisions or assumptions you want to add
  - How to run the application.
- Optional: Host the API (e.g., on Azure or another platform).

## Notes

- You are encouraged to make reasonable assumptions and design decisions.
- The goal is to evaluate your ability to model data, design APIs, and think about system performance and extensibility.
