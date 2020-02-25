import { Injectable } from '@angular/core';
import decode from 'jwt-decode';

@Injectable()
export class AuthService {  
    public getToken(): string {
        return localStorage.getItem('access-token');
    }  
    public isAuthenticated(): boolean {
        var token = this.getToken();
        var { exp } = decode(token);
        if (Date.now() >= exp * 1000) {
            return false;
        }
        return true;
    }
}