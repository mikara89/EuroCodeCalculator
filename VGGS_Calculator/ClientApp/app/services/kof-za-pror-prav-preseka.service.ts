import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class KofZaProrPravPresekaService {

    constructor(private http: Http) { }

    getList(obj:any) {
        return this.http.post('/api/kofzaproracunpravougaonogpreseka', obj)
            .map(res => res.json());
    }
}