import {RouterModule, Routes} from '@angular/router';
import {NgModule} from "@angular/core";
import {LoginComponent} from "./components/login/login.component";
import {GeneralLayoutComponent} from "./components/general-layout/general-layout.component";
import {HomePageComponent} from "./components/home-page/home-page.component";
import {canActivateRoute} from "./services/auth-guard.service";
import {canActivateLoginPage} from "./services/login-guard.service";
import {RegisterComponent} from "./components/register/register.component";

export const routes: Routes = [
  {path:'', component: GeneralLayoutComponent, children:[
      {path:'', redirectTo:'/home', pathMatch:"full"},
      {path:'home', component:HomePageComponent, canActivate:[canActivateRoute], pathMatch: "full"},
    ]},
  {path:'login', component:LoginComponent, pathMatch: "full", canActivate:[canActivateLoginPage]},
  {path:'register', component:RegisterComponent, pathMatch: "full", canActivate:[canActivateLoginPage]},
  {path:'**', redirectTo:'/home', pathMatch:"full"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {onSameUrlNavigation:'reload', initialNavigation:'enabledBlocking'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
