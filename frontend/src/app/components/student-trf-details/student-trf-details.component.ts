import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { StudentService } from '../../services/student.service';
import { ApplicationInsightsService } from '../../services/application-insights.service';

@Component({
  selector: 'app-student-trf-details',
  templateUrl: './student-trf-details.component.html',
  styleUrls: ['./student-trf-details.component.css']
})
export class StudentTrfDetailsComponent implements OnInit {
  studentForm!: FormGroup;
  isSubmitting = false;
  submitSuccess = false;
  submitError = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private authService: MsalService,
    private router: Router,
    private appInsightsService: ApplicationInsightsService
  ) {}

  ngOnInit(): void {
    this.appInsightsService.logPageView('StudentTrfDetails');
    this.initializeForm();
  }

  initializeForm(): void {
    this.studentForm = this.fb.group({
      // Student Details Section
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      dob: ['', Validators.required],
      gender: ['', Validators.required],

      // Address 1 Section
      address1: this.fb.group({
        address1: ['', Validators.required],
        address2: [''],
        city: ['', Validators.required],
        state: ['', Validators.required],
        zipcode: ['', [Validators.required, Validators.pattern(/^\d{5}(-\d{4})?$/)]]
      }),

      // Address 2 Section
      address2: this.fb.group({
        address1: ['', Validators.required],
        address2: [''],
        city: ['', Validators.required],
        state: ['', Validators.required],
        zipcode: ['', [Validators.required, Validators.pattern(/^\d{5}(-\d{4})?$/)]]
      }),

      // Address 3 Section
      address3: this.fb.group({
        address1: ['', Validators.required],
        address2: [''],
        city: ['', Validators.required],
        state: ['', Validators.required],
        zipcode: ['', [Validators.required, Validators.pattern(/^\d{5}(-\d{4})?$/)]]
      }),

      // Address 4 Section
      address4: this.fb.group({
        address1: ['', Validators.required],
        address2: [''],
        city: ['', Validators.required],
        state: ['', Validators.required],
        zipcode: ['', [Validators.required, Validators.pattern(/^\d{5}(-\d{4})?$/)]]
      })
    });
  }

  onSubmit(): void {
    if (this.studentForm.valid) {
      this.isSubmitting = true;
      this.submitSuccess = false;
      this.submitError = false;

      const formValue = this.studentForm.value;
      this.appInsightsService.logEvent('StudentFormSubmitAttempt');

      this.studentService.saveStudent(formValue).subscribe({
        next: (response) => {
          this.isSubmitting = false;
          this.submitSuccess = true;
          this.appInsightsService.logEvent('StudentFormSubmitSuccess', { studentId: response.id });

          // Reset form after 2 seconds
          setTimeout(() => {
            this.studentForm.reset();
            this.submitSuccess = false;
          }, 2000);
        },
        error: (error) => {
          this.isSubmitting = false;
          this.submitError = true;
          this.errorMessage = error.error?.message || 'An error occurred while saving student details';
          this.appInsightsService.logException(error);
          this.appInsightsService.logEvent('StudentFormSubmitError', { error: error.message });
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.studentForm.controls).forEach(key => {
        const control = this.studentForm.get(key);
        control?.markAsTouched();

        if (control instanceof FormGroup) {
          Object.keys(control.controls).forEach(nestedKey => {
            control.get(nestedKey)?.markAsTouched();
          });
        }
      });
    }
  }

  logout(): void {
    this.appInsightsService.logEvent('UserLogout');
    this.authService.logoutRedirect();
  }

  navigateToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  get firstName() { return this.studentForm.get('firstName'); }
  get lastName() { return this.studentForm.get('lastName'); }
  get dob() { return this.studentForm.get('dob'); }
  get gender() { return this.studentForm.get('gender'); }
}
