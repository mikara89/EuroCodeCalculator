import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';


@Injectable()
export class VitkostService {


    constructor(private http: Http) { }
    result = {
        Slenderness: "Ukljesten sa jedne",
        k: 2,
        N: 1620,
        M_top: -38.5,
        M_bottom: 38.5,
        L: 375,
        b: 30,
        h: 30,
        d1: 4,
        armtype: "B500B",
        betonClass: "C25/30",
        result: "FAKE"
    }
    getResult() {
        return this.http.get('/api/izvijanje')
            .map(res => res.json());
    }
    postIzracunaj(object:any) {
        return this.http.post('/api/vitkost',object)
            .map(res => res.json());
    }
    postIzracunajFake(object: any) {
        return this.http.post('', object)
            .map(res => this.result);
    }

}
