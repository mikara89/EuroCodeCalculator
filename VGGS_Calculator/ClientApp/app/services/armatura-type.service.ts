import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class ArmaturaTypeService {

    constructor(private http: Http) { }

    getArmaturaType() {

        return this.http.get('/api/reinforcementtype')
            .map(res => res.json());
    }
    getArmaturaList() {

        return this.http.get('/api/reinforcementList')
            .map(res => res.json());
    }
}
