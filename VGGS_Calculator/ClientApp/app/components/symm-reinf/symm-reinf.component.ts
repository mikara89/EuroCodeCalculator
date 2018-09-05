import { Component, OnInit } from '@angular/core';
import { SymmReinfService } from '../../services/symm-reinf.service';
import { NgxChartsModule } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-symm-reinf',
  templateUrl: './symm-reinf.component.html',
  styleUrls: ['./symm-reinf.component.css']
})
export class SymmReinfComponent implements OnInit {
    List: Array<Array<SymmReinfModel>>=[];
    

    // options
    id = 'chart1';
    width = 600;
    height = 400;
    type = 'column2d';
    dataFormat = 'json';
    dataSource: any[] = [];;
    title = 'Angular4 FusionCharts Sample';

    constructor(private symServices: SymmReinfService) { }

    ngOnInit() {
        this.symServices.getListOfAllLines().subscribe(list => {
            this.List = list;
            console.log(this.List);
            this.sortData(this.List);
        });
    }
    sortData(list: Array<Array<SymmReinfModel>>) {

        {
            //list.forEach(x => {
            //    let item = {
            //        ω: 0.05,
            //        points: [{ 'μSd':0, 'νSd': 0}]
            //    }
            //    x.forEach(y => {
            //        item.points.push({μSd: y.μSd,νSd: y.νSd})
            //    });
            //    this.dataSource.push(item);
            //});
           
        }
    }

}

interface SymmReinfModel {
    'μSd': number;
    'νSd': number;
    'εs2': number;
    'σs1': number;
    'σs2': number;
    x: number;
}
