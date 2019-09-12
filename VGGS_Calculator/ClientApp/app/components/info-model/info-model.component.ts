import { Component, OnInit, Input, HostListener, OnDestroy } from '@angular/core';
import { DataSharedService } from '../../services/data-shared.service';

@Component({
  selector: 'info-model',
  templateUrl: './info-model.component.html',
  styleUrls: ['./info-model.component.css']
})
export class InfoModelComponent implements OnInit {

    @Input('data')data: any;
    constructor(private dataSharedServices: DataSharedService,) { }

    ngOnInit() {
        //this.dataSharedServices
        //    .onGetInfoData
        //    .subscribe((x: any) => {
        //        this.data = x;
        //});
  }

}
