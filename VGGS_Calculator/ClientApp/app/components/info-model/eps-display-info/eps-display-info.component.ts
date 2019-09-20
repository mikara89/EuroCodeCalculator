import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'eps-display-info',
    templateUrl: './eps-display-info.component.html',
    styleUrls: ['./eps-display-info.component.css']
})
export class EpsDisplayInfoComponent implements OnInit  {

    constructor() { }

    @Input('model') model: CalcModel;
    @Input('forces') forces: InteractivModelItem;
    diletationPath: string;
    sectionPath: string;
    dimHPath: string[];
    dimHPoint = { x: 0, y: 0 };

    dimBPath: string[];
    dimBPoint = { x: 0, y: 0 };

    transform: string;
    font: number;
    ngOnInit() {
        if (this.model) {
            

            let unit = this.model.geometry.h > this.model.geometry.b ? Number(this.model.geometry.h) + 20.0 : Number(this.model.geometry.b)+20.0;
            let scale = 50 / unit;

            let offset = 5 * 1 / scale;
            this.font = 2.5 * 1 / scale;
            console.log(1 / scale);
            this.transform = 'translate(25, 12.5)scale(' + scale + ' ' + scale+')';
            this.sectionPath = 'M0 0 h' + this.model.geometry.b / 2 + ' v' + this.model.geometry.h + ' h-' + this.model.geometry.b + ' v-' + this.model.geometry.h + ' Z';
            this.diletationPath = 'M0 0 h' + Math.round(Math.abs(this.forces.eps_c2) * 10) / 10 + ' l-' + Math.round(Math.abs(this.forces.eps_c2)*10 + Math.round(this.forces.eps_s1)*10)/10 + ' ' + this.model.geometry.h + ' h' + Math.round(this.forces.eps_s1*10)/10+'  Z';
            this.dimHPath = [];

            this.dimHPath.push('M' + (this.model.geometry.b / 2 + offset) + ' -' + offset / 2 + ' v' + (this.model.geometry.h + offset));
            this.dimHPath.push('M' + (this.model.geometry.b / 2 + offset/2) + ' 0 h' + offset);
            this.dimHPath.push('M' + (this.model.geometry.b / 2 + offset / 2) + ' ' + this.model.geometry.h + ' h'+offset);
            this.dimHPoint.x = -1 * (this.model.geometry.h / 2 + offset);
            this.dimHPoint.y = this.model.geometry.b / 2 + offset;

            this.dimBPath = [];
            this.dimBPath.push('M' + (this.model.geometry.b / 2 + offset / 2) + ' -' + offset + ' h' + -(this.model.geometry.b + offset));
            this.dimBPath.push('M' + (this.model.geometry.b / 2) + ' -' + offset / 2 + ' v-' + offset);
            this.dimBPath.push('M' + (-this.model.geometry.b / 2) + ' -' + offset / 2 + ' v-' + offset);
            this.dimBPoint.x = -offset*0.8;
            this.dimBPoint.y = -offset*1.2;
        }
    }


}
