import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as $ from 'jquery';

import { ComponentSet } from './../../models/componentSet';
import { Medicine } from './../../models/medicine';
import { MedicineService } from './medicine.service';

@Component({
  selector: 'app-medicine',
  templateUrl: './medicine.component.html',
  styleUrls: ['./medicine.component.css'],
  providers: [MedicineService]
})
export class MedicineComponent implements OnInit {
  
  constructor(private medicineService: MedicineService, private router: Router, private activatedRoute: ActivatedRoute){
  }

  id: any;
  name = '';
  producer= '';
  category = '';
  form = ''
  shelfTime = 1;
  count = 1;

  allNames:string[];
  allProducers: string[];
  allCategories: string[];
  allForms: string[];
  currentComponents: string[];
  availableComponents: string[];

  ngOnInit() {
    if ($(document).height() <= $(window).height()) {
      $('#footer').addClass('fixed-bottom');
    }
    this.medicineService.getParams().subscribe(params => {
      this.id = params['id'];
      this.loadData();
    });
  }

  resetData() {
    this.name = '';
    this.producer= '';
    this.category = '';
    this.form = ''
    this.shelfTime = 1;
    this.count = 1;
    this.allNames = [];
    this.allProducers = [];
    this.allCategories = [];
    this.allForms = [];
    this.availableComponents = [];
    this.currentComponents = [];
  }

  loadData () {
    this.resetData();
    if (this.id != 'new') {
      this.medicineService.getComponentSet(this.id)
        .subscribe( (data:ComponentSet) => {
          this.currentComponents = data.currentComponents;
          this.availableComponents = data.availableComponents;
        });
      this.medicineService.getMedicineById(this.id)
        .subscribe( (data:Medicine) =>{
          this.name = data.name;
          this.producer = data.producer;
          this.category = data.category;
          this.form = data.form;
          this.shelfTime = data.shelfTime;
          this.count = data.count;
        });
    } else {
      this.medicineService.getComponentSet()
        .subscribe( (data:ComponentSet) => {
          this.availableComponents = data.availableComponents;
        });
    }
    this.medicineService.getAllMedicineNames()
      .subscribe( (data: string[]) => this.allNames = data);
    this.medicineService.getAllMedicineProducers()
      .subscribe( (data: string[]) => this.allProducers = data);
    this.medicineService.getAllMedicineCategories()
      .subscribe( (data: string[]) =>this.allCategories = data);
    this.medicineService.getAllMedicineForms()
      .subscribe( (data: string[]) => this.allForms = data);
  }

  addComp() {
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
    $('#status').text('');
    if (this.validateInputs()) {
      var query = this.getQuery();
      console.log(query);
      this.medicineService.postMedicine(this.id, query).subscribe( (data:boolean) => {
        if (data) {
          $('#status').text('Успешно');
          $('#status').removeClass('text-danger');
          $('#status').addClass('text-success');
        } else {
          $('#status').text('Ошибка');
          $('#status').removeClass('text-success');
          $('#status').addClass('text-danger');
        }
      });
    }
  }

  getQuery():string {
    var query = '';
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
                      this.validateInputNumber('count') & 
                      this.validateComposition();
    return <boolean> inputsAreValide;
  }

  validateComposition():number {
    if (this.currentComponents.length > 0) {
      $('#composition-help').text('');
      return 1;
    } else {
      $('#composition-help').text('Состав не может отсутствовать');
    }
  }
  validateName():number {
    var name = <string> $('#name-input').val();
    if (this.allNames.includes(name) && this.name != name) {
      $('#name-help').text('Данное название уже занято');
      $('#name-input').removeClass('is-valid');
      $('#name-input').addClass('is-invalid');
      return 0;
    }
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
    var name = /^[a-zA-Zа-яА-Я]+(([,. -][a-zA-Zа-яА-Я ])?[a-zA-Zа-яА-Я]*)*$/;
    return name.test(n);
  }
}