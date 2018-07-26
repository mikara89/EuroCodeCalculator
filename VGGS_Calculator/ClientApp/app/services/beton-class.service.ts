import{Injectable} from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class BetonClassService{ 
    constructor(private http: Http) { }

    getBetonClass() {

        return this.http.get('/api/betonclass')
            .map(res => res.json());
    }
}