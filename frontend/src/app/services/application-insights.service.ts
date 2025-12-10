import { Injectable } from '@angular/core';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApplicationInsightsService {
  private appInsights: ApplicationInsights;

  constructor() {
    this.appInsights = new ApplicationInsights({
      config: {
        connectionString: environment.appInsights.connectionString,
        enableAutoRouteTracking: true,
        enableCorsCorrelation: true,
        enableRequestHeaderTracking: true,
        enableResponseHeaderTracking: true
      }
    });
    this.appInsights.loadAppInsights();
  }

  logPageView(name?: string, url?: string) {
    this.appInsights.trackPageView({
      name: name,
      uri: url
    });
  }

  logEvent(name: string, properties?: { [key: string]: any }) {
    this.appInsights.trackEvent({ name: name }, properties);
  }

  logMetric(name: string, average: number, properties?: { [key: string]: any }) {
    this.appInsights.trackMetric({ name: name, average: average }, properties);
  }

  logException(exception: Error, severityLevel?: number) {
    this.appInsights.trackException({ exception: exception, severityLevel: severityLevel });
  }

  logTrace(message: string, properties?: { [key: string]: any }) {
    this.appInsights.trackTrace({ message: message }, properties);
  }
}
