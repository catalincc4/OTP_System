import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {TokenStorageService} from "./token-storage.service";
import {LoginDto} from "../dtos/loginDto";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn:'root'
})
export class AuthService {
  constructor(private http: HttpClient, private token: TokenStorageService) {
  }
  login(loginDto: LoginDto){
    const url:string = environment.apiUrl + 'login'
    return this.http.post<any>(url, loginDto,{
      headers:{
        'Content-Type': 'application/json'
      }
    });
  }
}
