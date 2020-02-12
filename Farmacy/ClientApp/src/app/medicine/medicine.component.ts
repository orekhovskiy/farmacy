import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-medicine',
  templateUrl: './medicine.component.html',
  styleUrls: ['./medicine.component.css']
})
export class MedicineComponent implements OnInit {

  private id: any;
  private name: string;
  private producer: string;
  private form: string;
  private shelfTime: number;
  private available: number;

  private allTypes: string[];
  private currentTypes: string[];
  private availableTypes: string[];
  private allCompositions: string[];
  private currentCompositions: string[];
  private availableCompositions: string[];

  private routeSubscription: Subscription;

  constructor(private route: ActivatedRoute) { 
    this.routeSubscription = route.params.subscribe(params => this.id = params['id']);
  }

  ngOnInit() {
    if ($(document).height() <= $(window).height())
      $("#footer").addClass("fixed-bottom");
    this.allTypes = ["Анальгетик", "Жаропонижающее"];
    this.allCompositions = ["Ацетилсилациловая кислота", "Вода"];
    this.availableCompositions = this.allCompositions;
    this.availableTypes = this.allTypes;
    this.currentTypes = [];
    this.currentCompositions = [];
  }

  addType() {
    var select = document.getElementById("availableTypes");
    var type = select.options[select.selectedIndex].value;
    document.getElementById(type + "-opt").remove();
    var index = this.availableTypes.indexOf(type);
    this.availableTypes.splice(index, 1);
    this.currentTypes.splice(this.currentTypes.length, 0, type);
  }
  removeType(type: string) {
    var index = this.currentTypes.indexOf(type);
    document.getElementById(type + "-btn").remove();
    this.currentTypes.splice(index, 1);
    this.availableTypes.splice(this.availableTypes.length, 0, type);
  }
  addComp() {
    var select = document.getElementById("availableCompositions");
    var comp = select.options[select.selectedIndex].value;
    var index = this.availableCompositions.indexOf(comp);
    this.availableCompositions.splice(index, 1);
    document.getElementById(comp + "-opt").remove();
    this.currentCompositions.splice(this.currentCompositions.length,0,comp);
  }
  removeComp(comp: string) {
    var index = this.currentCompositions.indexOf(comp);
    document.getElementById(comp + "-btn").remove();
    this.currentCompositions.splice(index, 1);
    this.availableCompositions.splice(this.availableCompositions.length, 0, comp);
  }
}
