# Employee Role Design Decisions

the employee role was changed to be a Flags enum to keep the widest storage compatability possible. My plan is to use postres testcontainers in the integration tests suite becasue they spin up _very_ fast and allow efficient testing. i try to avoid tests that skew too far towards validating the server implementation so this should be fine. if there are specific spots taht need more rigorous testing, eg transaction rollbacks etc, then those tests can be isolated with a different container that mirrors whatever we settle on for the backend.

this approach did wind up with a little complexity in the mapping layer, but if anything that highlights the benefits of the layered architecture we have in place because the frontend is a simple enum and abstracts them form the backend implementation while the repository layer can deal with the storage format directly.
