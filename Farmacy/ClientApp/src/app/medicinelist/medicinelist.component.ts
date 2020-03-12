import { Component, OnInit, Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as $ from 'jquery';

import { Medicine } from './../../models/medicine';
import { MedicineList } from './../../models/medicineList';
import { OptionSet } from './../../models/optionSet';
import { MedicineListService } from './medicinelist.service';
import { Position } from './../../models/position';
//import Position = models.Position;

@Component({
  selector: 'app-medicinelist',
  templateUrl: './medicinelist.component.html',
  styleUrls: ['./medicinelist.component.css'],
  providers: [MedicineListService]
})

@Injectable()
export class MedicinelistComponent implements OnInit {
  optionSet:OptionSet[];
  selectedOptionSet:OptionSet[];
  medicines: Medicine[];
  currentPage:number = 1;
  viewPart:string = 'all';
  pagesAmount:number;
  rowsOnPage:number = 7;
  userRole:number;
  isRestore: boolean = false;

  constructor(private medicineListService: MedicineListService, private router: Router, private activatedRoute: ActivatedRoute) {}
  
  ngOnInit() {
    this.setUserRole();
    this.medicines=[];
    this.selectedOptionSet = [];
    $('#footer').addClass('fixed-bottom');
    $('.paginator').css('padding-bottom', '40px');
    this.medicineListService.getOptionSet()
      .subscribe( (data:OptionSet[]) => {
        if (data) {
          this.optionSet = data;
          
          var available: OptionSet = {
            key:'available',
            name:'Доступно',
            options:['Да', 'Нет']
          }
          this.optionSet.push(available);
          this.optionSet.forEach(element => {
            var opt:OptionSet = {
              key: element.key,
              name: element.name,
              options: []
            };
            this.selectedOptionSet.push(opt);
          });
        }
        this.loadData();
      });
  }

  sellMedicine(id: number) {
    this.medicineListService.sellMedicine(id, 1).subscribe();
    this.loadPage(this.currentPage);
  }

  setUserRole() {
    switch (localStorage.getItem('role')) {
      case 'admin':
        this.userRole = Position.admin;
        break;
      case 'manager':
        this.userRole = Position.manager;
        break;
      case 'seller':
        this.userRole = Position.seller;
        break;
    }
  }

  hasAccess(userRole: number, minAccess: number) {
    return userRole <= minAccess ? true : false;
  }

  getPosition(level: string) {
    return Position[level];
  }

  loadData() {
    if (localStorage.getItem('currentPage')) {
      this.isRestore = true;
      this.restoreOptions();
      this.loadPage(this.currentPage);
    } else {
      $(document).ready(function() {
        $(':checkbox').prop('checked', 'true');
      });
      this.getAllMedicines();
    }
  }

  getAllMedicines() {
    this.medicineListService.getAllMedicinePaged(this.currentPage, this.rowsOnPage)
      .subscribe( (data:MedicineList) => {
        this.currentPage = data.currentPage;
        this.pagesAmount = data.pagesAmount;
        data.medicines.forEach(element => {
          this.medicines.push(element)
        });
        this.medicines = data.medicines;
      });
    this.viewPart = 'all';
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
      });
    this.viewPart="filter";
  }

  search() {
    this.medicines=[];
    var key = $('#search').val();
    this.medicineListService.getMedicinesByKeyPaged(this.currentPage, this.rowsOnPage, key)
      .subscribe((data:MedicineList) => {
        this.currentPage = data.currentPage;
        this.pagesAmount = data.pagesAmount;
        if (data.medicines) data.medicines.forEach(element => {
          this.medicines.push(element);
        });
      });
    this.viewPart = 'search';
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
      case 'search':
        this.search();
        break;
    }
  }

  setSelectedOptionSet():void {
    if (!this.isRestore) {
      this.selectedOptionSet.forEach(element => {
        element.options = [];
        $('input[name="' + element.key + '-check"]:checked').each( function () {
          element.options.push($(this).attr('value'));
        });
      });
    } else {
      this.isRestore = false;
      this.selectedOptionSet.forEach(element => {
        element.options = [];
        var storedOptions = JSON.parse(localStorage.getItem(element.key));
        storedOptions.forEach(opt => {
          element.options.push(opt);
        });
      });
    }
  }

  getFilterOptions():string {
    this.setSelectedOptionSet();
    var result:string[] = [];
    this.selectedOptionSet.forEach(element => {
      element.options.forEach(option => result.push(element.key + '=' + option));
    });
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
    localStorage.setItem('currentPage', this.currentPage.toString());
    localStorage.setItem('viewPart', this.viewPart);
    localStorage.setItem('searchKey', $('#search').val().toString());
  }

  restoreOptions() {
    this.currentPage = Number(localStorage.getItem('currentPage'));
    this.viewPart = localStorage.getItem('viewPart');
    $('#search').val(localStorage.getItem('searchKey'));
    localStorage.removeItem('currentPage');
    localStorage.removeItem('viewPart');
    localStorage.removeItem('searchKey');

    $(document).ready(function() {
      var options=['producer', 'category', 'form', 'component', 'shelfTime', 'available'];
      if (this.viewPart == 'all') {
        $(':checkbox').prop('checked', 'true');
      } else {
        options.forEach(opt => {
          var storedOptions = JSON.parse(localStorage.getItem(opt));
          storedOptions.forEach(element => {
            $(':checkbox[value="' + element + '"]').prop('checked', 'true');
          });
        });
      }
      options.forEach(opt => {localStorage.removeItem(opt)});
    });
  }

  redirect(route:string) {
    this.storeOptions();
    this.router.navigateByUrl(route);
  }
}
