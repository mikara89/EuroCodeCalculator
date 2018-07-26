import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
@Injectable()
export class SavPravPresekaEc2Service {

    constructor(private http: Http) { }

    getResult(object: any) {

        return this.http.post('/api/savPravPresEc', object)
            .map(res => res.json());
    }
}