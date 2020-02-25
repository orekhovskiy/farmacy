import { Component, OnInit, Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import * as $ from 'jquery';

import Medicine = models.Medicine;
import MedicineList = models.MedicineList;
import OptionSet = models.OptionSet;
import { MedicineListService } from './medicinelist.service';

@Component({
  selector: 'app-medicinelist',
  templateUrl: './medicinelist.component.html',
  styleUrls: ['./medicinelist.component.css'],
  providers: [MedicineListService]
})

@Injectable()
export class MedicinelistComponent implements OnInit {
  private optionSet:OptionSet[];
  private medicines: Medicine[];
  private currentPage:number = 1;
  private viewPart:string;
  private pagesAmount:number;
  private readonly rowsOnPage:number = 1;

  constructor(private medicineListService: MedicineListService, private router: Router, private activatedRoute: ActivatedRoute) {}

  /*testPaginator() {
    var aspirine:models.Medicine = {
      id:0,
      name: 'Аспирин',
      producer:'Bayer',
      category: "Анальгетик",
      form: "Таблетки",
      count: 45,
      medicineCompostion: ["Ацетилсалициловая кислота"],
      shelfTime:18
    }
    this.medicines = [aspirine,aspirine,aspirine,aspirine,aspirine,aspirine];
    this.pagesAmount = 7;
  }*/
  
  ngOnInit() {
    this.medicines=[];    
    if ($(document).height() <= $(window).height()) {
      $('#footer').addClass('fixed-bottom');
      $('.medicine').css('margin-bottom', '40px');
    }
    this.medicineListService.getOptionSet()
      .subscribe( (data:OptionSet[]) => this.optionSet = data);
    this.getAllMedicines();
    //this.testPaginator();
  }

  private getAllMedicines() {
    this.medicineListService.getAllMedicinePaged(this.currentPage, this.rowsOnPage)
      .subscribe( (data:MedicineList) => {
        this.currentPage = data.currentPage;
        this.pagesAmount = data.pagesAmount;
        data.medicines.forEach(element => {
          this.medicines.push(element)
        });
        this.medicines = data.medicines;
        this.viewPart = 'all';
      });
  }

  getFilteredMedicines() {
    this.medicines=[];
    this.medicineListService.getFilteredMedicinesPaged(this.currentPage,this.rowsOnPage,this.getFilterOptions())
      .subscribe( (data:MedicineList) => {
        this.currentPage = data.currentPage;
        this.pagesAmount = data.pagesAmount;
        if (data.medicines) data.medicines.forEach(element => {
          this.medicines.push(element);
        });
        this.viewPart="filter";
      });
  }

  search() {
    this.medicines=[];
    var key = $('#search').val();
    this.medicineListService.getMedicinesByKeyPaged(this.currentPage, this.rowsOnPage, key)
      .subscribe( (data:MedicineList) => {
      this.currentPage = data.currentPage;
      this.pagesAmount = data.pagesAmount;
      if (data.medicines) data.medicines.forEach(element => {
        this.medicines.push(element);
      });
      this.viewPart = 'search';
    });
  }

  loadPage(pageNumber) {
    this.currentPage = pageNumber;
    switch (this.viewPart) {
      case 'all':
        this.getAllMedicines();
        break;
      case 'filter':
        this.getFilteredMedicines();
        break;
      case 'filter':
        this.search();
        break;
    }
  }

  private getFilterOptions():string {
    var result:string[] = [];
    $.each($('input[name="producer-check"]:checked'), function () {result.push('producer=' + $(this).val())});
    $.each($('input[name="category-check"]:checked'), function () {result.push('category=' + $(this).val())});
    $.each($('input[name="form-check"]:checked'), function () {result.push('form=' + $(this).val())});
    $.each($('input[name="component-check"]:checked'), function () {result.push('component=' + $(this).val())});
    $.each($('input[name="shelfTime-check"]:checked'), function () {result.push('shelfTime=' + $(this).val())});
    $.each($('input[name="available-check"]:checked'), function () {result.push('available=' + $(this).val())});
    return result.join('&');
  }

  //down chevrone - &#8964;
  //right triangle bracket - &gt;
  changeArrow(id: string) {
    var obj = document.getElementById(id);
    if (obj.innerHTML === '&gt;' )
      obj.innerHTML = '&#x2304;';
    else
      obj.innerHTML = '>';
  }
  
  public signOut() {
    localStorage.clear();
    this.router.navigateByUrl('/');
  }
}