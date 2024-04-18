import {Component, OnInit} from '@angular/core';
import {Router, RouterOutlet} from '@angular/router';
import {TokenStorageService} from "./services/token-storage.service";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  title = 'OTP_System_Frontend';
  isLoggedIn = false;

  constructor(
      private tokenStorageService: TokenStorageService,
      private router: Router
  ) {
  }

  ngOnInit() {
    console.log(this.tokenStorageService.getToken())
    this.isLoggedIn = this.tokenStorageService.getToken() != '';
    if(!this.isLoggedIn){
      this.router.navigate(['/login']);
    }
  }
}
