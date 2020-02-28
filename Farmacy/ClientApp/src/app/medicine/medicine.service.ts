import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import ComponentSet = models.ComponentSet;
import Medicine = models.Medicine;

@Injectable()
export class MedicineService {
    constructor(private http:HttpClient, private router: Router, private activatedRoute: ActivatedRoute) { }

    private apiUrl = '/api/Medicine/';

    getParams() {
        return  this.activatedRoute.params;
    }

    getComponentSet(id?:number):Observable<ComponentSet> {
        if (id) {
            return this.http.get<ComponentSet>(this.apiUrl + 'GetComponentSet?id=' + id);
        } else {
            return this.http.get<ComponentSet>(this.apiUrl + 'GetComponentSet');
        }
    }

    getMedicineById(id:number):Observable<Medicine> {
        return this.http.get<Medicine>(this.apiUrl + 'GetMedicineById?id=' + id);
    }

    getAllMedicineProducers():Observable<string[]> {
        return this.http.get<string[]>(this.apiUrl + 'GetAllMedicineProducers');
    }

    getAllMedicineCategories():Observable<string[]> {
        return this.http.get<string[]>(this.apiUrl + 'GetAllMedicineCategories');
    }

    getAllMedicineForms():Observable<string[]> {
        return this.http.get<string[]>(this.apiUrl + 'GetAllMedicineForms');
    }

    getAllMedicineNames():Observable<string[]> {
        return this.http.get<string[]>(this.apiUrl + 'GetAllMedicineNames');
    }

    postMedicine(id:any, params:string):Observable<Object> {
        if (id == 'new') {
            return this.http.get(this.apiUrl + 'NewMedicine?' + params);
        } else {
            return this.http.get(this.apiUrl + 'AlterMedicine?id=' + id + '&' + params);
        }
    }
}