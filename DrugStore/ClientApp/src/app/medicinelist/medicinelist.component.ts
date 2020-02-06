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