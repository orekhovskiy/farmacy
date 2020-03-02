import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable()
export class LoginService {
    constructor (private http:HttpClient, private router: Router, private activatedRoute: ActivatedRoute) { }

    apiUrl = '/api/Login/';

    validateUser(login:string, password:string):Observable<boolean> {
        return this.http.get<boolean>('/api/Auth/ValidateUser?login=' + login + "&password=" + password)
    }
}