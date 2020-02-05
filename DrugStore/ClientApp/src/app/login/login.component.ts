import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    if ($(document).height() <= $(window).height()) { 
      $("#footer").addClass("fixed-bottom");
      $("#paginator").addClass("fixed-bottom");
    }    
  }

}
