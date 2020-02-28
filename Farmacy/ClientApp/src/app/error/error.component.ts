import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent implements OnInit {

  constructor(private router: Router, private activatedRoute: ActivatedRoute) { }

  private status:string;
  private message:string="Произошла непредвиденная ошибка";
  private isAuthorized:boolean;

  ngOnInit() {
    this.isAuthorized = localStorage.getItem('access-token') ? true : false;
    alert(this.isAuthorized);
    if ($(document).height() <= $(window).height())
      $("#footer").addClass("fixed-bottom");
    this.activatedRoute.queryParamMap.subscribe(params => {
      this.status = params.get("status");
      if (params.get("message")) {
        this.message = params.get("message");
      }
    })
  }
}