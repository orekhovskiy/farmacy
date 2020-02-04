import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { MedicineComponent } from './medicine/medicine.component';


const routes: Routes = [
  { path:'', component: LoginComponent },
  { path:'medicine', component:MedicineComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
