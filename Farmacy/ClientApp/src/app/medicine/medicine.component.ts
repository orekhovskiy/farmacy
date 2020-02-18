import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import {Medicine, ComponentSet} from 'src/models';
import * as $ from 'jquery';

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
  private shelfTime = 1;
  private count = 0;

  private allProducers: string[];
  private allCategories: string[];
  private allForms: string[];
  private currentComponents: string[];
  private availableComponents: string[];

  constructor(private http: HttpClient, private router: Router, private activatedRoute: ActivatedRoute) { 
    this.availableComponents = [];
    this.currentComponents = [];
    activatedRoute.params.subscribe(params => this.id = params['id']);
  }

  ngOnInit() {
    if ($(document).height() <= $(window).height()) {
      $("#footer").addClass("fixed-bottom");
    }
    this.loadData();
  }

  loadData () {
    if (this.id != 'new') {
      this.http.get('/api/Medicine/GetComponentSet?id=' + this.id)
        .subscribe( (data:ComponentSet) => {
          this.currentComponents = data.currentComponents;
          this.availableComponents = data.availableComponents;
        });
      this.http.get('/api/Medicine/GetMedicineById?id=' + this.id)
        .subscribe( (data:any) =>{
          this.name = data.name;
          this.producer = data.producer.name;
          this.category = data.category.name;
          this.form = data.form.name;
          this.shelfTime = data.shelfTime;
          this.count = data.count;
        });
    } else {
      this.http.get('/api/Medicine/GetComponentSet')
        .subscribe( (data:ComponentSet) => {
          this.availableComponents = data.availableComponents;
        });
    }
    this.http.get('/api/Medicine/GetAllMedicineProducers')
      .subscribe( (data:string[]) => this.allProducers = data);
    this.http.get('/api/Medicine/GetAllMedicineCategories')
      .subscribe( (data:string[]) => this.allCategories = data);
    this.http.get('/api/Medicine/GetAllMedicineForms')
      .subscribe( (data:string[]) => this.allForms = data);
  }

  addComp(comp:string) {
    var comp = $('select#availableComponents').children('option:selected').text();
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


  //form-control is-valid
  saveChanges() {}
}
