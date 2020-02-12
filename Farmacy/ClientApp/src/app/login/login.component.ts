import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Weather } from '../weather';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

@Injectable()
export class LoginComponent implements OnInit {

  public weatherList: Weather;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    if ($(document).height() <= $(window).height())
      $("#footer").addClass("fixed-bottom");
    this.http.get('/api/weatherforecast')
      .subscribe((data:Weather) => this.weatherList=data);
  }
}
