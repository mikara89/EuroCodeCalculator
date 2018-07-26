import { Component, HostListener, OnInit } from '@angular/core';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent implements OnInit{
    newInnerHeight: any;
    newInnerWidth: any;

    ngOnInit(): void {
        this.newInnerHeight = window.innerHeight;
        this.newInnerWidth = window.innerWidth;
    }

    @HostListener('window:resize', ['$event'])
    onResize(event:any) {
        this.newInnerHeight = event.target.innerHeight;
        this.newInnerWidth = event.target.innerWidth;
    }
}
