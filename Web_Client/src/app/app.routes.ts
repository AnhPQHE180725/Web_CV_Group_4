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
import { AppliedJobsComponent } from './feature/applied-jobs/applied-jobs.component';
import { UserCompaniesComponent } from './feature/user-companies/user-companies.component';
import { CompanyDetailComponent } from './feature/company-detail/company-detail.component';

import { RecruiterEditComponent } from './feature/recruiter/recruiter-edit/recruiter-edit.component';

import { RecruitmentDetailComponent } from './feature/recruitment/recruitment-detail/recruitment-detail.component';
import { ConfirmregisterComponent } from './feature/confirmregister/confirmregister.component';



export const routes: Routes = [
    { path: 'recruitment/category/:id', component: RecruitmentListComponent },
    { path: 'recruitment/company/:id', component: RecruitmentListComponent },
    { path: 'recruitment', component: RecruitmentListComponent },
    { path: 'recruitment/detail/:id', component: RecruitmentDetailComponent },
    { path: 'company', component: CompanyListComponent },
    { path: 'company/:id', component: CompanyDetailComponent },
    { path: 'recruiter', component: RecruiterHomepageComponent },
    {
        path: 'home', component: HomepageComponent
    },
    {
        path: 'register', component: SignuppageComponent
    },
    {
        path: 'register/confirm', component: ConfirmregisterComponent
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
        path: 'reset-password/:token', component: ResetpasswordComponent
    },
    {
        path: 'favorite-jobs', component: FavoriteJobsComponent
    },
    {
        path: 'favorite-companies', component: FavoriteCompaniesComponent
    },
    {
        path: 'applied-jobs', component: AppliedJobsComponent
    },
    {
        path: 'user-companies', component: UserCompaniesComponent
    },


    {
        path: 'recruiter/edit', component: RecruiterEditComponent
    },










    { path: '**', redirectTo: 'home' },


];
