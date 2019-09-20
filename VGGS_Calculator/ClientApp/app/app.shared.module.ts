import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { TransverzalneSileECComponent } from './components/TransverzalneSileEC/TransverzalneSileEC.component';
import { BetonClassService } from './services/beton-class.service';
import { ArmaturaTypeService } from './services/armatura-type.service';
import { TransSileEc2IzracunajService } from './services/trans-sile-ec2-izracunaj.service';
import { VitkostEc2Component } from './components/vitkost-ec2/vitkost-ec2.component';
import { SavijanjePravougaonogPresekaEc2Component } from './components/savijanje-pravougaonog-preseka-ec2/savijanje-pravougaonog-preseka-ec2.component'; 
import { KofZaProrPravPresekaService } from './services/kof-za-pror-prav-preseka.service';
import { SavPravPresekaEc2Service } from './services/sav-prav-preseka-ec2.service';
import { LoaderRotatingDotsComponent } from './components/loader-rotating-dots/loader-rotating-dots.component';
import { SymmReinfComponent } from './components/symm-reinf/symm-reinf.component';
import { SymmReinfService } from './services/symm-reinf.service';
import { VitkostService } from './services/vitkost.service';
import { InteractivComponent } from './components/Interactiv/Interactiv.component';
import { InteractivService } from './services/interactiv.service';
import { InfoModelComponent } from './components/info-model/info-model.component';
import { DataSharedService } from './services/data-shared.service';
import { EpsDisplayInfoComponent } from './components/info-model/eps-display-info/eps-display-info.component';
import { PathComponent } from './components/info-model/path/path.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent, 
        HomeComponent,
        TransverzalneSileECComponent,
        VitkostEc2Component,
        InteractivComponent,
        EpsDisplayInfoComponent,
        PathComponent,
        InfoModelComponent,
        SymmReinfComponent,
        SavijanjePravougaonogPresekaEc2Component,
        LoaderRotatingDotsComponent,
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'transverzalnesileec', component: TransverzalneSileECComponent },
            { path: 'savijanjepravresec', component: SavijanjePravougaonogPresekaEc2Component },
            { path: 'vitkostec', component: VitkostEc2Component },
            { path: 'symmReinf', component: SymmReinfComponent },
            { path: 'interactivMN', component: InteractivComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        BetonClassService,
        InteractivService,
        DataSharedService,
        ArmaturaTypeService,
        TransSileEc2IzracunajService,
        KofZaProrPravPresekaService,
        SavPravPresekaEc2Service,
        SymmReinfService,
        VitkostService
    ]
})
export class AppModuleShared {
}
