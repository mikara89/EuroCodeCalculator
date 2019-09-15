import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
 
@Injectable()
export class InteractivService {
    constructor(private http: Http) { }

    getListOfAllLines(calcModel: any)/*: Observable<Array<InteractivModel>> */{

        return this.http.post('/api/InterMN', calcModel)
            .map(res => res.json());
    }

    getExtremis(calcModel: any) {

        return this.http.post('/api/InterMN/search', calcModel)
            .map(res => res.json());
    }

}
