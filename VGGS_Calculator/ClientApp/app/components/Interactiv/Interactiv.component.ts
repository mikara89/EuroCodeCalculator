import { Component, OnInit, OnDestroy } from '@angular/core';
import * as Chart from 'chart.js';
import { ChartDataSets, ChartPoint } from 'chart.js';
import { BetonClassService } from '../../services/beton-class.service';
import { ArmaturaTypeService } from '../../services/armatura-type.service';
import { InteractivService } from '../../services/interactiv.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
    selector: 'app-Interactiv',
    templateUrl: './Interactiv.component.html',
    styleUrls: ['./Interactiv.component.css']
})
export class InteractivComponent implements OnInit, OnDestroy {
    ngOnDestroy(): void {
        if (this.subs)
            this.subs.unsubscribe();
        if (this.subsExt)
            this.subsExt.unsubscribe();
    }

    chart: any = [];
    subs: Subscription;
    subsExt: Subscription;
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
    infoData: InteractivModelDetails;
    public lineChartConfig: Chart.ChartConfiguration =
        {
            data: { datasets:this.lineChartData },
    };
    

    isReady: boolean;
    isT: boolean; 
    textResult: any;
    private color: any; 
  
    betonclassList: any;
    armaturaTypeList: any;

    izracunaj: any = {
        mi: 57,
        ni: 0,
        geometry: {
            b_eff: 0,
            h_f: 0,
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
    infoMN(point: any) {
        delete this.infoData;
        this.izracunaj.m = point.x;
        this.izracunaj.n = point.y;
        this.subsExt = this.intServices
            .getExtremis(this.izracunaj)
            .subscribe((x: any) => {
                this.infoData = x as InteractivModelDetails;
            });
    }
    creatNewChart() {
        this.subs= this.intServices.getListOfAllLines(this.izracunaj).subscribe(list => {
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
    OnIsT() {
        this.isT = !this.isT;
        if (!this.isT) {
            this.izracunaj.b_eff = 0;
            this.izracunaj.h_f = 0;
        }
        this.updateChart();
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
        this.lineChartData.forEach((item, index) => {
            if (
                item.data.length == 1
                && (item.data[0] as ChartPoint).x === point.x
                && (item.data[0] as ChartPoint).y === point.y
            ) {
                this.lineChartData.splice(index, 1);
                (this.chart.chart as Chart).update();
            }
        });
        
    }

   
    updateChart() {
        
        this.intServices.getListOfAllLines(this.izracunaj)
            .subscribe(
                list => {
                    this.lineChartData[0] = 
                        {
                            label: 'Line1',
                            fill: true,
                            borderColor: 'black',
                            pointRadius: 0,
                            pointHitRadius: 0, borderWidth: 2,
                            pointHoverRadius: 0,
                            data: list

                        };
                    this.updateLine(this.lineChartData[0]);
        });
    }
    updateLine(dataset: ChartDataSets) {
        this.lineChartData[0] = dataset;
        (this.chart as Chart).update();

    }
 

    AddPointMNAndPlot() {
        this.addData(
            this.createDataSet(
                this.RandomColor(true),
                'M_Rd/N_Rd: ' + Math.round(this.izracunaj.mi * 100) / 100 + '/'
                + Math.round(this.izracunaj.ni * 100) / 100,
                [{ x: this.izracunaj.mi, y: this.izracunaj.ni }]
                , 5
            )
        ); 

    }
    addData(dataset: ChartDataSets) {
        this.lineChartData.push(dataset);
        (this.chart as Chart).update();

    }

   
}

interface Point {
    x: number;
    y: number;
}
