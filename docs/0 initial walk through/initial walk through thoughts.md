# initial walk through thoughts

## commission

we need some sort of info on the sale for commission
the requirements state that it should be assigned to the product
if its a percentage i see a couple sue cases where it may make more sense to be on the sale
examples:
more commission to particular salespeople? that would have to be on the salesperson.
different commission rates for different customers? that would have to be on the sale.
different commission rates for different time periods? that would have to be on the sale.
different commission rates for different sales channels? that would have to be on the sale, or even the location?

maybe we start with it on the product and then we iterate on it. it wouldn't be a huge deal to introduce something at a latter date, we could do a migration of data and as long as we track the commission dollar amount on the sale itself then we wouldn't effect historical data.

**commission dollar amount should be a fixed data point, we can calculate it using functions defined in various places though. for now we can make that place the SALE (not the product) because I'm allowing for the sale price to be different than the product price. so we can calculate commission as a percentage of the sale price and store it on the sale itself. that can be split out later if needed.**

## reporting

i think we can actually come back to reporting after we have some entities, it may result in having to do some refactoring but i think i can do that quickly and i dont want to spend too much time on the design portion of a timed exercise (this would be a different decision maybe in a work environment)
