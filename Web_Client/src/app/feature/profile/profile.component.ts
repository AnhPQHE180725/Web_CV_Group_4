import { Component, OnInit } from '@angular/core';
import { UserProfile } from '../../models/UserProfile';
import { UserProfileService } from '../../services/user-profile.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  imports: [FormsModule, CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  userProfile: UserProfile = {
    fullName: "",
    phoneNumber: "",
    address: "",
    description: "",
    image: "",
  };
  profileUpdated = false;
  errorMessage = "";

  constructor(private userService: UserProfileService) {}

  ngOnInit(): void {
    this.userService.getProfile().subscribe(
      (data) => {
        this.userProfile = data;
      },
      (error) => {
        console.error("Error", error);
      }
    );
  }

  updateProfile(): void {
    this.userService.updateProfile(this.userProfile).subscribe(
      () => {
        this.profileUpdated = true;
        setTimeout(() => (this.profileUpdated = false), 3000);
      },
      (error) => {
        console.error("Error", error);
      }
    );
  }

  onImageChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.userProfile.image = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }
  }
}
