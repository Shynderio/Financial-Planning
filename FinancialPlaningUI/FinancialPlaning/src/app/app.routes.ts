import { Routes } from '@angular/router';
import { LoginComponent } from './pages/auth/login/login.component';
import { ForgotPasswordComponent } from './pages/auth/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './pages/auth/reset-password/reset-password.component';
import { HomeComponent } from './pages/home/home.component';
import { TermsComponent } from './pages/terms/terms.component';
import { CreateTermComponent } from './pages/terms/create-term/create-term.component';
import { EditTermComponent } from './pages/terms/edit-term/edit-term.component';
import { TermDetailsComponent } from './pages/terms/term-details/term-details.component';
import { ListReportComponent } from './pages/report/list-report/list-report.component';
import { SidenavComponent } from './share/sidenav/sidenav.component';
import { AuthGuard } from './services/auth/auth.guard';
import { AccountantGuard } from './services/auth/accountant.guard';
import { ImportPlanComponent } from './pages/plans/import-plan/import-plan.component';
import { UserListComponent } from './pages/users/user-list/user-list.component';
import { AddNewUserComponent } from './pages/users/add-new-user/add-new-user.component';
import { PlansComponent } from './pages/plans/plans.component';
import { ReportDetailsComponent } from './pages/report/report-details/report-details.component';
import { UserDetailComponent } from './pages/users/user-detail/user-detail.component';
import { ImportReportComponent } from './pages/report/import-report/import-report.component';
import { ReupReportComponent } from './pages/report/reup-report/reup-report.component';
import { ListAnnualReportsComponent } from './pages/annual-report/list-annual-reports/list-annual-reports.component';
import { AnnualReportDetailsComponent } from './pages/annual-report/annual-report-details/annual-report-details.component';
import { PlanDetailsComponent } from './pages/plans/plan-details/plan-details.component';

export const routes: Routes = [
    { path:'',redirectTo:'login',pathMatch:'full'},
    { path: 'login', component: LoginComponent},    
    { path: 'forgot-password', component: ForgotPasswordComponent },
    { path: 'reset-password', component: ResetPasswordComponent },
    { path: '', component: SidenavComponent, canActivate: [AuthGuard]},
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard]},
    // term
    { path: 'terms', component: TermsComponent, canActivate: [AuthGuard]},
    { path: 'create-term', component: CreateTermComponent, canActivate: [ AccountantGuard] },
    { path: 'edit-term/:id', component: EditTermComponent, canActivate: [AccountantGuard] },
    { path: 'term-details/:id', component: TermDetailsComponent, canActivate: [AuthGuard] },
     //report
    { path: 'reports', component: ListReportComponent , canActivate: [AuthGuard]},
    { path: 'report-details/:id', component: ReportDetailsComponent, canActivate: [AuthGuard] },
    { path: 'import-report', component: ImportReportComponent, canActivate: [AuthGuard]},
    { path: 'reup-report/:id', component: ReupReportComponent, canActivate: [AuthGuard]},
    // plan
    { path: 'plans', component: PlansComponent , canActivate: [AuthGuard]},
    { path: 'import-plan', component: ImportPlanComponent, canActivate: [AuthGuard]},
    { path: 'plan-details/:id', component: PlanDetailsComponent, canActivate: [AuthGuard] },
     //user
    { path: 'user-list', component: UserListComponent },
    { path: 'add-user', component: AddNewUserComponent },
    { path: 'edit-user/:id', component: AddNewUserComponent },
    { path: 'user-detail/:id', component: UserDetailComponent },
    //annual reports
    { path: 'annual-reports', component: ListAnnualReportsComponent },
    { path: 'annualreport-details/:year', component: AnnualReportDetailsComponent }
  
];
