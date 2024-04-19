import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {TokenStorageService} from "./token-storage.service";
import {SendOtpDto} from "../dtos/sendOtpDto";
import {environment} from "../../environments/environment";
import {VerificationOtpDto} from "../dtos/verificationOtpDto";

@Injectable({
  providedIn: 'root'
})
export class OtpService {

  constructor(private http: HttpClient,
              private token: TokenStorageService
  ){
  }

  httpOptions = {
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + this.token.getToken()
    }
  }

  sendOtp(sendOtpDto: SendOtpDto){
    const url = environment.apiUrl + 'sendOtp'
    return this.http.post<any>(url, sendOtpDto, this.httpOptions);
  }

  verifyOtp(verificationOtpDto : VerificationOtpDto){
    const url = environment.apiUrl + 'verifyOtp'
    return this.http.post<any>(url, verificationOtpDto, this.httpOptions);
  }
}
