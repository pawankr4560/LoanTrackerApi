import { Inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({providedIn: 'root'})

export class AuthGuard implements CanActivate {
    constructor(private router: Router, private authService: AuthService) { }
    canActivate (
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): boolean | Promise<boolean> {
        let isAuthenticated  = localStorage.getItem('jwt');
        if (!Boolean(isAuthenticated)) {
            this.router.navigate(['']);
        }
        return this.checkUserLogin(route,state);
    }

    checkUserLogin(route: ActivatedRouteSnapshot, url: any): boolean {
      const requiredRole = route.data['role'];
      const userRole = this.authService.getRole()?.toLocaleLowerCase();
    
      if (requiredRole && requiredRole.toLocaleLowerCase() !== userRole) {
        // Redirect to an unauthorized page if roles don't match
        this.router.navigate(['/error/unauthorize']);
        return false;
      }
    
      // Allow navigation if all checks pass
      return true;
    }
    
}