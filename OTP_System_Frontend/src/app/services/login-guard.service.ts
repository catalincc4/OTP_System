import {inject, Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, CanActivateFn, RouterStateSnapshot} from "@angular/router";
import {TokenStorageService} from "./token-storage.service";
@Injectable({
  providedIn: "root"
})
export class LoginGuardService{
  constructor(private tokenStorageService: TokenStorageService) {
  }
  canActivate(){
    return this.tokenStorageService.getToken() === ''
  }
}

export const canActivateLoginPage : CanActivateFn =
  (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    return inject(LoginGuardService).canActivate();
  };
