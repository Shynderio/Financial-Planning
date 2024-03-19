import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { ForgotPasswordComponent } from './pages/login/forgot-password/forgot-password.component';
import { HomeComponent } from './pages/home/home.component';
import { TermsComponent } from './pages/terms/terms.component';
import { CreateTermComponent } from './pages/terms/create-term/create-term.component';
import { EditTermComponent } from './pages/terms/edit-term/edit-term.component';
import { TermDetailsComponent } from './pages/terms/term-details/term-details.component';
import { ListReportComponent } from './pages/report/list-report/list-report.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { AuthGuard } from './services/auth/auth.guard';
import { AccountantGuard } from './services/auth/accountant.guard';
// import { UploadComponent } from './components/upload/upload.component';
import { ImportPlanComponent } from './pages/plans/import-plan/import-plan.component';
import { UserListComponent } from './pages/users/user-list/user-list.component';
import { AddNewUserComponent } from './pages/users/add-new-user/add-new-user.component';
import { PlansComponent } from './pages/plans/plans.component';

export const routes: Routes = [
    { path:'',redirectTo:'login',pathMatch:'full'},
    { path: 'login', component: LoginComponent},    
    { path: 'forgot-password', component: ForgotPasswordComponent },
    { path: '', component: SidenavComponent, canActivate: [AuthGuard]},
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard]},
    { path: 'terms', component: TermsComponent, canActivate: [AuthGuard]},
    { path: 'create-term', component: CreateTermComponent, canActivate: [ AccountantGuard] },
    { path: 'edit-term/:id', component: EditTermComponent, canActivate: [AccountantGuard] },
    { path: 'term-details/:id', component: TermDetailsComponent, canActivate: [AuthGuard] },
    { path: 'reports', component: ListReportComponent , canActivate: [AuthGuard]},
        { path: 'plans', component: PlansComponent , canActivate: [AuthGuard]},
    { path: 'import-plan', component: ImportPlanComponent, canActivate: [AuthGuard]},
    { path: 'user-list', component: UserListComponent },
    { path: 'add-user', component: AddNewUserComponent },
    { path: 'edit-user/:id', component: AddNewUserComponent },
];
