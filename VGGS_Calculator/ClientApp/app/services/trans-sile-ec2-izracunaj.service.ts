import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class TransSileEc2IzracunajService {

    constructor(private http: Http) { }

    getResult(object: any) {

        return this.http.post('/api/transileec2', object)
            .map(res => res.json());
    }
}