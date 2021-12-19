# Misc

## Loadtesting

We use [k6](https://k6.io/open-source/) to loadtest the program. This way we can load the program with a given amount of users and generate reports on how many fails/success we get.

### How to run

1. Makue sure k6 is installed. You can find the instructions [here](https://k6.io/docs/getting-started/installation/)
1. Start the program `dotnet run --project Server`
1. Run the loadtest in a different terminal session `k6 run --insecure-skip-tls-verify Misc/k6test.js`
1. ???
1. Profit 🚀

### Previous runs

In the two files, loadtest75 and loadtest80 is the results of two previous loadtests. One with 75 concurrent users and another with 80. These were conducted on a machine with the following specs:

- MacBook Pro (13-inch, M1, 2020)
- 16 gb ram
- Appæe M1
- MacOS 11.5.2
