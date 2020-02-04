import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-medicine',
  templateUrl: './medicine.component.html',
  styleUrls: ['./medicine.component.css']
})
export class MedicineComponent implements OnInit {

  constructor(public router: Router) { }

  ngOnInit() {
  }

}
