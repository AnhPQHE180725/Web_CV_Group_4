import { Component, OnInit } from '@angular/core';
import { UserProfile } from '../../models/UserProfile';
import { UserProfileService } from '../../services/user-profile.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CompanyProfile } from '../../models/CompanyProfile';
import { CompanyService } from '../../services/Company.service';

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
    roleId: 0
  };
  companyProfile: CompanyProfile = {
    email: "",
    name: "",
    phoneNumber: "",
    address: "",
    description: "",
    logo: ""
  };
  profileUpdated = false;
  errorMessage = "";
  showCompanyProfile: boolean = false;

  constructor(private userService: UserProfileService, private companyService: CompanyService) {}

  ngOnInit(): void {
    this.getUserProfile();
  }
  getUserProfile(){
    this.userService.getProfile().subscribe(
      (data) => {
        this.userProfile = data;
        if(this.userProfile.roleId == 2){
          this.showCompanyProfile = true;
          this.getCompanyProfile();
        }
      },
      (error) => {
        console.error("Error", error);
      }
    );
  }

  updateProfile(): void {
    this.userService.updateProfile(this.userProfile).subscribe({
      next: (response) => {  
        if (response) {
          window.alert("Hồ sơ đã được cập nhật thành công!");
        } else {
          window.alert("Không có thay đổi nào được thực hiện!");
        }
      },
      error: (error) => {  
        console.error("Lỗi cập nhật:", error);
        window.alert("Cập nhật thành công!");
      }
    });
  }
  
  

  getCompanyProfile() {
    this.companyService.getCompanyProfile().subscribe((data: any) => {
      this.companyProfile = data;
    });
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
