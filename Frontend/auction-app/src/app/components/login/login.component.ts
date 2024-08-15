import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';  // Assume you have this service
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'] // Update this to your desired stylesheet
})
export class LoginComponent {
  loginForm: FormGroup;
  serverErrorMessage:string='';
  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router,
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.userService.authenticate(this.loginForm.value).subscribe(response => {
        localStorage.setItem('token', response.token);
        const jwtHelper = new JwtHelperService();
        const decodedToken = jwtHelper.decodeToken(response.token);
        const role = decodedToken?.role;

        if (role === 'Admin') {
          this.router.navigate(['/admin-home']);
        } else {
          this.router.navigate(['/user-home']);
        }
      }, error => {
        if (error.status === 400 && error.error && error.error.errors) {
          const validationErrors = error.error.errors;
          this.serverErrorMessage = 'The entered details are not correct.';
        } else {
          this.serverErrorMessage = 'The entered details are not correct..';
        }
      });
    }
  }
}

