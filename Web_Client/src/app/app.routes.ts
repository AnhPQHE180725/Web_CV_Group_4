import { Routes } from '@angular/router';
import { RecruitmentListComponent } from './feature/recruitment/recruitment-list/recruitment-list.component';
import { HomepageComponent } from './feature/homepage/homepage.component';
import { CompanyListComponent } from './feature/company/company-list/company-list.component';
import { SignuppageComponent } from './feature/signuppage/signuppage.component';
import { LoginpageComponent } from './feature/loginpage/loginpage.component';
import { RecruiterHomepageComponent } from './feature/recruiter/recruiter-homepage/recruiter-homepage.component';

import { ConfirmloginComponent } from './feature/confirmlogin/confirmlogin.component';

import { RecruiterCandidateListComponent } from './feature/recruiter/recruiter-candidate-list/recruiter-candidate-list.component';

import { ApplyCVComponent } from './feature/recruiter/apply-cv/apply-cv.component';

import { UploadCvComponent } from './feature/upload-cv/upload-cv.component';

import { ProfileComponent } from './feature/profile/profile.component';
import { ForgotpasswordComponent } from './feature/forgotpassword/forgotpassword.component';
import { ResetpasswordComponent } from './feature/resetpassword/resetpassword.component';
import { FavoriteJobsComponent } from './components/favorite-jobs/favorite-jobs.component';
import { FavoriteCompaniesComponent } from './components/favorite-companies/favorite-companies.component';


export const routes: Routes = [
    { path: 'recruitment/category/:id', component: RecruitmentListComponent },
    { path: 'recruitment/company/:id', component: RecruitmentListComponent },
    { path: 'recruitment', component: RecruitmentListComponent },
    { path: 'company', component: CompanyListComponent },
    { path: 'recruiter', component: RecruiterHomepageComponent },
    {
        path: 'home', component: HomepageComponent
    },
    {
        path: 'register', component: SignuppageComponent
    },
    {
        path: 'login', component: LoginpageComponent
    },
    {

        path: 'login/confirm', component: ConfirmloginComponent
    },
    {
        path: 'recruiter/candidate/:id', component: RecruiterCandidateListComponent
    },
    {

        path: 'recruiter/candidate/apply/:postid/:userid', component: ApplyCVComponent
    },
    {
        path: 'upload-cv', component: UploadCvComponent
    },
    {
        path: 'profile', component: ProfileComponent
    },
    {
        path: 'forgot-password', component: ForgotpasswordComponent
    },
    {
        path: 'reset-password', component: ResetpasswordComponent
    },
    {
        path: 'favorite-jobs', component: FavoriteJobsComponent
    },
    {
        path: 'favorite-companies', component: FavoriteCompaniesComponent
    },
    { path: '**', redirectTo: 'home' }


];
