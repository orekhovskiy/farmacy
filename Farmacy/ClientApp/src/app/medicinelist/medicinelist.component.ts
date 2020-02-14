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
  private id: any;

  constructor(private http: HttpClient, private router: Router, private activatedRoute: ActivatedRoute) {}

  ngOnInit() {
    if ($(document).height() <= $(window).height()) {
      $("#footer").addClass("fixed-bottom");
      $(".medicine-list").css("margin-bottom", "40px");
    }
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
    this.activatedRoute.queryParams.subscribe(params => {
      if (params['producer']) {
        // checkbpxes from query to scheck position
      } else {
        //all checkboxes to check position
      }
    });
    if (this.router.url )
    this.http.get('/api/Medicine/')
      .subscribe( (data:string[]) => this.allShelfTimes = data);
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
  category: string;
  form: string;
  available: number;
  composition: string[];
  shelfTime: number;
}
