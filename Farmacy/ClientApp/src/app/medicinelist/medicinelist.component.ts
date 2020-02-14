import { Component, OnInit, Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-medicinelist',
  templateUrl: './medicinelist.component.html',
  styleUrls: ['./medicinelist.component.css']
})

@Injectable()
export class MedicinelistComponent implements OnInit {
  private allProducers: string[];
  private allCategories: string[];
  private allForms: string[];
  private allComponents: string[];
  private allShelfTimes: string[];
  private medicines: Medicine[];
  private currentPage:number = 1;
  private pagesAmount:number;
  //this.http.get('/api/Medicine/GetPagesAmount?rowsOnPage=' + this.rowsOnPage + '&' + this.getFilterOptions()).subscribe( (data:number) => {this.pagesAmount = data; alert(this.pagesAmount);});
  private readonly rowsOnPage:number = 30;

  constructor(private http: HttpClient, private router: Router, private activatedRoute: ActivatedRoute) {}

  ngOnInit() {
    this.medicines=[];    
    if ($(document).height() <= $(window).height()) {
      $("#footer").addClass("fixed-bottom");
      $(".medicine-list").css("margin-bottom", "40px");
    }
    this.loadData();
    
  }
  //down chevrone - &#8964;
  //right triangle bracket - &gt;
  changeArrow(id: string) {
    var obj = document.getElementById(id);
    if (obj.innerHTML === "&gt;" )
      obj.innerHTML = "&#x2304;";
    else
      obj.innerHTML = ">";
  }

  getFilteredMedicines() {
    this.medicines=[];
    var query = "/api/Medicine/GetFilteredMedicines?" + this.getFilterOptions();
    this.http.get(query).subscribe( (data:Medicine[]) => data.forEach(element => 
        this.medicines.push(this.toMedicine(element))
    ));
  }

  search() {
    this.medicines=[];
    var key = $('#search').val();
    this.http.get('/api/Medicine/GetMedicinesByName?name=' + key).subscribe( (data:Medicine[]) => {
      if (data) data.forEach(element => this.medicines.push(this.toMedicine(element)))
    });
    this.http.get('/api/Medicine/GetMedicinesByProducer?producer=' + key).subscribe( (data:Medicine[]) => {
      if (data) data.forEach(element => this.medicines.push(this.toMedicine(element)))
    });
  }

  private getFilterOptions():string {
    var result:string[] = [];
    $.each($("input[name='producer-check']:checked"), function () {result.push("producer=" + $(this).val())});
    $.each($("input[name='category-check']:checked"), function () {result.push("category=" + $(this).val())});
    $.each($("input[name='form-check']:checked"), function () {result.push("form=" + $(this).val())});
    $.each($("input[name='component-check']:checked"), function () {result.push("component=" + $(this).val())});
    $.each($("input[name='shelf-time-check']:checked"), function () {result.push("shelfTime=" + $(this).val())});
    $.each($("input[name='available-check']:checked"), function () {result.push("available=" + $(this).val())});
    return result.join("&");
  }

  private loadData() {
    this.http.get('/api/Medicine/GetAllMedicineProducers')
      .subscribe( (data:string[]) => this.allProducers = data);
    this.http.get('/api/Medicine/GetAllMedicineCategories')
      .subscribe( (data:string[]) => this.allCategories = data);
    this.http.get('/api/Medicine/GetAllMedicineForms')
      .subscribe( (data:string[]) => this.allForms = data);
    this.http.get('/api/Medicine/GetAllMedicineComponents')
      .subscribe( (data:string[]) => this.allComponents = data);
    this.http.get('/api/Medicine/GetAllMedicineShelfTimes')
      .subscribe( (data:string[]) => this.allShelfTimes = data);
    this.http.get('/api/Medicine/GetAllMedicines')
      .subscribe( (data:Medicine[]) => data.forEach(element =>
        this.medicines.push(this.toMedicine(element))));
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

interface Medicine {
  id: number,
  name: string;
  producer: string;
  category: string;
  form: string;
  count: number;
  component: string[];
  shelfTime: number;
}
