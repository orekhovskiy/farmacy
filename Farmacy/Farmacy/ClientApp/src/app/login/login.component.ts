import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

@Injectable()
export class LoginComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    if ($(document).height() <= $(window).height())
      $("#footer").addClass("fixed-bottom");
  }


  async login() {
    var login = $("#login").val();
    var password = $("#password").val();
    //var isValide;
    this.http.get('/api/User/ValidateUser?login=' + login + "&password=" + password)
      .subscribe( (data: boolean) => alert(data));
  }
}
