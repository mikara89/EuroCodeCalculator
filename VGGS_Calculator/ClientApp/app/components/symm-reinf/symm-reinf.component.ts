import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { SymmReinfService } from '../../services/symm-reinf.service';
import * as Chart from 'chart.js';
import { ChartDataSets, ChartPoint } from 'chart.js';

@Component({
  selector: 'app-symm-reinf',
  templateUrl: './symm-reinf.component.html',
  styleUrls: ['./symm-reinf.component.css']
})
export class SymmReinfComponent implements OnInit {
    List: Array<Array<SymmReinfModel>>=[];
    l: any = [];
    chart: any=[];
    x: any = [];
    y: any = [];
 dataset: ChartDataSets = { 
        label: '0.3%', fill: false,
        backgroundColor: '#003300', borderColor: '#003300',
     pointRadius: 0,
        borderWidth:0.5,
        pointHitRadius: 0,
        pointHoverRadius: 0,
        data: [{ x: -0.321789022102072, y: 0.368701488056952 }, { x: 1.18932208900904, y: 0.368701488056952 }],
    }
    constructor(private symServices: SymmReinfService) { }

    ngOnInit() {
        this.symServices.getListOfAllLines().subscribe(list => {
            this.List = list;
            this.sortData(this.List);

            this.chart = new Chart('canvas', {
                type: 'line',
                data: {
                    datasets: [{
                        label: '0.05', fill: false,
                        backgroundColor: '#ff6666', borderColor: '#ff6666',
                        pointRadius: 0,
                        pointHitRadius: 0,
                        pointHoverRadius: 0, 
                        data: this.l[0]['points'],

                    }, {
                        label: '0.1', fill: false,
                            backgroundColor: '#ff6666', borderColor: '#ff6666',
                            pointRadius: 0,
                            pointHitRadius: 0,
                            pointHoverRadius: 0, 
                        data: this.l[1]['points'],

                    }, {
                        label: '0.2', fill: false,
                            backgroundColor: '#ff668c', borderColor: '#ff668c',
                            pointRadius: 0,
                            pointHitRadius: 0,
                            pointHoverRadius: 0, 
                        data: this.l[2]['points'],

                    }, {
                        label: '0.3', fill: false,
                            pointRadius: 0,
                            pointHitRadius: 0,
                            pointHoverRadius: 0, 
                            backgroundColor: '#ffd966', borderColor: '#ffd966',
                        data: this.l[3]['points'],

                    }, {
                        label: '0.4', fill: false,
                            pointRadius: 0,
                            pointHitRadius: 0,
                            pointHoverRadius: 0, 
                            backgroundColor: '#d9ff66', borderColor: '#d9ff66',
                            data: this.l[4]['points'],
                        }, {
                            label: '0.5', fill: false,
                            pointRadius: 0,
                            pointHitRadius: 0,
                            pointHoverRadius: 0,
                            backgroundColor: '#8cff66', borderColor: '#8cff66',
                            data: this.l[5]['points'],


                        }, {
                            label: '0.6', fill: false,
                            pointRadius: 0,
                            pointHitRadius: 0,
                            pointHoverRadius: 0,
                            backgroundColor: '#66ff8c', borderColor: '#66ff8c',
                            data: this.l[6]['points'],


                        }, {
                            label: '0.7', fill: false,
                            pointRadius: 0,
                            pointHitRadius: 0,
                            pointHoverRadius: 0,
                            backgroundColor: '#66ffd9', borderColor: '#66ffd9',
                            data: this.l[7]['points'],


                        }, {
                            label: '0.8', fill: false,
                            pointRadius: 0,
                            pointHitRadius: 0,
                            pointHoverRadius: 0,
                            backgroundColor: '#66d9ff', borderColor: '#66d9ff',
                            data: this.l[8]['points'],


                        }, {
                            label: '0.9', fill: false,
                            pointRadius: 0,
                            pointHitRadius: 0,
                            pointHoverRadius: 0,
                            backgroundColor: '#66b3ff', borderColor: '#66b3ff',
                            data: this.l[9]['points'],


                        }, {
                            label: '1.0', fill: false,
                            pointRadius: 0,
                            pointHitRadius: 0,
                            pointHoverRadius: 0,
                            backgroundColor: '#6666ff', borderColor: '#6666ff',
                            data: this.l[10]['points'],


                        }, this.dataset
                    ],

                },
                options: {
                    
                    scales: {
                        xAxes: [{
                            type: 'linear',
                            display: true,
                            ticks: {
                                beginAtZero: true,
                                min:0,
                                max: 1.2,
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
                                //callback: function(label, index, labels) {
                                //    if (index === 0) {
                                //        return label; 
                                //    }
                                //},
                            },
                            
                        }]
                    }
                }
            });
            //this.addData(this.chart, this.dataset);

        });
    }
    addData(chart: any, dataset: ChartDataSets) {
        (chart.data.datasets as ChartDataSets[]).push(dataset);
        chart.update();
}

    sortData(list: Array<Array<SymmReinfModel>>) {
        {
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
           
            console.log(this.l[0]['points']); 
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
