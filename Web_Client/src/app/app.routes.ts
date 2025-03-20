import { Routes } from '@angular/router';
import { RecruitmentListComponent } from './feature/recruitment/recruitment-list/recruitment-list.component';
import { HomepageComponent } from './feature/homepage/homepage.component';

export const routes: Routes = [
    { path: 'recruitment/category/:id', component: RecruitmentListComponent },
    { path: 'recruitment/company/:id', component: RecruitmentListComponent },
    {
        path: 'home', component: HomepageComponent
    },
];
