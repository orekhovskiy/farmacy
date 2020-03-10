import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [LoginService]
})
export class LoginComponent implements OnInit {

  constructor(private loginService: LoginService, private router: Router) { }

  ngOnInit() {
    if ($(document).height() <= $(window).height())
      $("#footer").addClass("fixed-bottom");
  }


  login() {
    var login = <string > $("#login").val();
    var password = <string> $("#password").val();
    this.loginService.validateUser(login, password)
      .subscribe( (data:ValidateUserResponce) => {
        if (data) {
          localStorage.setItem('access-token', data.access_token);
          localStorage.setItem('username', data.username);
          localStorage.setItem('role', data.role);
          this.router.navigateByUrl('/medicine-list');
        } else {
          this.router.navigateByUrl('/error?status=503&message=Сервер временно недоступен');
        }
      }, (error: HttpErrorResponse) => {
        console.log(error);
        $('#error-msg').text('Введённые логин и пароль неверны');
      });
  }
}

interface ValidateUserResponce {
  access_token:string;
  username:string;
  role:string;
}
