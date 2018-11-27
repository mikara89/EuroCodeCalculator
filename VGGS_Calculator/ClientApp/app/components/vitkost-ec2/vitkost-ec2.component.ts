import { Component, OnInit } from '@angular/core';
import { BetonClassService } from '../../services/beton-class.service';
import { ArmaturaTypeService } from '../../services/armatura-type.service';
import { VitkostService } from '../../services/vitkost.service';

@Component({
  selector: 'app-vitkost-ec2',
  templateUrl: './vitkost-ec2.component.html',
  styleUrls: ['./vitkost-ec2.component.css']
})
export class VitkostEc2Component implements OnInit {
    IsPageReady: boolean;
    Image: any = "Ukljesten_Sa_Jedne.jpg";
    IsCalcuating: boolean=false;
    betonclassList: any;
    SlendernessList: SlendernessType[];
    armaturaTypeList: any;
    izracunaj: any = {
        Slenderness: "Ukljesten sa jedne",
        k:2,
        N: 1620,
        M_top: -38.5,
        M_bottom: 38.5,
        L:375,
        b: 30,
        h: 30,
        d1: 4,
        armtype: "B500B",
        betonClass: "C25/30",
        result: null
    };
    validCalc: boolean = true;

    constructor(
        private betonClasService: BetonClassService,
        private vitkostService: VitkostService,
        private armaturaTypeService: ArmaturaTypeService) { }

    ngOnInit() {
        this.vitkostService.getResult().subscribe(x => {
            this.SlendernessList = x;
            this.betonClasService.getBetonClass().subscribe(beton => {
                this.betonclassList = beton;
                this.armaturaTypeService.getArmaturaType().subscribe(arm => {
                    this.armaturaTypeList = arm;
                    this.IsPageReady = true;
                });
            });
        });
    }

    reset() {
        this.izracunaj.result = null;
    }
    setK() {
        this.SlendernessList.forEach(x => {
            if (x.name == this.izracunaj.Slenderness) {
                this.izracunaj.k = x.k;
                this.Image = x.image;
            }   
        });
    }
    Izracunaj() {
        this.IsCalcuating = true;
        var v = this.vitkostService.postIzracunaj(this.izracunaj)
            .subscribe(x => {
                this.izracunaj = x;
                this.validCalc = true;
                this.IsCalcuating = false;
                v.unsubscribe();
            });
    }
}
