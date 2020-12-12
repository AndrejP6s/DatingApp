import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(
    public authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  ngOnInit() {}

  login() {
    this.authService.login(this.model).subscribe(
      next => {
        this.toastr.success('Logged in successful');
      }, error => {
        this.toastr.error(error);
      }, () => {
        this.router.navigateByUrl('/members');
      }
    );
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    this.authService.logout();
    this.toastr.info('You have been logged out');
    this.router.navigateByUrl('/home');
  }
}
