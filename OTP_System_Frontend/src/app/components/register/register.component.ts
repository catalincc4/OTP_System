import {Component} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {RegisterDto} from "../../dtos/registerDto";
import {Router} from "@angular/router";
import {AuthService} from "../../services/auth.service";
import {TokenStorageService} from "../../services/token-storage.service";
import {HttpClientModule} from "@angular/common/http";
import {take} from "rxjs";
import {CommonModule, NgClass, NgIf} from "@angular/common";

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,CommonModule
    ],
    providers: [AuthService, TokenStorageService, Router],
    templateUrl: './register.component.html',
    styleUrl: './register.component.scss'
})
export class RegisterComponent {
    registerDto: RegisterDto = {email: '', firstName: '', password: '', lastName: ''};
    confirmPassword = '';
    passwordEmpty = false;
    emailEmpty = false;
    invalidEmailFormat = false;
    errorMessage = '';
    registerFail = false;
    firstNameEmpty = false;
    lastNameEmpty = false;
    passwordsDontMatch = false;
    invalidPasswordFormat = false;
    passwordFieldOn = false
    registrationSuccess = false

    constructor(private readonly router: Router,
                private authService: AuthService) {
    }

    register() {
        this.registerFail = false;
        if(this.validateInput()){
            this.authService.register(this.registerDto).pipe(take(1)).subscribe(
                {next: data => {
                    this.registrationSuccess = true;
                    },
                    error: err => {
                    this.registerFail = true;
                    this.errorMessage = "Registration fail. Try again"
                    },
                    complete: () => {}
                }
            )
        }
    }

    validateInput(){
        this.passwordEmpty = false;
        this.emailEmpty = false;
        this.invalidEmailFormat = false;
        this.firstNameEmpty = false;
        this.lastNameEmpty = false;
        this.passwordsDontMatch = false;
        this.invalidPasswordFormat = false;

        if(this.registerDto === null){
            this.passwordEmpty = true;
            this.emailEmpty = true;
            this.errorMessage = "Invalid inputs"
            return false;
        }

        if(this.registerDto.firstName === ''){
            this.errorMessage = "You need to provide your first name";
            this.firstNameEmpty = true;
            return false;
        }

        if(this.registerDto.lastName === ''){
            this.errorMessage = "You need to provide your last name";
            this.lastNameEmpty = true;
            return false;
        }

        if(this.registerDto.email === ''){
            this.errorMessage = 'Provide an email'
            this.emailEmpty = true;
            return false;
        }
        const emailRegex: RegExp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        if(!emailRegex.test(this.registerDto.email)){
            this.invalidEmailFormat = true;
            this.errorMessage = "Provide an valid email"
            return false;
        }
        if(this.registerDto.password === ''){
            this.passwordEmpty = true;
            this.errorMessage = "You need to enter a password"
            return false;
        }

        const passwordRegex: RegExp = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
        if(!passwordRegex.test(this.registerDto.password)){
            this.invalidPasswordFormat = true;
            this.errorMessage="Password must be at least 8 characters and it must contain number, special character, upper case letter"
            return false;
        }
        if(this.confirmPassword !== this.registerDto.password){
            this.passwordsDontMatch = true;
            this.errorMessage = "Passwords don't match";
            return false;
        }
        return true;
    }

    checkPasswords(){
        this.passwordFieldOn = true;
        if(this.registerDto.password !== this.confirmPassword){
            this.passwordsDontMatch = true;
        }else{
            this.passwordsDontMatch = false;
        }
    }
    navigateToLoginPage() {
        this.router.navigate(['/login']);
    }

}
