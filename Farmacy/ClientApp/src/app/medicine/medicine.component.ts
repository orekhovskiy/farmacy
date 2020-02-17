import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-medicine',
  templateUrl: './medicine.component.html',
  styleUrls: ['./medicine.component.css']
})
export class MedicineComponent implements OnInit {

  private id: any;
  private name = "";
  private producer= "";
  private category = "";
  private form = "";
  private component:string[] = [];
  private shelfTime = 0;
  private available = 0;

  private allProducers: string[];
  private allCategories: string[];
  private allForms: string[];
  private allComponents: string[];
  private currentComponents: string[];
  private availableComponents: string[];

  constructor(private http: HttpClient, private router: Router, private activatedRoute: ActivatedRoute) { 
    this.allComponents = [];
    this.availableComponents = [];
    this.currentComponents = [];
    activatedRoute.params.subscribe(params => this.id = params['id']);
  }

  ngOnInit() {
    if ($(document).height() <= $(window).height())
      $("#footer").addClass("fixed-bottom");
    this.loadData();
  }

  loadData () {
    if (this.id != 'new') {
      this.http.get('/api/Medicine/GetMedicineComponents?id=' + this.id)
        .subscribe( (data:string[]) => {this.currentComponents = data;});
      this.http.get('/api/Medicine/GetMedicineById?id=' + this.id)
        .subscribe( (data:Medicine) =>{
          this.name = data.name;
          this.producer = data.producer;
          this.category = data.category;
          this.form = data.form;
          this.component = data.
        });
    }
    this.http.get('/api/Medicine/GetAllMedicineComponents')
      .subscribe( (data:string[]) => this.allComponents = data);
    this.http.get('/api/Medicine/GetAllMedicineProducers')
      .subscribe( (data:string[]) => this.allProducers = data);
    this.http.get('/api/Medicine/GetAllMedicineCategories')
      .subscribe( (data:string[]) => this.allCategories = data);
    this.http.get('/api/Medicine/GetAllMedicineForms')
      .subscribe( (data:string[]) => this.allForms = data);
    this.allComponents.forEach(element => {
      if (this.currentComponents.indexOf(element) == -1) this.availableComponents.splice(0,0,element);
    });
  }

  addComp() {
    var select = document.getElementById("availableComponents");
    var comp = $("#availableComponents:selected").text();
    var index = this.availableComponents.indexOf(comp);
    this.availableComponents.splice(index, 1);
    document.getElementById(comp + "-opt").remove();
    this.currentComponents.splice(this.currentComponents.length,0,comp);
  }
  removeComp(comp: string) {
    var index = this.currentComponents.indexOf(comp);
    document.getElementById(comp + "-btn").remove();
    this.currentComponents.splice(index, 1);
    this.availableComponents.splice(this.availableComponents.length, 0, comp);
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
