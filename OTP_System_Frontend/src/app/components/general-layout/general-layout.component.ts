import { Component } from '@angular/core';
import {RouterOutlet} from "@angular/router";
import {NavBarComponent} from "./nav-bar/nav-bar.component";

@Component({
  selector: 'app-general-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    NavBarComponent
  ],
  templateUrl: './general-layout.component.html',
  styleUrl: './general-layout.component.scss'
})
export class GeneralLayoutComponent {

}
