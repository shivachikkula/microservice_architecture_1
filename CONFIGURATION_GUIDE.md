# Configuration Guide

This guide provides step-by-step instructions for configuring all Azure resources and application settings.

## Table of Contents

1. [Azure B2C Setup](#azure-b2c-setup)
2. [Azure Service Bus Setup](#azure-service-bus-setup)
3. [Application Insights Setup](#application-insights-setup)
4. [SQL Database Setup](#sql-database-setup)
5. [Application Configuration](#application-configuration)

## Azure B2C Setup

### Step 1: Create Azure B2C Tenant

1. Sign in to the [Azure Portal](https://portal.azure.com)
2. Search for "Azure AD B2C" and select it
3. Click "Create a new Azure AD B2C Tenant"
4. Choose "Create a new Azure AD B2C Tenant"
5. Fill in the details:
   - Organization name: `StudentPortal`
   - Initial domain name: `studentportal` (this will be studentportal.onmicrosoft.com)
   - Country/Region: Select your country
6. Click "Create"
7. Wait for tenant creation (takes a few minutes)
8. Switch to the new tenant

### Step 2: Register Frontend Application

1. In your B2C tenant, go to "App registrations"
2. Click "New registration"
3. Fill in the details:
   - Name: `StudentPortalApp`
   - Supported account types: "Accounts in any identity provider or organizational directory"
   - Redirect URI:
     - Platform: "Single-page application (SPA)"
     - URI: `http://localhost:4200`
4. Click "Register"
5. **Save the Application (client) ID** - you'll need this for frontend configuration
6. Go to "Authentication" in the left menu
7. Under "Implicit grant and hybrid flows":
   - ✅ Check "Access tokens"
   - ✅ Check "ID tokens"
8. Click "Save"

### Step 3: Register API Application

1. Go back to "App registrations"
2. Click "New registration"
3. Fill in the details:
   - Name: `StudentServiceAPI`
   - Supported account types: "Accounts in any identity provider or organizational directory"
4. Click "Register"
5. **Save the Application (client) ID**
6. Go to "Expose an API" in the left menu
7. Click "Add a scope"
8. Accept the default Application ID URI or customize it
9. Click "Save and continue"
10. Add a scope:
    - Scope name: `user.read`
    - Admin consent display name: `Read user data`
    - Admin consent description: `Allows the app to read user data`
    - State: Enabled
11. Click "Add scope"
12. **Save the full scope URI** (e.g., `https://studentportal.onmicrosoft.com/api/user.read`)

### Step 4: Grant API Permissions to Frontend

1. Go to the **StudentPortalApp** registration
2. Click "API permissions"
3. Click "Add a permission"
4. Select "My APIs"
5. Select "StudentServiceAPI"
6. Select the "user.read" scope
7. Click "Add permissions"
8. Click "Grant admin consent" (if you're an admin)

### Step 5: Create User Flow

1. In Azure B2C, go to "User flows"
2. Click "New user flow"
3. Select "Sign up and sign in"
4. Select "Recommended" version
5. Name it: `B2C_1_SUSI`
6. Under "Identity providers":
   - ✅ Check "Email signup"
7. Under "User attributes and token claims":
   - Select attributes to collect: Given name, Surname, Display name
   - Select claims to return: Display Name, Given Name, Surname, User's Object ID
8. Click "Create"

### Step 6: Get Tenant Information

Note down the following information:

- **Tenant Name**: `studentportal.onmicrosoft.com`
- **Tenant ID**: Found in Azure AD B2C > Overview
- **Frontend Client ID**: From StudentPortalApp registration
- **API Client ID**: From StudentServiceAPI registration
- **Scope URI**: From StudentServiceAPI > Expose an API
- **User Flow Name**: `B2C_1_SUSI`
- **Authority URL**: `https://studentportal.b2clogin.com/studentportal.onmicrosoft.com/B2C_1_SUSI`

## Azure Service Bus Setup

### Step 1: Create Service Bus Namespace

1. In Azure Portal, search for "Service Bus"
2. Click "Create"
3. Fill in the details:
   - Subscription: Select your subscription
   - Resource group: Create new or use existing
   - Namespace name: `studentportal-sb` (must be globally unique)
   - Location: Select your region
   - Pricing tier: **Standard** (required for queues)
4. Click "Review + create"
5. Click "Create"
6. Wait for deployment to complete

### Step 2: Create Queue

1. Go to your Service Bus namespace
2. Click "Queues" in the left menu
3. Click "+ Queue"
4. Fill in the details:
   - Name: `student-queue`
   - Max queue size: 1 GB
   - Message time to live: 14 days
   - Lock duration: 30 seconds
   - Enable duplicate detection: No
   - Enable dead lettering: Yes
   - Enable sessions: No
5. Click "Create"

### Step 3: Get Connection String

1. In your Service Bus namespace
2. Click "Shared access policies" in the left menu
3. Click "RootManageSharedAccessKey"
4. **Copy the "Primary Connection String"**
5. Save this connection string securely

Example connection string format:
```
Endpoint=sb://studentportal-sb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YOUR_KEY_HERE
```

## Application Insights Setup

### Step 1: Create Application Insights

1. In Azure Portal, search for "Application Insights"
2. Click "Create"
3. Fill in the details:
   - Subscription: Select your subscription
   - Resource group: Use same as Service Bus
   - Name: `studentportal-insights`
   - Region: Select your region
   - Workspace: Create new or use existing Log Analytics workspace
4. Click "Review + create"
5. Click "Create"

### Step 2: Get Connection String

1. Go to your Application Insights resource
2. Click "Overview" in the left menu
3. **Copy the "Connection String"** from the top
4. Save this connection string

Example connection string format:
```
InstrumentationKey=YOUR_KEY;IngestionEndpoint=https://REGION.in.applicationinsights.azure.com/;LiveEndpoint=https://REGION.livediagnostics.monitor.azure.com/
```

## SQL Database Setup

### Option 1: Azure SQL Database (Recommended for Production)

1. In Azure Portal, search for "SQL databases"
2. Click "Create"
3. Fill in the details:
   - Subscription: Select your subscription
   - Resource group: Use same as other resources
   - Database name: `StudentDB`
   - Server: Create new server
     - Server name: `studentportal-sql` (must be globally unique)
     - Location: Select your region
     - Authentication: SQL authentication
     - Server admin login: `sqladmin`
     - Password: Create a strong password
   - Compute + storage: Basic (for development) or Standard (for production)
4. Click "Review + create"
5. Click "Create"

#### Configure Firewall

1. Go to your SQL Server resource
2. Click "Networking" in the left menu
3. Under "Firewall rules":
   - ✅ Check "Allow Azure services and resources to access this server"
   - Add your client IP address
4. Click "Save"

#### Get Connection String

1. Go to your SQL Database
2. Click "Connection strings" in the left menu
3. Copy the "ADO.NET" connection string
4. Replace `{your_password}` with your actual password

### Option 2: Local SQL Server (Development)

1. Install SQL Server Express or Developer edition
2. Install SQL Server Management Studio (SSMS)
3. Create database:
   ```sql
   CREATE DATABASE StudentDB;
   ```
4. Connection string:
   ```
   Server=localhost;Database=StudentDB;Integrated Security=true;TrustServerCertificate=true;
   ```

## Application Configuration

### Frontend Configuration

1. Open `frontend/src/environments/environment.ts`
2. Replace placeholders with your values:

```typescript
export const environment = {
  production: false,
  msalConfig: {
    auth: {
      clientId: 'YOUR_FRONTEND_CLIENT_ID', // From Step 2 of B2C setup
      authority: 'https://studentportal.b2clogin.com/studentportal.onmicrosoft.com/B2C_1_SUSI',
      knownAuthorities: ['studentportal.b2clogin.com'],
      redirectUri: 'http://localhost:4200'
    }
  },
  apiConfig: {
    scopes: ['https://studentportal.onmicrosoft.com/api/user.read'],
    uri: 'http://localhost:5000/api'
  },
  appInsights: {
    connectionString: 'YOUR_APP_INSIGHTS_CONNECTION_STRING'
  }
};
```

3. For production, update `environment.prod.ts` with production URLs

### StudentService Configuration

1. Open `backend/StudentService/appsettings.json`
2. Replace placeholders:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_SQL_CONNECTION_STRING"
  },
  "AzureAdB2C": {
    "Instance": "https://studentportal.b2clogin.com/",
    "ClientId": "YOUR_API_CLIENT_ID",
    "Domain": "studentportal.onmicrosoft.com",
    "SignUpSignInPolicyId": "B2C_1_SUSI",
    "TenantId": "YOUR_TENANT_ID"
  },
  "ServiceBus": {
    "ConnectionString": "YOUR_SERVICE_BUS_CONNECTION_STRING",
    "QueueName": "student-queue"
  },
  "ApplicationInsights": {
    "ConnectionString": "YOUR_APP_INSIGHTS_CONNECTION_STRING"
  },
  "CorsOrigins": "http://localhost:4200"
}
```

### MessageConsumerService Configuration

1. Open `backend/MessageConsumerService/appsettings.json`
2. Replace placeholders:

```json
{
  "ServiceBus": {
    "ConnectionString": "YOUR_SERVICE_BUS_CONNECTION_STRING",
    "QueueName": "student-queue"
  },
  "ApplicationInsights": {
    "ConnectionString": "YOUR_APP_INSIGHTS_CONNECTION_STRING"
  }
}
```

## Verification Steps

### 1. Test Azure B2C

1. Navigate to: `https://studentportal.b2clogin.com/studentportal.onmicrosoft.com/B2C_1_SUSI/oauth2/v2.0/authorize?client_id=YOUR_CLIENT_ID&response_type=id_token&redirect_uri=http://localhost:4200&scope=openid&nonce=test`
2. You should see the B2C sign-in page
3. Try creating a test user account

### 2. Test Service Bus

1. In Azure Portal, go to your Service Bus namespace
2. Go to "Queues" > "student-queue"
3. Click "Service Bus Explorer"
4. Try sending a test message
5. Verify it appears in the queue

### 3. Test Application Insights

1. Run any of your applications
2. Go to Application Insights in Azure Portal
3. Click "Live Metrics"
4. You should see live telemetry data

### 4. Test Database Connection

From StudentService directory:
```bash
dotnet ef database update
```

This should create the database tables.

## Common Issues

### Issue: B2C "Redirect URI mismatch"

**Solution**: Ensure redirect URI in Azure B2C exactly matches your application URL (including trailing slashes)

### Issue: "Unable to connect to Service Bus"

**Solution**:
- Verify connection string is correct
- Check firewall rules
- Ensure pricing tier is Standard or above

### Issue: "SQL connection failed"

**Solution**:
- Check firewall rules allow your IP
- Verify credentials in connection string
- Test connection with SSMS

### Issue: Application Insights not receiving data

**Solution**:
- Verify connection string is correct
- Check that instrumentation key is valid
- Wait a few minutes for data to appear (there can be a delay)

## Security Notes

1. **Never commit secrets** to source control
2. Use **Azure Key Vault** in production
3. Use **Managed Identities** where possible
4. Regularly rotate keys and secrets
5. Use separate B2C tenants for dev/prod
6. Implement least-privilege access

## Next Steps

After completing this configuration:

1. Run database migrations
2. Start all three services
3. Test the complete flow:
   - Login through frontend
   - Create a student record
   - Verify message in Service Bus
   - Check logs in Application Insights

For detailed running instructions, see the main [README.md](README.md).
