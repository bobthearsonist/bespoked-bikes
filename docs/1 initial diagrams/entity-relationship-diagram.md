# Entity Relationship Diagram - Bespoked Bikes

This diagram shows the database entities and their relationships for the Bespoked Bikes sales tracking system.

```mermaid
erDiagram
    CUSTOMER ||--o{ SALE : places
    SALESPERSON ||--o{ SALE : makes
    PRODUCT ||--o{ SALE : "sold in"
    PRODUCT ||--o{ INVENTORY : tracks
    SALESPERSON ||--o| EMPLOYEE : "is a"

    CUSTOMER {
        uuid id PK
        string name
        string email UK
        string phone
        string address
        string city
        string state
        string postal_code
        timestamp created_at
        timestamp updated_at
    }

    SALESPERSON {
        uuid id PK
        uuid employee_id FK
        string first_name
        string last_name
        string email UK
        string phone
        timestamp start_date
        timestamp termination_date
        timestamp created_at
        timestamp updated_at
    }

    EMPLOYEE {
        uuid id PK
        string first_name
        string last_name
        string email UK
        string phone
        string location
        timestamp hire_date
        timestamp created_at
        timestamp updated_at
    }

    PRODUCT {
        uuid id PK
        string product_type
        string name
        string description
        string manufacturer
        decimal cost_price
        decimal retail_price
        decimal commission_percentage
        timestamp created_at
        timestamp updated_at
    }

    INVENTORY {
        uuid id PK
        uuid product_id FK
        string location
        int quantity
        timestamp created_at
        timestamp updated_at
    }

    SALE {
        uuid id PK
        uuid customer_id FK
        uuid salesperson_id FK
        uuid product_id FK
        decimal sale_price
        decimal commission_amount
        string sale_channel
        string location
        timestamp sale_date
        timestamp created_at
        timestamp updated_at
    }
```

## Key Design Decisions

1. **Commission Storage**: Commission percentage is stored on `PRODUCT`, but the actual commission dollar amount is calculated and stored on each `SALE` to preserve historical accuracy.

2. **Sale Price vs Retail Price**: `SALE.sale_price` can differ from `PRODUCT.retail_price` to accommodate discounts, promotions, or negotiations.

3. **Product Extensibility**: The `product_type` field allows for future expansion beyond bicycles (accessories, services, etc.).

4. **Inventory Tracking**: Separate `INVENTORY` table supports multi-location inventory management with simplified quantity tracking.

5. **Employee Hierarchy**: `SALESPERSON` references `EMPLOYEE` to maintain organizational structure while allowing specialized salesperson data.

6. **Sale Channel**: Track whether sales are retail, online, or other channels for future analysis.

7. **Sale Location**: Each sale records the location where it occurred for location-based reporting and analysis.

## Relationships

- One customer can place many sales (1:N)
- One salesperson can make many sales (1:N)
- One product can be sold in many sales (1:N)
- One product can have inventory at multiple locations (1:N)
- One employee can be a salesperson (1:1 optional)
