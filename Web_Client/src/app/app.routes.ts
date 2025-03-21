import { Routes } from '@angular/router';
import { RecruitmentListComponent } from './feature/recruitment/recruitment-list/recruitment-list.component';
import { HomepageComponent } from './feature/homepage/homepage.component';
import { CompanyListComponent } from './feature/company/company-list/company-list.component';
import { SignuppageComponent } from './feature/signuppage/signuppage.component';
import { LoginpageComponent } from './feature/loginpage/loginpage.component';

export const routes: Routes = [
    { path: 'recruitment/category/:id', component: RecruitmentListComponent },
    { path: 'recruitment/company/:id', component: RecruitmentListComponent },
    { path: 'recruitment', component: RecruitmentListComponent },
    { path: 'company', component: CompanyListComponent },
    {
        path: 'home', component: HomepageComponent
    },
    {
        path:'register',component: SignuppageComponent
    },
    {
        path:'login', component: LoginpageComponent
    }
];
