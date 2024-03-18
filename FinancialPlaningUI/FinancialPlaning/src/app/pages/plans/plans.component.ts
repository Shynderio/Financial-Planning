import { Component, OnInit } from '@angular/core';
import { Plan } from '../../models/planviewlist.model';
import { PlanService } from '../../services/plan.service';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from '../../app.routes';
import { AppComponent } from '../../app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [PlanService], // Đăng ký PlanService ở đây
  bootstrap: [AppComponent]
})
export class AppModule { }


@Component({
  selector: 'app-plans',
  standalone: true,
  imports: [],
  templateUrl: './plans.component.html',
  styleUrl: './plans.component.css'
})


export class PlansComponent  implements OnInit {
  plans: Plan[] = [];
  keyword = '';
  department = '';
  status = '';

  constructor(private planService: PlanService) { }

  ngOnInit(): void {
    this.loadFinancialPlans();
  }

  loadFinancialPlans(): void {
    this.planService.getFinancialPlans(this.keyword, this.department, this.status)
      .subscribe((plans: Plan[]) => {
    this.plans = plans;
      });
  }
}