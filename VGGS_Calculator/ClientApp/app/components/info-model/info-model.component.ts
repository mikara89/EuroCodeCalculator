import { Component, OnInit, Input, ChangeDetectorRef } from '@angular/core';
import { InteractivService } from '../../services/interactiv.service';

@Component({
  selector: 'info-model',
  templateUrl: './info-model.component.html',
  styleUrls: ['./info-model.component.css']
})
export class InfoModelComponent implements OnInit  {


    @Input('infoData') infoData: InteractivModelDetails;
    @Input('model') model: any;
    isReady: boolean;
    selectedItem: InteractivModelItem;
    constructor(private cdr: ChangeDetectorRef) { }

    ngOnInit() {
        
    }
    selectedRow(select: any) {
        this.isReady = false;
        this.cdr.detectChanges();
        this.selectedItem = select as InteractivModelItem;
        this.isReady = true;
        this.cdr.detectChanges();
    }

}
