import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class KofZaProrPravPresekaService {

    constructor(private http: Http) { }

    getList() {
        return this.http.get('/api/kofzaproracunpravougaonogpreseka')
            .map(res => res.json());
    }
}