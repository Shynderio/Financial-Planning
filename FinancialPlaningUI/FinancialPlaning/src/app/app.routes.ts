import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { TermsComponent } from './pages/terms/terms.component';
import { CreateTermComponent } from './pages/terms/create-term/create-term.component';
import { EditTermComponent } from './pages/terms/edit-term/edit-term.component';
import { TermDetailsComponent } from './pages/terms/term-details/term-details.component';
import { ListReportComponent } from './pages/report/list-report/list-report.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { AuthGuard } from './services/auth/auth.guard';
import { AccountanGuard } from './services/auth/accountan.guard';
import { PlansComponent } from './pages/plans/plans.component';

export const routes: Routes = [
    { path:'',redirectTo:'login',pathMatch:'full'},
    { path: 'login', component: LoginComponent},
    { path: '', component: SidenavComponent, canActivate: [AuthGuard]},
        { path: 'home', component: HomeComponent, canActivate: [AuthGuard]},
        { path: 'terms', component: TermsComponent, canActivate: [AuthGuard]},
        { path: 'create-term', component: CreateTermComponent, canActivate: [ AccountanGuard] },
        { path: 'edit-term/:id', component: EditTermComponent, canActivate: [AccountanGuard] },
        { path: 'term-details/:id', component: TermDetailsComponent, canActivate: [AccountanGuard] },
        { path: 'reports', component: ListReportComponent , canActivate: [AuthGuard]},
        { path: 'plans', component: PlansComponent , canActivate: [AuthGuard]},
   

   
];
