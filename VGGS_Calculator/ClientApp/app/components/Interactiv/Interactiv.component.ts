import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { SymmReinfService } from '../../services/symm-reinf.service';
import * as Chart from 'chart.js';
import { ChartDataSets, ChartPoint } from 'chart.js';
import { BetonClassService } from '../../services/beton-class.service';
import { ArmaturaTypeService } from '../../services/armatura-type.service';
import { InteractivService } from '../../services/interactiv.service';
import { Color, BaseChartDirective } from 'ng2-charts';

@Component({
    selector: 'app-Interactiv',
    templateUrl: './Interactiv.component.html',
    styleUrls: ['./Interactiv.component.css']
})
export class InteractivComponent implements OnInit {
    chart:any=[];
    public lineChartData: ChartDataSets[] = [];
    public lineChartOptions: Chart.ChartOptions =
        {

            responsive: true,
            scales: {
                xAxes: [{
                    type: 'linear',
                    display: true,
                    ticks: {

                        beginAtZero: false,
                    }, scaleLabel: {
                        display: true,
                        labelString: 'MRd'
                    },
                    position: 'bottom'
                }],
                yAxes: [{
                    type: 'linear',
                    display: true,
                    ticks: {
                        reverse: true,
                        beginAtZero: false,
                    }, scaleLabel: {
                        display: true,
                        labelString: 'NRd'
                    },

                }]
            }
            
        };
    public lineChartType = 'line';

    public lineChartConfig: Chart.ChartConfiguration =
        {
            data: { datasets:this.lineChartData },
    };
    

    isReady: boolean;
    textResult: any;
    pointsMN: Point[]=[];
    private color: any;

    betonclassList: any;
    armaturaTypeList: any;

    izracunaj: any = {
        mi: 150,
        ni: 0,
        geometry: {
            b: 30,
            h: 30,
            d1: 6,
            d2: 6,
            as1: 6.8,
            as2: 6.8,
        },
        material: {
            armtype: "B500B",
            betonClass: "C25/30",
        }
    }


    constructor(private intServices: InteractivService,
        private betonClasService: BetonClassService,
        private armaturaTypeService: ArmaturaTypeService,
    ) { }

    ngOnInit() {
        this.betonClasService.getBetonClass().subscribe(beton => {
            this.betonclassList = beton;
            this.armaturaTypeService.getArmaturaType().subscribe(arm => {
                this.armaturaTypeList = arm;
            });
        });
        this.creatNewChart();

    }
    creatNewChart() {
        this.intServices.getListOfAllLines(this.izracunaj).subscribe(list => {
            this.isReady = false;
            this.lineChartData = [
                {
                    label: 'Line1',
                    fill: true,
                    borderColor: 'black',
                    pointRadius: 0,
                    pointHitRadius: 0, borderWidth: 2,
                    pointHoverRadius: 0,
                    data: list

                }];
            this.lineChartConfig.options = this.lineChartOptions;
            this.lineChartConfig.type = this.lineChartType;
            this.lineChartConfig.data.datasets = this.lineChartData;

            this.chart = new Chart('canvas', this.lineChartConfig);
            this.isReady = true;
        });
        
    }
    
    
    RandomColor(clean: boolean = false) {
        if (clean || this.color==null)
            this.color = "rgb(" + Math.floor(Math.random() * 255)
                + "," + Math.floor(Math.random() * 255)
                + "," + Math.floor(Math.random() * 255) + ")";
        return this.color;
    }

 
    createDataSet(color: any,lable:any,points:any[],pointRadius:number=0) {
        {
           let dataset: ChartDataSets = {
               label: lable, fill: false,
               backgroundColor: color,
               borderColor: color,
               pointRadius: pointRadius,
                borderWidth: 1,
                borderDash: [10, 5],
               pointHitRadius: pointRadius,
               pointHoverRadius: pointRadius*1.2,
               data: points,
            }

            return dataset;
        }
    }

    removePointMNAndPlot(point: Point) {
        this.pointsMN.forEach((item, index) => {
            if (item.x === point.x && item.y === point.y)
                this.pointsMN.splice(index, 1);
        });
        this.lineChartData.forEach((item, index) => {
            if (item.data.length == 1 && (item.data[0] as ChartPoint).x === point.x && (item.data[0] as ChartPoint).y === point.y) {
                this.lineChartData.splice(index, 1);
                (this.chart as Chart).data.datasets.splice(index, 1);
                (this.chart.chart as Chart).update();
                console.log((this.chart as Chart).data.datasets);
            }
        });
        
    }

   
    updateChart() {
        this.intServices.getListOfAllLines(this.izracunaj)
            .subscribe(
                list => {
                    this.updateLine({ data: list });
        });
    }
    updateLine(dataset: ChartDataSets) {
        (this.chart as Chart).data.datasets[0] = dataset;
        (this.chart as Chart).update();

    }
 

    AddPointMNAndPlot() {
        this.pointsMN.push({ x: this.izracunaj.mi, y: this.izracunaj.ni })
        this.addData(
            this.createDataSet(
                this.RandomColor(true),
                this.pointsMN.length,
                [{ x: this.izracunaj.mi, y: this.izracunaj.ni }]
                , 5
            )
        ); 

    }
    addData(dataset: ChartDataSets) {
        (this.chart as Chart).data.datasets.push(dataset);
        (this.chart as Chart).update();

    }
   
}

interface Point {
    x: number;
    y: number;
}
 //SearchForLine() {

    //    this.intServices.getLinesFromInput(this.izracunaj)
    //        .subscribe((line: searcedPoint) => {

    //            this.addData(this.chart, this.createDataSet('#000000', '0 to μSd', [{ x: this.izracunaj.mi, y: 0 }, { x: this.izracunaj.mi, y: this.izracunaj.ni }]));
    //            this.addData(this.chart, this.createDataSet('#000000', '0 to νSd', [{ x: 0, y: this.izracunaj.ni }, { x: this.izracunaj.mi, y: this.izracunaj.ni }]));
    //            this.addData(this.chart, this.createDataSet('#000000', 'w= ' + Math.round(line.w * 100) / 100, [{ x: this.izracunaj.mi, y: this.izracunaj.ni }], 2));
    //            this.textResult = line.textResulte;
    //        },
    //        async (error: Response) => {
    //            var mess = (await error.json()).error;
    //            alert(mess);
    //            console.log(mess);
    //        }, () => { }
    //        );
    //}

    //sortDataFromSearch(list: any) {
    //    {
    //        let w = list.w;
    //        let item: DataForChart = { w: 0, points: [{ x: 0,y: 0 }] };

    //        list.list.forEach((y: SymmReinfModel) => {
    //            item.points.push({ x: y.μSd, y: y.νSd });
    //        });

    //        item.points.splice(0, 1);
    //        return item;
    //    }
    //}