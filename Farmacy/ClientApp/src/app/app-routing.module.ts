import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { MedicineComponent } from './medicine/medicine.component';
import { MedicinelistComponent } from './medicinelist/medicinelist.component';
import { ErrorComponent } from './error/error.component';


const routes: Routes = [
  { path:'', component: LoginComponent },
  { path:'medicine-list', component: MedicinelistComponent },
  { path:'medicine/:id', component: MedicineComponent},
  { path:'error', component: ErrorComponent},
  { path:'**', redirectTo: '/error?status=404&message=Страница не найдена'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
