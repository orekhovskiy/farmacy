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

  testStoring() {
    var prod:OptionSet = {
      key: 'producer',
      name: 'Producer',
      options: ['Bayer', 'Farm']
    };
    var cat: OptionSet = {
      key: 'category',
      name: 'Category',
      options: ['Pills', 'Liquid']
    };
    this.optionSet = [ prod, cat];
  }
  
  ngOnInit() {
    this.medicines=[];    
    if ($(document).height() <= $(window).height()) {
      $('#footer').addClass('fixed-bottom');
      $('.medicine').css('margin-bottom', '40px');
    }
    this.medicineListService.getOptionSet()
      .subscribe( (data:OptionSet[]) => this.optionSet = data);
    //this.getAllMedicines();
    this.testStoring();
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
  
  storeOptions() {
    this.optionSet.forEach(opt => {
      var selected = [];
      $('input[name="' + opt.key + '-check"]:checked').each( function () {
        selected.push($(this).attr('value'));
      });
      localStorage.setItem(opt.key, JSON.stringify(selected));
    });
    var selected = [];
    $('input[name=available-check]:checked').each( function () {
      selected.push($(this).attr('value'));
    });
    localStorage.setItem('available', JSON.stringify(selected));
    localStorage.setItem('currentPage', this.currentPage.toString());
    localStorage.setItem('viewPart', this.viewPart);
  }

  restoreOptions() {
    this.currentPage = Number(localStorage.getItem('currentPage'));
    this.viewPart = localStorage.getItem('viewPart');
    this.optionSet.forEach(opt => {
      var storedOptions = JSON.parse(localStorage.getItem(opt.key));
      storedOptions.forEach(element => {
        $(':checkbox[value=' + element + ']').prop("checked","true");
      });
    });
    //available stuff tl;dt
  }

  public signOut() {
    localStorage.clear();
    this.router.navigateByUrl('/');
  }
}