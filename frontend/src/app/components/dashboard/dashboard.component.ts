import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { ApplicationInsightsService } from '../../services/application-insights.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  userName: string = '';

  constructor(
    private authService: MsalService,
    private router: Router,
    private appInsightsService: ApplicationInsightsService
  ) {}

  ngOnInit(): void {
    this.appInsightsService.logPageView('Dashboard');
    const accounts = this.authService.instance.getAllAccounts();
    if (accounts.length > 0) {
      this.userName = accounts[0].name || accounts[0].username;
    }
  }

  navigateToStudentForm(): void {
    this.appInsightsService.logEvent('NavigateToStudentForm');
    this.router.navigate(['/student-trf-details']);
  }

  logout(): void {
    this.appInsightsService.logEvent('UserLogout');
    this.authService.logoutRedirect();
  }
}
