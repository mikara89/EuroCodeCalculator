import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

@Injectable()
export class SymmReinfService {
    constructor(private http: Http) { }

    getListOfAllLines() {

        return this.http.get('/api/Symclassic')
            .map(res => res.json());
    }
}
