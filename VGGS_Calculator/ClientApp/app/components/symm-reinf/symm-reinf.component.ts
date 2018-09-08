import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { SymmReinfService } from '../../services/symm-reinf.service';
import * as Chart from 'chart.js';
import { ChartDataSets, ChartPoint } from 'chart.js';
import { BetonClassService } from '../../services/beton-class.service';
import { ArmaturaTypeService } from '../../services/armatura-type.service';

@Component({
  selector: 'app-symm-reinf',
  templateUrl: './symm-reinf.component.html',
  styleUrls: ['./symm-reinf.component.css']
})
export class SymmReinfComponent implements OnInit {
    isReady: boolean=true;
    List: Array<Array<SymmReinfModel>> = [];
    private color:any;
    l: any = [];
    chart: any=[];
    x: any = [];
    y: any = [];
    betonclassList: any;
    armaturaTypeList: any;

    izracunaj: any = {
        mi: 0.5,
        ni: 0.6,
        geometry: {
            b: 25,
            h: 40,
            d1: 4,
        },
        material: {
            armtype: "B500B",
            betonClass: "C25/30",
        }
    }
     dataset1: ChartDataSets = { 
            label: '0/-0.35%', fill: false,
         backgroundColor: '#000000', borderColor: '#000000',
         pointRadius: 0, hideInLegendAndTooltip:true,
         borderWidth: 0.8,
            borderDash:[10,5],
            pointHitRadius: 0,
            pointHoverRadius: 0,
         data: [
             { x: -0.126171584531692, y: 0.188095241435375 },
             { x: 0.629384005779419, y: 1.88809516323537 }
         ],
     }
    dataset2: ChartDataSets = {
        label: '0.3/-0.35%', fill: false,
        backgroundColor: '#000000', borderColor: '#000000',
        pointRadius: 0, hideInLegendAndTooltip: true,
        borderWidth: 0.8,
        borderDash: [10, 5],
        pointHitRadius: 0,
        pointHoverRadius: 0,
        data: [
            { x: -0.321789022102072, y: 0.368701488056952 },
            { x: 1.18932208900904, y: 0.368701488056952 }
        ],
    }

    constructor(private symServices: SymmReinfService,
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
        this.symServices.getListOfAllLines(this.izracunaj).subscribe(list => {
            delete this.List;
            this.List = list;
            this.sortData(this.List);
            delete this.chart;
            this.chart = new Chart('canvas', {
                type: 'line',
                data: {
                    datasets: [{
                        label: '0.05', fill: false,
                        backgroundColor: this.RandomColor(true), borderColor: this.RandomColor(),
                        pointRadius: 0,
                        pointHitRadius: 0, borderWidth: 2,
                        pointHoverRadius: 0,
                        data: this.l[0]['points'],

                    }, {
                        label: '0.1', fill: false,
                        backgroundColor: this.RandomColor(true), borderColor: this.RandomColor(),
                        pointRadius: 0,
                        pointHitRadius: 0, borderWidth: 2,
                        pointHoverRadius: 0,
                        data: this.l[1]['points'],

                    }, {
                        label: '0.2', fill: false,
                        backgroundColor: this.RandomColor(true), borderColor: this.RandomColor(),
                        pointRadius: 0,
                        pointHitRadius: 0, borderWidth: 2,
                        pointHoverRadius: 0,
                        data: this.l[2]['points'],

                    }, {
                        label: '0.3', fill: false,
                        pointRadius: 0,
                        pointHitRadius: 0, borderWidth: 2,
                        pointHoverRadius: 0,
                        backgroundColor: this.RandomColor(true), borderColor: this.RandomColor(),
                        data: this.l[3]['points'],

                    }, {
                        label: '0.4', fill: false,
                        pointRadius: 0,
                        pointHitRadius: 0, borderWidth: 2,
                        pointHoverRadius: 0,
                        backgroundColor: this.RandomColor(true), borderColor: this.RandomColor(),
                        data: this.l[4]['points'],
                    }, {
                        label: '0.5', fill: false,
                        pointRadius: 0,
                        pointHitRadius: 0, borderWidth: 2,
                        pointHoverRadius: 0,
                        backgroundColor: this.RandomColor(true), borderColor: this.RandomColor(),
                        data: this.l[5]['points'],


                    }, {
                        label: '0.6', fill: false,
                        pointRadius: 0,
                        pointHitRadius: 0, borderWidth: 2,
                        pointHoverRadius: 0,
                        backgroundColor: this.RandomColor(true), borderColor: this.RandomColor(),
                        data: this.l[6]['points'],


                    }, {
                        label: '0.7', fill: false,
                        pointRadius: 0,
                        pointHitRadius: 0, borderWidth: 2,
                        pointHoverRadius: 0,
                        backgroundColor: this.RandomColor(true), borderColor: this.RandomColor(),
                        data: this.l[7]['points'],


                    }, {
                        label: '0.8', fill: false,
                        pointRadius: 0,
                        pointHitRadius: 0, borderWidth: 2,
                        pointHoverRadius: 0,
                        backgroundColor: this.RandomColor(true), borderColor: this.RandomColor(),
                        data: this.l[8]['points'],


                    }, {
                        label: '0.9', fill: false,
                        pointRadius: 0,
                        pointHitRadius: 0, borderWidth: 2,
                        pointHoverRadius: 0,
                        backgroundColor: this.RandomColor(true), borderColor: this.RandomColor(),
                        data: this.l[9]['points'],


                    }, {
                        label: '1.0', fill: false,
                        pointRadius: 0,
                        pointHitRadius: 0, borderWidth: 2,
                        pointHoverRadius: 0,
                        backgroundColor: this.RandomColor(true), borderColor: this.RandomColor(),
                        data: this.l[10]['points'],


                    }//, this.dataset1, this.dataset2
                    ],

                },
                options: {
                    scales: {
                        xAxes: [{
                            type: 'linear',
                            display: true,
                            ticks: {
                                beginAtZero: true,
                                min: 0,
                                max: 1.2,
                            }, scaleLabel: {
                                display: true,
                                labelString: 'μRd=MRd/(b*d^2*fcd)'
                            },
                            position: 'bottom'
                        }],
                        yAxes: [{
                            type: 'linear',
                            display: true,
                            ticks: {
                                beginAtZero: true,
                                min: 0,
                                max: 3.0,
                                autoSkip: false,
                            }, scaleLabel: {
                                display: true,
                                labelString: 'vRd=NRd/(b*d*fcd)'
                            },

                        }]
                    }
                }
            });
        });
    }
    addData(chart: any, dataset: any) {
        (chart.data.datasets as ChartDataSets[]).push(dataset);
        chart.update();
}
    RandomColor(clean: boolean = false) {
        if (clean || this.color==null)
            this.color = "rgb(" + Math.floor(Math.random() * 255)
                + "," + Math.floor(Math.random() * 255)
                + "," + Math.floor(Math.random() * 255) + ")";
        return this.color;
    }
    sortData(list: Array<Array<SymmReinfModel>>) {
        {
            this.l=[];
            list.forEach(x => {
                let item = {
                    ω: 0.05,
                    points: [{ 'x':0, 'y': 0}]
                }
                x.forEach(y => {
                        item.points.push({ x: y.μSd, y: y.νSd });
                        this.y.push(y.μSd); this.x.push(y.νSd);
                });
                item.points.splice(0, 1);
                this.l.push(item);
                
            });
        }
    }

    SearchForLine() {
       
        this.symServices.getLinesFromInput(this.izracunaj)
            .subscribe((line : any) => {
                let data = this.sortDataFromSearch(line);
                let dataset = this.createDataSet('#000000',Math.round(line.w*100)/100, data.points);
                console.log(dataset);
                this.addData(this.chart, dataset);
                /// ni to zero anmi to zero lines
                this.addData(this.chart, this.createDataSet('#000000', '0 to μSd', [{ x: this.izracunaj.mi, y: 0 }, { x: this.izracunaj.mi, y: this.izracunaj.ni }]));
                this.addData(this.chart, this.createDataSet('#000000', '0 to νSd', [{ x: 0, y: this.izracunaj.ni }, { x: this.izracunaj.mi, y: this.izracunaj.ni }]));
                this.addData(this.chart, this.createDataSet('#000000', 'w= ' + Math.round(line.w * 100) / 100, [{ x: this.izracunaj.mi, y: this.izracunaj.ni }], 2));
            },
            async (error: Response) => {
                var mess = (await error.json()).error;
                alert(mess);
                console.log(mess);
            }, () => { }
            );
    }

    sortDataFromSearch(list: any) {
        {
            let w = list.w;
            let item: DataForChart = { w: 0, points: [{ x: 0,y: 0 }] };

            list.list.forEach((y: SymmReinfModel) => {
                item.points.push({ x: y.μSd, y: y.νSd });
            });

            item.points.splice(0, 1);
            return item;
        }
    }
    createDataSet(color: any,lable:any,points:any[],pointRadius:number=0) {
        {
           let dataset: ChartDataSets = {
               label: lable, fill: false,
               backgroundColor: color, borderColor: color,
               pointRadius: pointRadius,
                borderWidth: 1,
                borderDash: [10, 5],
                pointHitRadius: 0,
                pointHoverRadius: 0,
               data: points,
            }

            return dataset;
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
interface DataForChart {
    w: number;
    points: [{
        x: number;
        y: number;
    }]
}
