import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { MenuItem } from './../../models/menuItem';
import { Position } from './../../models/position';

import { Test } from './test';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {

  menuItems: MenuItem[];
  userPosition: Position;
  test: Test;

  constructor(private router: Router) { }

  ngOnInit() {
    this.menuItems = [];
    var role = localStorage.getItem('role');
    for (var pos in Position) {
      console.log (pos);
    }
  }

  public signOut() {
    localStorage.clear();
    this.router.navigateByUrl('/');
  }
}
