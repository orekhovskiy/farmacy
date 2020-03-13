import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';

import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import { OptionSet } from 'src/models/optionSet';

@Injectable()
export class FilterService {
    constructor(private http:HttpClient, private router: Router, private activatedRoute: ActivatedRoute) { }

    apiUrl = '/api/Medicine/';

    getOptionSet():Observable<OptionSet[]> {
        return this.http.get<OptionSet[]>(this.apiUrl + 'GetOptionSet');
    }
}