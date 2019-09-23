import { Component, OnInit } from '@angular/core';
import { BetonClassService } from '../../services/beton-class.service';
import { ArmaturaTypeService } from '../../services/armatura-type.service';
import { KofZaProrPravPresekaService } from '../../services/kof-za-pror-prav-preseka.service';
import { SavPravPresekaEc2Service } from '../../services/sav-prav-preseka-ec2.service';

@Component({
  selector: 'app-savijanje-pravougaonog-preseka-ec2',
  templateUrl: './savijanje-pravougaonog-preseka-ec2.component.html',
  styleUrls: ['./savijanje-pravougaonog-preseka-ec2.component.css']
})
export class SavijanjePravougaonogPresekaEc2Component implements OnInit {
    IsMsd: boolean;
    IsPageReady: boolean;
    IsCalcuating: boolean;
    validCalc: boolean;
    betonclassList: any;
    armaturaTypeList: any;
    armaturaList: any;
    armaturaSelected: any;
    muList: any;
    izracunaj: any = {
        b: 25,
        h: 40,
        d1: 6,
        d2: 4,
        armtype: "B500B",
        betonClass: "C25/30",
        mg: 50,
        mq: 30,
        msd: 0,
        ng: 50,
        nq: 0,
        nsd: 0,
        mu:0,
        result:null
    }
    constructor(private betonClasService: BetonClassService,
        private armaturaTypeService: ArmaturaTypeService,
        private kofServices: KofZaProrPravPresekaService,
        private savPravPresEc2Services: SavPravPresekaEc2Service
    ) { }

    ngOnInit() {
        this.betonClasService.getBetonClass().subscribe(beton => {
            this.betonclassList = beton;
            this.armaturaTypeService.getArmaturaType().subscribe(arm => {
                this.armaturaList = arm[0]; this.armaturaTypeList = arm;
                this.IsPageReady = true;
            });
        });
        this.Toggled(true);
        this.kofServices.getList({
            b: this.izracunaj.b,
            h: this.izracunaj.h,
            d1: this.izracunaj.d1,
            d2: this.izracunaj.d2,
            armtype: this.izracunaj.armtype,
            betonClass: this.izracunaj.betonClass,
        }).subscribe(kof => { this.muList = kof; });
    }
    free() {
        if (this.izracunaj.h == 0) {
            this.kofServices.getList({
                b: this.izracunaj.b,
                h: this.izracunaj.h,
                d1: this.izracunaj.d1,
                d2: this.izracunaj.d2,
                armtype: this.izracunaj.armtype,
                betonClass: this.izracunaj.betonClass,
            }).subscribe(kof => { this.muList = kof; });
        }
    }
    Toggled(b: boolean) {
        this.IsMsd = b;
        if (this.IsMsd == true) {
            this.izracunaj.mg = 0;
            this.izracunaj.mq = 0;
            this.izracunaj.msd = 112.5;
            this.izracunaj.ng = 0;
            this.izracunaj.nq = 0;
            this.izracunaj.nsd = 67.5;
        } else {
            this.izracunaj.mg =50;
            this.izracunaj.mq = 30;
            this.izracunaj.msd = 0;
            this.izracunaj.ng = 50;
            this.izracunaj.nq = 0;
            this.izracunaj.nsd = 0;
        }
    }
    Izracunaj() {
        this.IsCalcuating = true;
        this.savPravPresEc2Services.getResult(this.izracunaj)
            .subscribe(r => {
                this.izracunaj = r;              
                this.validCalc = true;
            },
            async (error: Response) => {
                var mess = (await error.json()).error;
                alert(mess);
                console.log(mess);
                this.IsCalcuating = false;
            }, () => { this.IsCalcuating = false;}
            );

    }
}
