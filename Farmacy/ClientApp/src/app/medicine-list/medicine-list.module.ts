import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilterComponent } from './filter/filter.component';
import { ListComponent } from './list/list.component';
import { MedicineListComponent } from './medicine-list.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [MedicineListComponent, FilterComponent, ListComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: 'medicine-list', component: MedicineListComponent, children: [
          { path: 'all', component: ListComponent },
          { path: 'filter', component: ListComponent },
          { path: 'search', component: ListComponent },
          { path: '', redirectTo: 'all', pathMatch: 'full'},
          { path:'**', redirectTo: '/error?status=404&message=Страница не найдена'}
        ]
      }
    ])
  ],
  exports: [
     MedicineListComponent
  ]
})
export class MedicineListModule { }