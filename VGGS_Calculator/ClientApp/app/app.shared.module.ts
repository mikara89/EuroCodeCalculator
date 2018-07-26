import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { TransverzalneSileECComponent } from './components/TransverzalneSileEC/TransverzalneSileEC.component';
import { BetonClassService } from './services/beton-class.service';
import { ArmaturaTypeService } from './services/armatura-type.service';
import { TransSileEc2IzracunajService } from './services/trans-sile-ec2-izracunaj.service';
import { VitkostEc2Component } from './components/vitkost-ec2/vitkost-ec2.component';
import { SavijanjePravougaonogPresekaEc2Component } from './components/savijanje-pravougaonog-preseka-ec2/savijanje-pravougaonog-preseka-ec2.component'; 
import { KofZaProrPravPresekaService } from './services/kof-za-pror-prav-preseka.service';
import { SavPravPresekaEc2Service } from './services/sav-prav-preseka-ec2.service';
import { LoaderRotatingDotsComponent } from './components/loader-rotating-dots/loader-rotating-dots.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent, 
        CounterComponent,
        FetchDataComponent,
        HomeComponent,
        TransverzalneSileECComponent,
        VitkostEc2Component,
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
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        BetonClassService,
        ArmaturaTypeService,
        TransSileEc2IzracunajService,
        KofZaProrPravPresekaService,
        SavPravPresekaEc2Service,
    ]
})
export class AppModuleShared {
}
