import {Component, OnInit} from '@angular/core';
import {LoginDto} from "../../dtos/loginDto";
import {FormsModule} from "@angular/forms";
import {Router} from "@angular/router";
import {TokenStorageService} from "../../services/token-storage.service";
import {AuthService} from "../../services/auth.service";
import {take} from "rxjs";
import {HttpClientModule} from "@angular/common/http";
import {NgClass, NgIf} from "@angular/common";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule, HttpClientModule, NgClass, NgIf
  ],
  providers:[AuthService, TokenStorageService, Router],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{
  loginDto: LoginDto = {email:'', password:''}
  passwordEmpty = false;
  emailEmpty = false;
  invalidEmailFormat = false;
  errorMessage = '';
  loginFail = false;

  constructor(private readonly router: Router,
              private authService: AuthService,
              private tokenStorageService: TokenStorageService,) {
  }

  ngOnInit(): void {
    this.loginDto = {email:'', password:''}
  }
  login(){
    if(this.validateInput()){
      this.authService.login(this.loginDto).pipe(take(1)).subscribe(
          {next: data =>{
              this.tokenStorageService.saveToken(data.token);
              this.router.navigate(['/home']);
            },
            error:err => {
            this.loginFail = true;
            this.errorMessage= 'Email or password invalid'
            },
            complete: () => {}
          }
      );
    }else{
      if(this.invalidEmailFormat) {
        this.errorMessage = "Invalid email format"
      }
      if(this.emailEmpty){
        this.errorMessage = "You must enter a email"
      }
      if(this.passwordEmpty){
        this.errorMessage = "you must enter a password"
      }
    }
  }

  validateInput(){
    this.passwordEmpty = false;
    this.emailEmpty = false;
    this.invalidEmailFormat = false;
    if(this.loginDto === null){
      this.passwordEmpty = true;
      this.emailEmpty = true;
      return false;
    }
    if(this.loginDto.email === ''){
      this.emailEmpty = true;
      return false;
    }
    const emailRegex: RegExp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if(!emailRegex.test(this.loginDto.email)){
      this.invalidEmailFormat = true;
      return false;
    }
    if(this.loginDto.password === ''){
      this.passwordEmpty = true;
      return false;
    }
    return true;
  }

  navigateToRegisterPage(){
    this.router.navigate(['/register']);
  }

}
