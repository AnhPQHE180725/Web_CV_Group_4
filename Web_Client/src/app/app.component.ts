import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../core/navbar/navbar.component';
import { FooterComponent } from "../core/footer/footer.component";
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, FooterComponent,CommonModule],

  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Web_Client';
    constructor(private router: Router) {}

  get isResetPasswordPage(): boolean {
    return this.router.url.startsWith('/reset-password');
  }
}
