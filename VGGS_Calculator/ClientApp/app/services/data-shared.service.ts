import { Http } from '@angular/http';
import { EventEmitter, Injectable } from '@angular/core';

@Injectable()
export class DataSharedService {

    onGetInfoData = new EventEmitter<any>();
    constructor(private http: Http) { }
    getInfoData() {
        this.http.post('/params', null).map(res => {
            this.onGetInfoData.emit(res.json());
        });
    }
}
