import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { MenuItem } from './../../models/menuItem';
import { Position } from './../../models/position';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {

  menuItems: MenuItem[];
  userPosition: Position;

  private readonly medicineListMenuItem: MenuItem = {
    name: 'Список лекарств',
    route: '/medicine-list'
  }
  private readonly medicineMenuItem: MenuItem = {
    name: 'Новое лекарство',
    route: '/medicine/new'
  }
  private readonly purchaseMenuItem: MenuItem = {
    name: 'Список изменений',
    route: '/history'
  }

  constructor(private router: Router) { }

  ngOnInit() {
    this.menuItems = [];
    this.router.events.subscribe(() => {
      var role = localStorage.getItem('role');
      if (role) {
        this.userPosition = Position[role];
        switch (Position[role]) {
          case Position.admin:
            this.menuItems = [this.medicineListMenuItem, this.medicineMenuItem, this.purchaseMenuItem];
            break;
          case Position.manager:
            this.menuItems = [this.medicineListMenuItem, this.medicineMenuItem, this.purchaseMenuItem];
            break;
          case Position.seller:
            this.menuItems = [this.medicineListMenuItem, this.purchaseMenuItem];
            break;
          default:
            this.menuItems = [];
            break;
        }
      }
      else {
        this.userPosition = null;
        this.menuItems = [];
      }
    });
  }

  isSignedIn():boolean {
    console.log(this.userPosition);
    if (this.userPosition === null) {
      return false;
    }
    else {
      return true;
    }
  }

  public signOut() {
    localStorage.clear();
    this.router.navigate(['/']);
  }
}
