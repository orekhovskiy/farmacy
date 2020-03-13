import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import { MedicineList } from 'src/models/medicineList';

@Injectable()
export class ListService {
    constructor(private http:HttpClient, private router: Router, private activatedRoute: ActivatedRoute) { }

    apiUrl = '/api/Medicine/';

    getAllMedicinePaged(currentPage:number, rowsOnPage:number):Observable<MedicineList> {
        return this.http.get<MedicineList>(this.apiUrl + 'GetAllMedicinesPaged?' + 
            'currentPage=' + currentPage + '&rowsOnPage=' + rowsOnPage);
    }

    getFilteredMedicinesPaged(currentPage, rowsOnPage, filterOptions):Observable<MedicineList> {
        return this.http.get<MedicineList>(this.apiUrl + 'GetFilteredMedicinesPaged?' + 
            'currentPage=' + currentPage + '&rowsOnPage=' + rowsOnPage + '&' + filterOptions);
    }

    getMedicinesByKeyPaged(currentPage, rowsOnPage, key):Observable<MedicineList> {
        return this.http.get<MedicineList>(this.apiUrl + 'GetMedicinesByKeyPaged?' +
            'currentPage=' + currentPage + '&rowsOnPage=' + rowsOnPage + '&key=' + key);
    }

    sellMedicine(id:number, count:number):Observable<boolean> {
        return this.http.get<boolean>(this.apiUrl + 'SellMedicine?' +
            'id=' + id + '&count=' + count);
    }
}