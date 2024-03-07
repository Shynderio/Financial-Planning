import { Routes } from '@angular/router';

import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { TermsComponent } from './pages/terms/terms.component';
import { CreateTermComponent } from './pages/terms/create-term/create-term.component';

export const routes: Routes = [
    { path:'',redirectTo:'login',pathMatch:'full'},
    { path: 'login', component: LoginComponent},
    { path: 'home', component: HomeComponent},
    { path: 'terms', component: TermsComponent},
    { path: 'create-term', component: CreateTermComponent },
];
