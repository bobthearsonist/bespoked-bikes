# Entity Relationship Diagram - Bespoked Bikes

This diagram shows the database entities and their relationships for the Bespoked Bikes sales tracking system.

```mermaid
erDiagram
    CUSTOMER ||--o{ SALE : places
    EMPLOYEE ||--o{ SALE : "sells (sold_by)"
    EMPLOYEE ||--o{ SALE : "fulfills (fulfilled_by)"
    PRODUCT ||--o{ SALE : "sold in"
    PRODUCT ||--o{ INVENTORY : tracks

    CUSTOMER {
        uuid id PK
        string name
        timestamp created_at
        timestamp updated_at
    }

    EMPLOYEE {
        uuid id PK
        string name
        Location location "enum: STORE"
        EmployeeRole[] roles "enum: SALESPERSON, FULFILLMENT, ADMIN"
        timestamp hire_date
        timestamp termination_date
        timestamp created_at
        timestamp updated_at
    }

    PRODUCT {
        uuid id PK
        string product_type
        string name
        string description
        string supplier "Will become supplier_id FK when Supplier entity is added"
        decimal cost_price
        decimal retail_price
        decimal commission_percentage
        timestamp created_at
        timestamp updated_at
    }

    INVENTORY {
        uuid id PK
        uuid product_id FK
        Location location "enum: STORE"
        int quantity
        timestamp created_at
        timestamp updated_at
    }

    SALE {
        uuid id PK
        uuid customer_id FK
        uuid sold_by_employee_id FK
        uuid fulfilled_by_employee_id FK "nullable"
        uuid product_id FK
        SaleStatus status "enum: PENDING, FULFILLED"
        decimal sale_price
        decimal commission_amount
        string sale_channel
        Location location "enum: STORE"
        timestamp sale_date
        timestamp fulfilled_date "nullable"
        timestamp created_at
        timestamp updated_at
    }
```

## Enumerations

### EmployeeRole

```
enum EmployeeRole {
    SALESPERSON  // Can create sales
    FULFILLMENT  // Can fulfill sales and manage inventory
    ADMIN        // Administrative access
}
```

### SaleStatus

```
enum SaleStatus {
    PENDING    // Sale created, awaiting fulfillment
    FULFILLED  // Sale completed, product delivered to customer
}
```

### Location

```
enum Location {
    STORE  // Single store location (expandable to multiple locations in future)
}
```

## Key Design Decisions

1. **Employee Role Composition**: Removed separate `SALESPERSON` entity. Employees can have multiple roles through the `roles` array, allowing a single employee to both sell and fulfill orders.

2. **Sale Status Tracking**: Added `status` field to track sale lifecycle. All sales start as `PENDING` and are marked `FULFILLED` after physical handoff to customer.

3. **Dual Employee References**: `SALE` tracks both who sold the product (`sold_by_employee_id`) and who fulfilled it (`fulfilled_by_employee_id`), supporting audit trail and commission tracking.

4. **Commission Storage**: Commission percentage is stored on `PRODUCT`, but the actual commission dollar amount is calculated and stored on each `SALE` to preserve historical accuracy.

5. **Sale Price vs Retail Price**: `SALE.sale_price` can differ from `PRODUCT.retail_price` to accommodate discounts, promotions, or negotiations.

6. **Product Extensibility**: The `product_type` field allows for future expansion beyond bicycles (accessories, services, etc.).

7. **Simplified Customer**: Minimal customer data (just name) - contact details can be added when needed.

8. **Simplified Employee**: Minimal employee data (just name and location) - contact details can be added when needed.

9. **Supplier as String**: The `supplier` field on Product is currently a string. When the Supplier entity is added in future expansion, this will become `supplier_id` FK to enable full supplier relationship management.

10. **Location Enum**: Currently only `STORE`, but using enum allows easy expansion to multiple locations, warehouses, or online channels without schema changes.

11. **Sale Channel**: Track whether sales are retail, online, or other channels for future analysis.

## Relationships

- One customer can place many sales (1:N)
- One employee can sell many products (1:N via sold_by_employee_id)
- One employee can fulfill many sales (1:N via fulfilled_by_employee_id)
- One product can be sold in many sales (1:N)
- One product can have inventory at multiple locations (1:N)

## Future Expansion Entities

These entities were identified in the scenario analysis but are not included in the initial implementation. They can be added when business needs grow:

1. **Fulfillment Order**: For complex fulfillment workflows and tracking delivery obligations
2. **Supplier**: For vendor management and product sourcing
3. **Supplier Order**: For inventory replenishment and purchase order tracking

These will enable more sophisticated inventory management and supplier relationship workflows as the business scales.
