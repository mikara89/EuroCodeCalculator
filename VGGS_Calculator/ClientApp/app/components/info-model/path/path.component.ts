import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'g[path]',
  templateUrl: './path.component.html',
  styleUrls: ['./path.component.css']
})
export class PathComponent implements OnInit {
  @Input('d')d:string;
  @Input('stroke-width')stroke_width:string;
  @Input('fill')fill:string;
  constructor() { }
 
  ngOnInit() {

  }

}
