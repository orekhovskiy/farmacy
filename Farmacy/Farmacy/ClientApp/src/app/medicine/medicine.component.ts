import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import {ComponentSet} from 'src/models';
import * as $ from 'jquery';

@Component({
  selector: 'app-medicine',
  templateUrl: './medicine.component.html',
  styleUrls: ['./medicine.component.css']
})
export class MedicineComponent implements OnInit {

  private id: any;
  private name = '';
  private producer= '';
  private category = '';
  private form = ''
  private shelfTime = 1;
  private count = 1;

  private allProducers: string[];
  private allCategories: string[];
  private allForms: string[];
  private currentComponents: string[];
  private availableComponents: string[];

  constructor(private http: HttpClient, private router: Router, private activatedRoute: ActivatedRoute) { 
    activatedRoute.params.subscribe(params => this.id = params['id']);
  }

  ngOnInit() {
    if ($(document).height() <= $(window).height()) {
      $('#footer').addClass('fixed-bottom');
    }
    this.loadData();
  }

  resetData() {
    this.name = '';
    this.producer= '';
    this.category = '';
    this.form = ''
    this.shelfTime = 1;
    this.count = 1;
    this.allProducers = [];
    this.allCategories = [];
    this.allForms = [];
    this.availableComponents = [];
    this.currentComponents = [];
  }

  loadData () {
    this.resetData();
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
    document.getElementById(comp + '-opt').remove();
    this.currentComponents.splice(this.currentComponents.length,0,comp);
  }

  removeComp(comp: string) {
    var index = this.currentComponents.indexOf(comp);
    document.getElementById(comp + '-btn').remove();
    this.currentComponents.splice(index, 1);
    this.availableComponents.splice(this.availableComponents.length, 0, comp);
  }

  saveChanges() {
    if (this.validateInputs()) {
      var query = this.getQuery();
      console.log(query);
      this.http.get(query).subscribe();
    }
  }

  getQuery():string {
    var query:string='/api/Medicine/';
    if (this.id == 'new') {
      query += 'NewMedicine?';
    } else {
      query += 'AlterMedicine?id=' + this.id + '&';
    }
    query += 'name=' + $('#name-input').val() + '&' +
             'producer=' + $('#producer-input').val() + '&' +
             'category=' + $('#category-input').val() + '&' +
             'form=' + $('#form-input').val() + '&' +
             this.getCompositionQuery() +
             'shelfTime=' + $('#shelfTime-input').val() + '&' +
             'count=' + $('#count-input').val();
    return query;
  }

  getCompositionQuery():string {
    var query = '';
    document.getElementsByName('component').forEach( element => {
      query += 'component=' + element.innerText.substring(0, element.innerText.length - 2) + '&';
    });
    return query;
  }

  validateInputs():boolean {
    var inputsAreValide;
    inputsAreValide = this.validateName() &
                      this.validateInputList('producer') &
                      this.validateInputList('category') &
                      this.validateInputList('form') &
                      this.validateInputNumber('shelfTime') &
                      this.validateInputNumber('count');
    return <boolean> inputsAreValide;
  }

  validateName():number {
    var name = <string> $('#name-input').val();
    if (!name || !this.isName(name)) {
      $('#name-help').text('Введите корректное название');
      $('#name-input').removeClass('is-valid');
      $('#name-input').addClass('is-invalid');
      return 0;
    } else if (name.length > 30) {
      $('#name-help').text('Длина названия не может превышать 30 символов');
      $('#name-input').removeClass('is-valid');
      $('#name-input').addClass('is-invalid');
      return 0;
    } else {
      $('#name-help').text('');
      $('#name-input').addClass('is-valid');
      $('#name-input').removeClass('is-invalid');
      return 1;
    }
  }

  validateInputList(key:string):number {
    if (!$('#' + key + '-input').val() || this.getOptionsByKey(key).indexOf(<string> $('#' + key + '-input').val()) == -1) {
      $('#' + key + '-help').text('Выберите значение из списка');
      $('#' + key + '-input').removeClass('is-valid');
      $('#' + key + '-input').addClass('is-invalid');
      return 0;
    } else {
      $('#' + key + '-help').text('');
      $('#' + key + '-input').addClass('is-valid');
      $('#' + key + '-input').removeClass('is-invalid');
      return 1;
    }
  }

  validateInputNumber(key:string):number {
    var val = <number> $('#' + key + '-input').val();
    if ( val > 0 && this.isInteger(val)) {
      $('#' + key + '-help').text('');
      $('#' + key + '-input').addClass('is-valid');
      $('#' + key + '-input').removeClass('is-invalid');
      return 1;
      
    } else {
      $('#' + key + '-help').text('Введите целое значение большее ноля');
      $('#' + key + '-input').removeClass('is-valid');
      $('#' + key + '-input').addClass('is-invalid');
      return 0;
    }
  }

  getOptionsByKey(key:string):string[] {
    switch (key){
      case 'producer': {
        return this.allProducers;
      }
      case 'category': {
        return this.allCategories;
      }
      case 'form': {
        return this.allForms;
      }
    }
  }

  isInteger(v):boolean {
    var num = /^-?[0-9]+$/;
    return num.test(v);
  }

  isName(n):boolean {
    var name = /^[a-zA-Zа-яА-Я]+(([',. -][a-zA-Zа-яА-Я ])?[a-zA-Zа-яА-Я]*)*$/;
    return name.test(n);
  }
}
