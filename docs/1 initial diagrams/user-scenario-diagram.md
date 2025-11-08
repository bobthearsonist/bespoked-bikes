# Sale with Fulfillment Scenario

This sequence diagram represents the scenario identified in the initial sketch, showing the complete flow from sale creation through potential supplier order fulfillment.

## Color Coding & UML Notation

- **Human actors** (stick figures):
  - Customer, Salesperson - existing
  - Fulfillment User - newly identified in exercise
- **UML Message Types**:
  - Solid arrows (`->>`) - Synchronous messages/calls
  - Dashed arrows (`-->>`) - Return messages/responses
- **Physical interactions** (gray rect boxes): Real-world actions outside the system (goes to store, product handoff, etc.)
- **Standard entities** (boxes): Existing system entities (Product, Inventory, Sales)
- **New entities** (orange highlighted boxes): Entities identified during this scenario exercise (Location, Fulfillment Order, Supplier Order, Supplier)
  - Boxes have black outlines
  - Lifecycle lines and boxes are color-coded to indicate newly identified components
  - **Location**: Shown in diagram for future iteration; will likely be implemented as a property on Inventory records rather than a separate entity initially

```mermaid
sequenceDiagram
    actor Customer
    actor Salesperson

    box rgb(255, 220, 200) New Role - Identified in Exercise
    actor FulfillmentUser as Fulfillment User
    end

    participant Inventory
    participant Location
    participant Sales
    participant Product
    participant FulfillmentOrder as Fulfillment Order
    participant SupplierOrder as Supplier Order
    participant Supplier

    %% Color coding for new entities
    box rgb(255, 220, 200) New Entities - Identified in Exercise
    participant Location
    participant FulfillmentOrder as Fulfillment Order
    participant SupplierOrder as Supplier Order
    participant Supplier
    end

    Note over Customer,Inventory: Customer checks availability online
    Customer->>Inventory: Check availability
    Inventory-->>Customer: Show inventory status

    rect rgba(200, 200, 200, 0.2)
        Note over Customer,Salesperson: Physical interactions (outside system)
        Customer->>Location: Goes to store location
        Customer->>Salesperson: Talks to salesperson about product
    end

    Note over Salesperson,Inventory: Salesperson verifies inventory at location
    Salesperson->>Inventory: Check availability (location)
    Note right of Inventory: Future: location as entity
    Inventory->>Location: Query by location (future iteration)
    Inventory-->>Salesperson: Availability

    Salesperson->>Sales: Create (product, customer, location, quantity)
    Sales->>Sales: Create sale record (pending status)

    alt Product Available in Inventory
        Sales->>FulfillmentOrder: Create fulfillment order

    else Product Not Available - Need to Order
        Sales->>SupplierOrder: Create supplier order (pending status)

        FulfillmentUser->>Supplier: Receive product from supplier
        Supplier-->>FulfillmentUser: Product

        FulfillmentUser->>Inventory: Update inventory (increment quantity)

        Sales->>FulfillmentOrder: Create fulfillment order
    end

    FulfillmentUser->>FulfillmentOrder: Get fulfillment order
    FulfillmentOrder-->>FulfillmentUser: Order details

    rect rgba(200, 200, 200, 0.2)
        Note over FulfillmentUser,Product: Fulfill order
        FulfillmentUser->>Product: Retrieve product
        Product-->>FulfillmentUser: Product
    end

    FulfillmentUser->>FulfillmentOrder: Mark as fulfilled
    FulfillmentOrder->>Inventory: Update quantity
    FulfillmentOrder->>Sales: Update sale status to fulfilled
    Sales->>Sales: Calculate commission

    rect rgba(200, 200, 200, 0.2)
        Note over FulfillmentUser,Customer: Physical handoff
        FulfillmentUser->>Customer: Provide product
    end
```

## Key Insights from This Scenario

### New Entities & Roles Discovered

1. **Location**: Represents a physical store location where sales occur

   - Has location-specific inventory (initially as a property on Inventory)
   - Context for customer visits and sales transactions
   - Links to Inventory to track stock at specific locations

2. **Fulfillment Order**: Represents the obligation to deliver a product to a customer

   - Created for every sale (regardless of stock availability)
   - Links to Sale
   - Tracks fulfillment status (pending, fulfilled)
   - Fulfilled by Fulfillment User who does physical handoff to customer

3. **Supplier Order**: Represents a purchase order placed with a supplier to restock inventory

   - Created only when inventory is insufficient
   - Links to Product
   - Links to Supplier
   - Tracks order status (pending, fulfilled)
   - Processed by Fulfillment User who places order with supplier

4. **Supplier**: External entity that provides products to restock inventory

   - Has supplier contact information
   - Manages product catalog and pricing
   - Processes supplier orders from the system

5. **Fulfillment User** (newly identified role): Human actor who manages both customer fulfillment and supplier ordering
   - Fulfills customer orders by doing physical handoffs
   - Processes supplier orders when inventory is insufficient
   - Places orders with suppliers
   - Receives stock and updates inventory
   - Updates sale status when fulfilled

### Extended Business Flow

This scenario reveals that the system needs to handle:

1. **All Sales Start as Pending**: Every sale starts with "pending" status and is only marked fulfilled after physical handoff
2. **All Sales Create Fulfillment Orders**: Every sale creates a Fulfillment Order to track the delivery obligation
3. **In-Stock Sales**: When inventory is available:
   - Create sale (pending status)
   - Create Fulfillment Order (ready to fulfill)
   - Fulfillment User fulfills order
   - Decrement inventory when fulfilled
   - Update sale to fulfilled status
   - Physical handoff to customer
4. **Out-of-Stock Sales**: When inventory is unavailable:
   - Create sale (pending status)
   - Create Fulfillment Order (pending status)
   - Create Supplier Order to restock
   - Fulfillment User places order with supplier
   - When stock arrives, increment inventory
   - Fulfillment User fulfills the Fulfillment Order
   - Decrement inventory when fulfilled
   - Update sale to fulfilled status
   - Physical handoff to customer

### Impact on Existing Diagrams

This scenario suggests we need to:

- Add **Location** entity to the ERD (initially as a property on Inventory)
- Add **Fulfillment Order** entity to the ERD
- Add **Supplier Order** entity to the ERD
- Add **Supplier** entity to the ERD
- Add **FulfillmentService** to the class diagram
- Update **Sale** entity to include status field (pending, fulfilled, cancelled) and link to Location and Fulfillment Order
- Update **Inventory** to track stock by Location
- Add fulfillment tracking and supplier management capabilities
