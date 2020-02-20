import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import MedicineList = models.MedicineList;
import OptionSet = models.OptionSet;

@Injectable()
export class MedicineListService {
    constructor(private http:HttpClient, private router: Router, private activatedRoute: ActivatedRoute) { }

    private apiUrl = '/api/Medicine/';

    getOptionSet():Observable<OptionSet[]> {
        return this.http.get<OptionSet[]>(this.apiUrl + 'GetOptionSet');
    }

    getAllMedicinePaged(currentPage:number, rowsOnPage:number):Observable<MedicineList> {
        return this.http.get<MedicineList>(this.apiUrl + 'GetAllMedicinesPaged?currentPage=' + 
            currentPage + '&rowsOnPage=' + rowsOnPage);
    }

    getFilteredMedicinesPaged(currentPage, rowsOnPage, filterOptions):Observable<MedicineList> {
        return this.http.get<MedicineList>(this.apiUrl + 'GetFilteredMedicinesPaged?currentPage=' + 
            currentPage + '&rowsOnPage=' + rowsOnPage + '&' + filterOptions);
    }

    getMedicinesByKeyPaged(currentPage, rowsOnPage, key):Observable<MedicineList> {
        return this.http.get<MedicineList>(this.apiUrl + 'GetMedicinesByKeyPaged?currentPage=' + 
            currentPage + '&rowsOnPage=' + rowsOnPage + '&key=' + key);
    }
}