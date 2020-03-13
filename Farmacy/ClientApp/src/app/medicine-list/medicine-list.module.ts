import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilterComponent } from './filter/filter.component';
import { ListComponent } from './list/list.component';
import { MedicineListComponent } from './medicine-list.component';



@NgModule({
  declarations: [MedicineListComponent, FilterComponent, ListComponent],
  imports: [
    CommonModule
  ],
  exports: [
     MedicineListComponent
  ]
})
export class MedicineListModule { }