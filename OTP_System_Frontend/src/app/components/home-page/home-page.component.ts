import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {TokenService} from "../../services/token.service";
import {CountdownComponent} from "../countdown/countdown.component";
import {MatSnackBar, MatSnackBarModule} from "@angular/material/snack-bar";
import {OtpService} from "../../services/otp.service";
import {take} from "rxjs";
import {HttpClientModule} from "@angular/common/http";
import {NgClass, NgIf} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
@Component({
    selector: 'app-home-page',
    standalone: true,
    imports: [
        CountdownComponent,
        MatSnackBarModule, HttpClientModule, NgIf, ReactiveFormsModule, NgClass, FormsModule
    ],
    providers:[OtpService, TokenService],
    templateUrl: './home-page.component.html',
    styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements OnInit, AfterViewInit {
    userEmail: string = ''
    @ViewChild(CountdownComponent) countDownComponent!: CountdownComponent;
    otpValid = false;
    otp= '';
    otpVerified = false;
    startOtpVerification = false
    providedInvalidOtp = false;

    constructor(
        private tokenService: TokenService,
        private _snackBar: MatSnackBar,
        private otpService: OtpService
        ) {
    }
    sendOtp(){
        this.otpService.sendOtp({userEmail: this.userEmail}).pipe(take(1)).subscribe(
            {next: data => {
                    this.startOtpVerification = true;
                    this.providedInvalidOtp = false;
                    this.otpValid = true;
                    const message = 'Your OTP passcode is: ' + data
                    this.openSnackBar(message, 'close');
                },
            error: error =>{
                console.log("Error")
            },
                complete: () => {}
            }
        )
    }
    openSnackBar(message: string, action: string) {
        this._snackBar.open(message, action, {
            duration: 30000, // Duration in milliseconds
            horizontalPosition:'end',
            verticalPosition:'top'
        });
        setTimeout(() => {
            if (this.countDownComponent) {
                this.startCountDown(30);
            }
        }, 100);
    }

    startCountDown(duration: number) {
        this.countDownComponent.startCountdown(duration);
    }

    ngOnInit(): void {
        const token = this.tokenService.getDecodedAccessToken();
        this.userEmail = token.email;
    }

    otpExpired() {
        this.otpValid = false;
        this.startOtpVerification = false;
        this._snackBar.open("OTP Expired", 'close', {
            verticalPosition:'top',
            horizontalPosition: 'center'
        })
    }

    verifyOtp(){
        this.otpService
            .verifyOtp({userEmail: this.userEmail, userEnteredCode: this.otp})
            .pipe(take(1))
            .subscribe({
                next: data => {
                    console.log(data)
                    this._snackBar.open("OTP verified", 'close', {
                        verticalPosition:'top',
                        horizontalPosition: 'center'
                    })
                    this.startOtpVerification = false;
                    this.otpValid = false;
                },
                error: err => {this.providedInvalidOtp = true},
                complete: () => {}
            })
    }

    ngAfterViewInit(): void {

    }

}
