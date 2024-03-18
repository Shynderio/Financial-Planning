export interface Plan {
        id: string;
        planName: string;
        term: string;
        department: string;
        status: string;
        version: number;
}
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  imports: [
    CommonModule,
    AppRoutingModule
  ],
  exports: [
    CommonModule,
    AppRoutingModule
  ]
})
export class CoreModule { }