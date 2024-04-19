import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {TokenStorageService} from "./token-storage.service";
import {LoginDto} from "../dtos/loginDto";
import {environment} from "../../environments/environment";
import {RegisterDto} from "../dtos/registerDto";

@Injectable({
  providedIn:'root'
})
export class AuthService {
  private httpOptions = {headers:{
      'Content-Type': 'application/json'
    }};
  constructor(private http: HttpClient) {
  }
  login(loginDto: LoginDto){
    const url:string = environment.apiUrl + 'login'
    return this.http.post<any>(url, loginDto,this.httpOptions);
  }

  register(registerDto: RegisterDto){
    const url:string = environment.apiUrl + 'register'
    return this.http.post<any>(url, registerDto, this.httpOptions);
  }
}
