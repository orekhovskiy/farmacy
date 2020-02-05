import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-medicine',
  templateUrl: './medicine.component.html',
  styleUrls: ['./medicine.component.css']
})
export class MedicineComponent implements OnInit {
  public aspirine: Medicine;
  public medicines: Medicine[];

  constructor() { }

  ngOnInit() {
    if ($(document).height() <= $(window).height()) { 
      $("#footer").addClass("fixed-bottom");
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

interface Medicine {
  name: string;
  producer: string;
  type: string[];
  form: string;
  available: number;
  composition: string[];
  shelftine: number;
}