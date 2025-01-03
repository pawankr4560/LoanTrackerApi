import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  data : any;
  users : any;
  constructor(private userService: AuthService, private cdr: ChangeDetectorRef) {}

ngOnInit() {
    this.userService.getUsers().subscribe({
      next : (res) => {
        this.data = res;
        this.users = this.data.data;
        console.log(this.users);
        this.cdr.detectChanges(); // Force update
      },
      error : (err) => {
        console.log(err);
      }
    });
}
}
