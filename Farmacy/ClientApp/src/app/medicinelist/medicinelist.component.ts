import { Component, OnInit, Injectable, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, NavigationStart } from '@angular/router';
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
  private viewPart:string = 'all';
  private pagesAmount:number;
  private readonly rowsOnPage:number = 7;
  private userRole:number;

  constructor(private medicineListService: MedicineListService, private router: Router, private activatedRoute: ActivatedRoute) {}
  
  ngOnInit() {
    /*var paginatorHeight =  48 + 20;
    var tdHeight = 65;
    this.rowsOnPage = Math.floor(($(window).height() - $('#footer').height() - $('header').height() - $('thead').height() - paginatorHeight) / (tdHeight));*/
    this.setUserRole();
    this.medicines=[];
    $('#footer').addClass('fixed-bottom');
    $('.medicine').css('margin-bottom', '40px');
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
      case 'filter':
        this.search();
        break;
    }
  }

  getFilterOptions():string {
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
    localStorage.setItem('currentPage', this.currentPage.toString());
    localStorage.setItem('viewPart', this.viewPart);
  }

  restoreOptions() {
    this.currentPage = Number(localStorage.getItem('currentPage'));
    this.viewPart = localStorage.getItem('viewPart');
    localStorage.removeItem('currentPage');
    localStorage.removeItem('viewPart');
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

  signOut() {
    localStorage.clear();
    this.router.navigateByUrl('/');
  }
}

enum Position{
  admin,
  manager,
  seller
}