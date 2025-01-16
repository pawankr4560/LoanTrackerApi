import { ChangeDetectorRef, Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  data : any;
  users : any;
  checked: boolean = true;
  constructor(private userService: AuthService, private cdr: ChangeDetectorRef) {}

  formGroup!: FormGroup;

  
ngOnInit() {
this.userService.getUsers().subscribe({

next : (res) => {
  this.data = res;
  this.users = this.data.data;
  this.cdr.detectChanges(); 
},

error : (err) => {
}
})

}

toggleActiveStatus(user: any) {
  user.active = !user.active;
}
}
