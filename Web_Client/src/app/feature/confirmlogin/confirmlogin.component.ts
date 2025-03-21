import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-confirmlogin',
  imports: [],
  templateUrl: './confirmlogin.component.html',
  styleUrl: './confirmlogin.component.css'
})
export class ConfirmloginComponent {

email: string | undefined;
  role: string | undefined;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.email = this.authService.getEmailFromToken();
    this.role = this.authService.getRoleFromToken();
    console.log('Email in component:', this.email); // Debug
    console.log('Role in component:', this.role);   // Debug
  }
}
