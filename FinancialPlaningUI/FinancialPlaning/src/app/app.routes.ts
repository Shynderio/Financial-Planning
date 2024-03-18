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

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: '', component: SidenavComponent, canActivate: [AuthGuard] },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'terms', component: TermsComponent, canActivate: [AuthGuard] },
  {
    path: 'create-term',
    component: CreateTermComponent,
    canActivate: [AccountantGuard],
  },
  {
    path: 'edit-term/:id',
    component: EditTermComponent,
    canActivate: [AccountantGuard],
  },
  {
    path: 'term-details/:id',
    component: TermDetailsComponent,
    canActivate: [AccountantGuard],
  },
  { path: 'reports', component: ListReportComponent, canActivate: [AuthGuard] },
];
