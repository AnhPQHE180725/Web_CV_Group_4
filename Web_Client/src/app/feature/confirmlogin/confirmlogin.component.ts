import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-confirmlogin',
  imports: [],
  templateUrl: './confirmlogin.component.html',
  styleUrl: './confirmlogin.component.css'
})
export class ConfirmloginComponent {
  userEmail: string | undefined;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.userEmail = this.authService.getToken();
  }
}
