export const environment = {
  production: true,
  msalConfig: {
    auth: {
      clientId: 'YOUR_AZURE_B2C_CLIENT_ID',
      authority: 'https://YOUR_TENANT_NAME.b2clogin.com/YOUR_TENANT_NAME.onmicrosoft.com/B2C_1_SUSI',
      knownAuthorities: ['YOUR_TENANT_NAME.b2clogin.com'],
      redirectUri: 'https://your-production-url.com'
    }
  },
  apiConfig: {
    scopes: ['https://YOUR_TENANT_NAME.onmicrosoft.com/api/user.read'],
    uri: 'https://your-api-url.com/api'
  },
  appInsights: {
    connectionString: 'YOUR_APPLICATION_INSIGHTS_CONNECTION_STRING'
  }
};
