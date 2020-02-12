import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-medicinelist',
  templateUrl: './medicinelist.component.html',
  styleUrls: ['./medicinelist.component.css']
})
export class MedicinelistComponent implements OnInit {
  private aspirine: Medicine;
  private medicines: Medicine[];
  private id: any;

  constructor() {}

  ngOnInit() {
    if ($(document).height() <= $(window).height())
      $("#footer").addClass("fixed-bottom");
    this.aspirine = {
      name: 'Аспирин',
      producer:'Bayer',
      type: ["Анальгетик", "Жаропонижающее"],
      form: "Таблетки",
      available: 45,
      composition: ["Ацетилсалициловая кислота"],
      shelfTime:18
    }
    this.medicines = [this.aspirine, this.aspirine, this.aspirine, this.aspirine, this.aspirine];
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
}

interface Medicine {
  name: string;
  producer: string;
  type: string[];
  form: string;
  available: number;
  composition: string[];
  shelfTime: number;
}