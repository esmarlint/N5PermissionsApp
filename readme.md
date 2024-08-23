# N5 Permission Manager

N5 Permission Manager is a comprehensive solution for managing employee permissions. It consists of a .NET backend API and a React frontend, utilizing SQL Server for data storage, Elasticsearch for efficient searching, and Kafka for event streaming.

## Features

- Create, read, update, and delete employee permissions
- Paginated list of permissions
- Real-time updates using Kafka
- Efficient search capabilities with Elasticsearch
- Dockerized application for easy deployment

## Technologies Used

- Backend: .NET 8.0, Entity Framework Core
- Frontend: React, Material-UI
- Database: SQL Server
- Search: Elasticsearch
- Message Broker: Kafka
- Containerization: Docker

## Prerequisites

- Docker and Docker Compose
- .NET SDK 8.0 or later
- Node.js 18 or later
- npm (usually comes with Node.js)
- Git

## Installation

1. Clone the repository:

   ```
   git clone https://github.com/yourusername/n5-permission-manager.git
   cd n5-permission-manager
   ```

2. Create a `.env` file in the root directory with the following content:

   ```
   VITE_API_URL=http://localhost:5000/api
   ```

3. Build and start the application using Docker Compose:

   ```
   docker-compose up -d
   ```

4. The application services will be available at:
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:5000
   - SQL Server: localhost:1433
   - Elasticsearch: http://localhost:9200
   - Kafka: localhost:9092

## Usage

1. Open a web browser and navigate to `http://localhost:3000`.
2. Use the interface to manage employee permissions:
   - View the list of permissions
   - Create new permissions
   - Modify existing permissions

## API Endpoints

- `GET /api/permissions`: Retrieve a paginated list of permissions
- `GET /api/permissions/{id}`: Retrieve a specific permission
- `POST /api/permissions`: Create a new permission
- `PUT /api/permissions/{id}`: Update an existing permission
- `GET /api/permission-types`: Retrieve all permission types

## Development

To run the frontend outside of Docker for development:

1. Navigate to the frontend directory:

   ```
   cd frontend
   ```

2. Install dependencies:

   ```
   npm install
   ```

3. Start the development server:
   ```
   npm run dev
   ```

## Testing

- Backend unit tests: Navigate to the backend test project directory and run:

  ```
  dotnet test
  ```

- Frontend tests: In the frontend directory, run:
  ```
  npm test
  ```

## Troubleshooting

- If you encounter database migration issues, run:

  ```
  dotnet ef database update
  ```

- For service-specific logs, use:
  ```
  docker-compose logs [service_name]
  ```

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

- N5 for the project requirements and support
- All open-source libraries and tools used in this project
