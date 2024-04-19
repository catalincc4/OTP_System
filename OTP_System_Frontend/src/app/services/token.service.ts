import {Injectable} from "@angular/core";
import {jwtDecode} from "jwt-decode";
import {TokenStorageService} from "./token-storage.service";

@Injectable({
  providedIn: "root"
})
export class TokenService{

  constructor(private readonly tokenStorageService: TokenStorageService) {
  }
  getDecodedAccessToken(): any {
    try {
      return jwtDecode(this.tokenStorageService.getToken());
    } catch(Error) {
      return null;
    }
  }
}
