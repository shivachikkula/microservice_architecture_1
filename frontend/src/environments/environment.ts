export const environment = {
  production: false,
  msalConfig: {
    auth: {
      clientId: 'YOUR_AZURE_B2C_CLIENT_ID',
      authority: 'https://YOUR_TENANT_NAME.b2clogin.com/YOUR_TENANT_NAME.onmicrosoft.com/B2C_1_SUSI',
      knownAuthorities: ['YOUR_TENANT_NAME.b2clogin.com'],
      redirectUri: 'http://localhost:4200'
    }
  },
  apiConfig: {
    scopes: ['https://YOUR_TENANT_NAME.onmicrosoft.com/api/user.read'],
    uri: 'http://localhost:5000/api'
  },
  appInsights: {
    connectionString: 'YOUR_APPLICATION_INSIGHTS_CONNECTION_STRING'
  }
};
