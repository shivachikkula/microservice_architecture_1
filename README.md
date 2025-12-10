# Student Portal - Microservice Architecture

A comprehensive microservice architecture solution with Angular frontend, .NET Core Web APIs, Azure B2C authentication, Azure Service Bus messaging, and Application Insights logging.

## Architecture Overview

This solution consists of three main components:

1. **Frontend (Angular)**: Student portal with Azure B2C authentication and Tailwind CSS
2. **Student Service API (.NET Core)**: REST API for student data management with SQL database
3. **Message Consumer Service (.NET Core)**: Background service for processing Service Bus messages

## Technology Stack

### Frontend
- **Angular 17** - Frontend framework
- **Tailwind CSS** - Utility-first CSS framework
- **Azure MSAL** - Microsoft Authentication Library for Azure B2C
- **Application Insights** - Monitoring and logging

### Backend
- **.NET 8.0** - Web API framework
- **Entity Framework Core** - ORM for SQL Server
- **Azure Service Bus** - Message queue for asynchronous processing
- **Azure B2C** - Identity and access management
- **Application Insights** - Monitoring and logging

## Project Structure

```
microservice_architecture_1/
├── frontend/                           # Angular application
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/
│   │   │   │   ├── login/            # Login component
│   │   │   │   ├── dashboard/        # Dashboard component
│   │   │   │   └── student-trf-details/  # Student form component
│   │   │   ├── models/               # TypeScript models
│   │   │   ├── services/             # Angular services
│   │   │   └── app.module.ts         # Main app module with MSAL config
│   │   ├── environments/             # Environment configurations
│   │   └── styles.css                # Global styles with Tailwind
│   ├── tailwind.config.js
│   └── package.json
│
├── backend/
│   ├── StudentService/               # Main student API
│   │   ├── Controllers/
│   │   │   └── StudentController.cs  # REST endpoints
│   │   ├── Data/
│   │   │   └── StudentDbContext.cs   # EF Core DbContext
│   │   ├── Models/                   # Domain models
│   │   ├── Services/
│   │   │   └── ServiceBusService.cs  # Service Bus producer
│   │   ├── Program.cs                # App configuration
│   │   └── appsettings.json
│   │
│   └── MessageConsumerService/       # Service Bus consumer
│       ├── Controllers/
│       │   └── HealthController.cs
│       ├── Models/
│       │   └── StudentMessage.cs
│       ├── Services/
│       │   ├── ServiceBusConsumerService.cs
│       │   └── MessageProcessorHostedService.cs
│       ├── Program.cs
│       └── appsettings.json
│
├── README.md
└── CONFIGURATION_GUIDE.md
```

## Prerequisites

- **Node.js** (v18 or higher)
- **.NET 8.0 SDK**
- **SQL Server** (or Azure SQL Database)
- **Azure Subscription** for:
  - Azure B2C tenant
  - Azure Service Bus namespace
  - Application Insights instance

## Quick Start

### 1. Configure Azure Resources

Follow the detailed [Configuration Guide](CONFIGURATION_GUIDE.md) to set up:
- Azure B2C tenant and app registrations
- Azure Service Bus namespace and queue
- Application Insights
- SQL Database

### 2. Configure Applications

Update configuration files with your Azure resource values:

**Frontend**: `frontend/src/environments/environment.ts`
**Student Service**: `backend/StudentService/appsettings.json`
**Message Consumer**: `backend/MessageConsumerService/appsettings.json`

### 3. Run Database Migrations

```bash
cd backend/StudentService
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Start Applications

**Frontend**:
```bash
cd frontend
npm install
npm start
```

**Student Service**:
```bash
cd backend/StudentService
dotnet restore
dotnet run
```

**Message Consumer**:
```bash
cd backend/MessageConsumerService
dotnet restore
dotnet run
```

## Features

### Authentication & Authorization
- Azure B2C authentication for frontend
- JWT token validation in backend APIs
- Secure API endpoints with authorization
- MSAL integration for seamless authentication

### Student Management
- Comprehensive student form with:
  - Student details (name, DOB, gender)
  - Four separate address sections
  - Form validation
  - Auto-save functionality

### Asynchronous Processing
- Service Bus integration for message queuing
- Automatic message processing
- Dead-letter queue handling
- Retry logic

### Monitoring & Observability
- Application Insights integration
- Request/response tracking
- Custom event logging
- Exception tracking
- Live metrics and performance monitoring

## API Endpoints

### Student API (port 5000/5001)

- **GET** `/api/student` - Get all students
- **GET** `/api/student/{id}` - Get student by ID
- **POST** `/api/student` - Create new student (triggers Service Bus message)
- **PUT** `/api/student/{id}` - Update student
- **DELETE** `/api/student/{id}` - Delete student

### Message Consumer API

- **GET** `/api/health` - Health check endpoint
- **GET** `/health` - Built-in health check

## Student Form Details

The student-trf-details form includes:

### Student Details Section
- First Name (required, min 2 characters)
- Last Name (required, min 2 characters)
- Date of Birth (required)
- Gender (required, dropdown: Male/Female/Other)

### Address Sections (1-4)
Each of the four address sections contains:
- Address Line 1 (required)
- Address Line 2 (optional)
- City (required)
- State (required)
- Zipcode (required, format: 12345 or 12345-6789)

## Architecture Flow

1. **User Authentication**: User logs in via Angular app using Azure B2C
2. **Form Submission**: User fills out student form and submits
3. **API Call**: Angular app sends POST request to Student Service API (with JWT token)
4. **Data Persistence**: API validates token, saves data to SQL database
5. **Message Queue**: API sends message to Service Bus queue
6. **Message Processing**: Consumer service picks up message and processes it
7. **Logging**: All operations logged to Application Insights

## Development

### Database Migrations

Create new migration:
```bash
cd backend/StudentService
dotnet ef migrations add MigrationName
```

Apply migrations:
```bash
dotnet ef database update
```

### Testing

**Frontend Tests**:
```bash
cd frontend
npm test
```

**Backend Tests**:
```bash
cd backend/StudentService
dotnet test
```

## Monitoring

### Application Insights Dashboards

Access Application Insights in Azure Portal to view:

- **Live Metrics**: Real-time performance metrics
- **Application Map**: Visual representation of service dependencies
- **Transaction Search**: Detailed request traces
- **Performance**: Response times and dependencies
- **Failures**: Exception tracking and error rates
- **Custom Events**: Track specific user actions

### Service Bus Monitoring

Monitor Service Bus in Azure Portal:

- Queue length
- Message rates
- Dead-letter queue
- Processing errors

## Troubleshooting

### Common Issues

**CORS Errors**:
- Verify `CorsOrigins` in `appsettings.json` matches frontend URL
- Check browser console for exact error

**Authentication Failures**:
- Verify Azure B2C configuration
- Check redirect URIs match exactly
- Ensure user flow is published

**Database Connection Issues**:
- Verify connection string
- Check firewall rules
- Test connection with SQL Server Management Studio

**Service Bus Issues**:
- Verify connection string and queue name
- Check queue exists in Azure Portal
- Review dead-letter queue for failed messages

## Production Deployment

### Frontend
1. Build: `npm run build --prod`
2. Deploy to Azure Static Web Apps or App Service
3. Update `environment.prod.ts` with production URLs
4. Configure custom domain and SSL

### Backend APIs
1. Publish: `dotnet publish -c Release`
2. Deploy to Azure App Service or Container Apps
3. Use Azure Key Vault for secrets
4. Enable managed identity for Azure resources
5. Configure auto-scaling

### Database
1. Use Azure SQL Database with appropriate tier
2. Enable automated backups
3. Configure geo-replication if needed
4. Implement connection pooling

### Best Practices
- Use separate Azure B2C tenants for environments
- Implement proper logging levels
- Configure health checks
- Set up monitoring alerts
- Use Application Insights for APM
- Implement proper error handling

## Security Considerations

- ✅ Azure B2C for authentication
- ✅ JWT token validation
- ✅ HTTPS enforcement
- ✅ CORS configuration
- ✅ SQL injection prevention (Entity Framework)
- ✅ Secrets management (Azure Key Vault recommended)
- ✅ Principle of least privilege
- ✅ Input validation

## Contributing

This is a demonstration project. For production use:
1. Add comprehensive unit tests
2. Implement integration tests
3. Add API versioning
4. Implement rate limiting
5. Add input sanitization
6. Implement proper error handling
7. Add API documentation (Swagger)

## License

This project is provided as-is for educational purposes.

## Documentation

- [Configuration Guide](CONFIGURATION_GUIDE.md) - Detailed setup instructions
- [Azure B2C Documentation](https://docs.microsoft.com/azure/active-directory-b2c/)
- [Azure Service Bus Documentation](https://docs.microsoft.com/azure/service-bus-messaging/)
- [Application Insights Documentation](https://docs.microsoft.com/azure/azure-monitor/app/app-insights-overview)
