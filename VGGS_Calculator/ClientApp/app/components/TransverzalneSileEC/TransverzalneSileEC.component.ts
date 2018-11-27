import { Component , OnInit} from '@angular/core';
import { BetonClassService } from '../../services/beton-class.service';
import { ArmaturaTypeService } from '../../services/armatura-type.service';
import { TransSileEc2IzracunajService } from '../../services/trans-sile-ec2-izracunaj.service';

@Component({
    styleUrls: ['./TransverzalneSileEC.component.css'],
    selector: 'TransverzalneSileEC',
    templateUrl: './TransverzalneSileEC.component.html'
})
export class TransverzalneSileECComponent implements OnInit {
    IsVed:boolean=true;
    IsPageReady: boolean;
    IsCalcuating: boolean;
    betonclassList: any;
    armaturaTypeList: any;
    armaturaList: any;
    armaturaSelected: any;
    izracunaj: any = {
        Vg: 35,
        Vq: 50,
        Ved: 0,
        b: 25,
        h: 40,
        d1: 4,
        armLongitudinal: {
            kom: 2,
            diametar: 16,
        },
        armtype: "B500B",
        betonClass: "C25/30",
        s: 0,
        m: 0,
        u_diametar: 0,
        addArm: {
            kom: 1,
            diametar: 16
        }
    };
    result: any = null;
    validCalc: boolean = true;
    errors: string;
    constructor(private betonClasService: BetonClassService,
        private armaturaTypeService: ArmaturaTypeService,
        private izracunajServices: TransSileEc2IzracunajService
    ) {

    }
    ngOnInit(): void {
        this.betonClasService.getBetonClass().subscribe(beton => {
            this.betonclassList = beton;
            this.armaturaTypeService.getArmaturaType().subscribe(arm => {
                this.armaturaList = arm[0]; this.armaturaTypeList = arm;
                this.IsPageReady = true;
            });
        });
        this.Toggled(true);
    }
    log(a: any) {
        console.log(a);
    }
    Toggled(b:boolean) {
        this.IsVed = b;
        if (this.IsVed == true) {
            this.izracunaj.Vg = 0;
            this.izracunaj.Vq = 0;
            this.izracunaj.Ved = 122.25;
        } else {
            this.izracunaj.Vg = 35;
            this.izracunaj.Vq = 50;
            this.izracunaj.Ved = 0;
        }
    }

    displayMessage(messager: any) {
        alert(messager);
    }
    Izracunaj() {

        this.IsCalcuating = true;
        this.izracunajServices.getResult(this.izracunaj)
            .subscribe(r => {
                this.result = r;
                if ((this.result.errors.length > 0)) {
                    this.validCalc = false;
                    alert(this.result.errors[0])
                } else { this.validCalc = true };
                this.error();
                this.addUsvajanje();
                
            },
            async (error: Response) => {
                var mess = (await error.json()).error;
                alert(mess);
                console.log(mess);
            }, () => { this.IsCalcuating = false;}
        );
        
    }
    error() {
        let errorstoString: string = "";
        for (let e of (this.result.errors)) {
            errorstoString = errorstoString + e + "\r\n---------------------------------------";
        }
        this.errors = errorstoString;
    }
    addUsvajanje() {
        if (this.izracunaj.addArm.kom != 0 && this.izracunaj.addArm.diametar != 0) {
            
            let arm;
            for (let fi of (this.armaturaList.listOfArmatura)) {
                if (fi['diameter'] == this.izracunaj.addArm.diametar)
                    arm = fi;
            }
            this.result.addArm_usv = arm['cm2'] * this.izracunaj.addArm.kom;
        }
    }
    minUsvajanje() {
        if (this.izracunaj.u_diametar != 0) {

            let arm;
            for (let fi of (this.armaturaList.listOfArmatura)) {
                if (fi['diameter'] == this.izracunaj.u_diametar)
                    arm = fi;
            }
            this.result.minArm_usv = arm['cm2'];
        }
    }
    reset() {
        this.izracunaj.m = 0;
        this.izracunaj.s = 0;
        this.izracunaj.u_diametar = 0;
        this.result = null;
    }

} 
