import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-medicinelist',
  templateUrl: './medicinelist.component.html',
  styleUrls: ['./medicinelist.component.css']
})
export class MedicinelistComponent implements OnInit {
  public aspirine: Medicine;
  public medicines: Medicine[];

  constructor() { }

  ngOnInit() {
    if ($(document).height() <= $(window).height())
      $("#footer").addClass("fixed-bottom");
  }

}

interface Medicine {
  name: string;
  producer: string;
  type: string[];
  form: string;
  available: number;
  composition: string[];
  shelftime: number;
}