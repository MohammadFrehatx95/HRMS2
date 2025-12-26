import { Component } from '@angular/core';
import { Employee } from '../../interfaces/employee';

@Component({
  selector: 'app-employees',
  imports: [],
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.css'
})
export class EmployeesComponent {
  employees : Employee[] = [
    
  ];

  employeesTableColumns : string[] =[
    "#",
    "Name",
    "Position",
    "Status",
    "Email",
    "Salary",
    "Department",
    "Manager"
  ]
  
  constructor(){
}
}

