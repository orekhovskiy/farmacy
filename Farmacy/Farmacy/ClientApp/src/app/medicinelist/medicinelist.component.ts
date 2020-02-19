import { Component, OnInit, Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import {Medicine, MedicineList, OptionSet} from 'src/models';
import * as $ from 'jquery';

@Component({
  selector: 'app-medicinelist',
  templateUrl: './medicinelist.component.html',
  styleUrls: ['./medicinelist.component.css']
})

@Injectable()
export class MedicinelistComponent implements OnInit {
  private optionSet:OptionSet[];
  private medicines: Medicine[];
  private currentPage:number = 1;
  private viewPart:string;
  private pagesAmount:number;
  private readonly rowsOnPage:number = 30;

  constructor(private http: HttpClient, private router: Router, private activatedRoute: ActivatedRoute) {}

  ngOnInit() {
    this.medicines=[];    
    if ($(document).height() <= $(window).height()) {
      $('#footer').addClass('fixed-bottom');
      $('.medicine').css('margin-bottom', '40px');
    }
    this.loadData();
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

  getFilteredMedicines() {
    this.medicines=[];
    var query = '/api/Medicine/GetFilteredMedicinesPaged?currentPage=' + this.currentPage + '&rowsOnPage=' + this.rowsOnPage + '&' + this.getFilterOptions();
    this.http.get(query).subscribe( (data:MedicineList) => {
      this.currentPage = data.currentPage;
      this.pagesAmount = data.pagesAmount;
      if (data.medicines) data.medicines.forEach(element => {
        this.medicines.push(this.toMedicine(element));
      });
      this.viewPart="filter";
    });
  }

  search() {
    this.medicines=[];
    var key = $('#search').val();
    this.http.get('/api/Medicine/GetMedicinesByKeyPaged?currentPage=' + this.currentPage + '&rowsOnPage=' + this.rowsOnPage + '&key=' + key).subscribe( (data:MedicineList) => {
      this.currentPage = data.currentPage;
      this.pagesAmount = data.pagesAmount;
      if (data.medicines) data.medicines.forEach(element => {
        this.medicines.push(this.toMedicine(element));
      });
      this.viewPart = 'search';
    });
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

  private loadData() {
    this.http.get('/api/Medicine/GetOptionSet')
      .subscribe( (data:OptionSet[]) => this.optionSet = data);
    this.http.get('/api/Medicine/GetAllMedicinesPaged?currentPage=' + this.currentPage + '&rowsOnPage=' + this.rowsOnPage)
      .subscribe( (data:MedicineList) => {
        this.currentPage = data.currentPage;
        this.pagesAmount = data.pagesAmount;
        data.medicines.forEach(element => {
          this.medicines.push(this.toMedicine(element))
        });
        this.medicines = data.medicines;
        this.viewPart = 'filter';
      });
  }

  private toMedicine (element:any): Medicine {
    var result = <Medicine> element;
    result.component = [];
    result.id = element.id
    result.name = element.name;
    result.producer = element.producer.name;
    result.category = element.category.name;
    result.form = element.form.name;
    result.count = element.count;
    result.shelfTime = element.shelfTime;
    element.medicineComposition.forEach(c => result.component.splice(0,0,c.component.name));
    return result;
  }
}
