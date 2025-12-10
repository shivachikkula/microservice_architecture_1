import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { ApplicationInsightsService } from '../../services/application-insights.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(
    private authService: MsalService,
    private router: Router,
    private appInsightsService: ApplicationInsightsService
  ) {}

  ngOnInit(): void {
    this.appInsightsService.logPageView('Login Page');

    // Check if user is already logged in
    if (this.authService.instance.getAllAccounts().length > 0) {
      this.router.navigate(['/dashboard']);
    }
  }

  login(): void {
    this.appInsightsService.logEvent('LoginAttempt');
    this.authService.loginRedirect();
  }
}
