import { Component, HostListener, OnInit } from '@angular/core';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

    newInnerHeight: any;
    newInnerWidth: any;

    ngOnInit(): void {
        this.newInnerHeight = window.innerHeight;
        this.newInnerWidth = window.innerWidth;
    }

    @HostListener('window:resize', ['$event'])
    onResize(event: any) {
        this.newInnerHeight = event.target.innerHeight;
        this.newInnerWidth = event.target.innerWidth;
    }
}
