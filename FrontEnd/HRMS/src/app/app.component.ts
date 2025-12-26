import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgIf, NgFor, NgClass, NgStyle, CommonModule } from '@angular/common';
import { RandomColorDirective } from './directives/random-color.directive';
import { FormsModule, FormGroup, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { ReversePipe } from './pipes/reverse.pipe';
import { EmployeesComponent } from './components/employees/employees.component';
// Decorator
@Component({
  selector: 'app-root',
  imports: [RouterOutlet,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    ReversePipe, EmployeesComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
 title = "HRMS";
}
