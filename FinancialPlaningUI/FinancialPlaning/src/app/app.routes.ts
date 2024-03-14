import { Routes } from '@angular/router';

import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { TermsComponent } from './pages/terms/terms.component';
import { CreateTermComponent } from './pages/terms/create-term/create-term.component';
import { EditTermComponent } from './pages/terms/edit-term/edit-term.component';
import { TermDetailsComponent } from './pages/terms/term-details/term-details.component';
import { ListReportComponent } from './pages/list-report/list-report.component';

export const routes: Routes = [
    { path:'',redirectTo:'login',pathMatch:'full'},
    { path: 'login', component: LoginComponent},
    { path: 'home', component: HomeComponent},
    { path: 'terms', component: TermsComponent},
    { path: 'create-term', component: CreateTermComponent },
    { path: 'edit-term/:id', component: EditTermComponent },
    { path: 'term-details/:id', component: TermDetailsComponent },
    { path: 'reports', component: ListReportComponent },
];
