# Simple TCP Server in C#

This is a basic TCP server implemented in C# that handles basic HTTP requests for specific routes.

## Description

This code implements a simple TCP server that accepts client connections and processes basic HTTP requests for different routes. It contains the following functionalities:

- Receives HTTP GET requests from clients.
- Parses the request route and responds with specific data for known routes.
- Supports a specific route, "/resposta1," returning specific content defined in the code.

- ![image](https://github.com/C4mps41205/ApiWithSocket/assets/93053849/ab48a29d-b720-4b4b-9cc4-199ff04b19c0)
- ![image](https://github.com/C4mps41205/ApiWithSocket/assets/93053849/dbcb25ce-b6b3-4a27-a7a3-51ced63f867d)



## How to Use

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) installed on your machine.

### Execution

1. Clone the repository:

```bash
git clone https://github.com/C4mps41205/ApiWithSocket/)https://github.com/C4mps41205/ApiWithSocket/
```

## Testing the Server

1. Open a web browser or use tools like cURL or Postman.
2. Access the server using the configured IP and port (for example, `http://192.168.0.22:666`).
3. Try different routes, including `/resposta1`, to see the server's responses.

### Using cURL

You can use cURL, a command-line tool for transferring data with URL syntax, to interact with the server.

- To perform a GET request for the `/resposta1` route:

    ```bash
    curl http://192.168.0.22:666/resposta1
    ```

This will output the response from the server for the `/resposta1` route.

### Using Postman

Postman is a popular API development environment that allows you to create and test APIs.

- Open Postman and create a new request.
- Set the request type to GET.
- Enter the server's URL in the address bar (for example, `http://192.168.0.22:666/resposta1`).
- Send the request to see the server's response.

Experimenting with different routes and tools will showcase the server's ability to handle various requests and responses.

## Additional Notes

- Ensure to adjust the server's IP address to the correct one on your local network.
- This is a basic example and can be expanded to handle more routes, HTTP methods, and more complex logic.




