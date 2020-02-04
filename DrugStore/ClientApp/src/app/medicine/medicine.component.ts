import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-medicine',
  templateUrl: './medicine.component.html',
  styleUrls: ['./medicine.component.css']
})
export class MedicineComponent implements OnInit {
  public aspirine: Medicine;
  public medicines: Medicine[];

  constructor(public router: Router) { }

  ngOnInit() {
    if ($(document).height() <= $(window).height()) { 
      $("#footer").addClass("fixed-bottom");
      $("#paginator").addClass("fixed-bottom");
    }
  }

  showFilter() {
    document.getElementById("filter").style.visibility = 'visible';
    document.getElementById("signoutBtn").disabled = true;
    document.getElementById("filterBtn").disabled = true;
    document.getElementById("searchBtn").disabled = true;
    document.getElementById("searchInput").disabled = true;

    $('body,html').css("background-color","lightgrey");
    $('#paginator').css("background-color","lightgrey");
    $('#footer').css("background-color","lightgrey");
  }

  hideFilter() {
    alert(1);
    document.getElementById("filter").style.visibility = 'hidden';
    document.getElementById("signoutBtn").disabled = false;
    document.getElementById("filterBtn").disabled = false;
    document.getElementById("searchBtn").disabled = false;
    document.getElementById("searchInput").disabled = false;

    $('body,html').css("background-color","white");
    $('#paginator').css("background-color","white");
    $('#footer').css("background-color","white");
  }
  }

}

interface Medicine {
  name: string;
  producer: string;
  type: string[];
  form: string;
  available: number;
  composition: string[];
  shelftine: number;
}