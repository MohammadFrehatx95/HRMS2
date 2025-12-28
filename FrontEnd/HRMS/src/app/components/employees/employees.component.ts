import { Component, ElementRef, ViewChild, viewChild } from '@angular/core';
import { Employee } from '../../interfaces/employee';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';

@Component({
  selector: 'app-employees',
  imports: [CommonModule, ReactiveFormsModule, NgxPaginationModule],
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.css'
})
export class EmployeesComponent {
  @ViewChild ('closeDialog') closeDialog: ElementRef | undefined;

  paginationConfig = {
    itemsPerPage: 5,
    currentPage: 1, 
  };

  employees: Employee[] = [
  
    { id: 1, name: "Emp 1", birthdate: new Date("2000-1-1"), email: "emp1@example.com", salary: 5000, status: false, positionId: 1, positionName: "Developer", departmentId: 1, departmentName: "IT", userId: 1, managerId: null, managerName: null },
    { id: 2, name: "Emp 2", birthdate: new Date("1995-5-15"), email: "emp2@example.com", salary: 6000, status: true, positionId: 2, positionName: "Manager", departmentId: 2, departmentName: "HR", userId: 2, managerId: null, managerName: null },
    { id: 3, name: "Emp 3", birthdate: new Date("1990-8-20"), email: "emp3@example.com", salary: 7000, status: true, positionId: 3, positionName: "HR", departmentId: 1, departmentName: "IT", userId: 3, managerId: 2, managerName: "Emp 2" },
    { id: 4, name: "Emp 4", birthdate: new Date("1988-12-30"), email: "emp4@example.com", salary: 8000, status: false, positionId: 4, positionName: "Developer", departmentId: 2, departmentName: "Finance", userId: 4, managerId: 3, managerName: "Emp 3" }
  ];

  employeeForm = new FormGroup({
    name: new FormControl(null, [Validators.required]),
    birthdate: new FormControl(null),
    email: new FormControl(null),
    salary: new FormControl(null, [Validators.required]),
    status: new FormControl(false, [Validators.required]),
    positionId: new FormControl(null, [Validators.required]),
    departmentId: new FormControl(null, [Validators.required]),
    managerId: new FormControl(null),
  });

  employeesTableColumns: string[] = [
    "#",
    "Name",
    "Position",
    "birthdate",
    "Status",
    "Email",
    "Salary",
    "Department",
    "Manager"
  ];

  departments = [
    { id: null, name: "Select Department" },
    { id: 1, name: "IT" },
    { id: 2, name: "HR" },
    { id: 3, name: "Finance" }
  ];

  positions = [
    { id: null, name: "Select Position" },
    { id: 1, name: "Developer" },
    { id: 2, name: "HR" },
    { id: 3, name: "Manager" }
  ];


  managers = [
    { id: null, name: "Select Manager" },
    { id: 1, name: "Emp 1" },
    { id: 2, name: "Emp 2" },
    { id: 3, name: "Emp 3" },
    { id: 4, name: "Emp 4" }
  ];

  constructor() {
  }

  
saveEmployee() {
  let newEmployee: Employee = {
    id: ((this.employees[this.employees.length - 1]?.id ?? 0) + 1),
    name: this.employeeForm.value.name!,
    birthdate: this.employeeForm.value.birthdate || new Date(),
    email: this.employeeForm.value.email || '',
    salary: this.employeeForm.value.salary!,
    status: this.employeeForm.value.status!,
    userId: ((this.employees[this.employees.length - 1]?.userId ?? 0) + 1),
    departmentId: this.employeeForm.value.departmentId!,
    departmentName: this.departments.find(d => d.id == (this.employeeForm.value.departmentId || 0))?.name,
    managerId: this.employeeForm.value.managerId || null,
    managerName: this.managers.find(m => m.id == (this.employeeForm.value.managerId || 0))?.name,
    positionId: this.employeeForm.value.positionId!,
    positionName: this.positions.find(p => p.id == (this.employeeForm.value.positionId || 0))?.name
  };

  this.employees.push(newEmployee);
  this.closeDialog?.nativeElement.click();
  //document.getElementById('closeDialog')?.click();

}

resetForm() {
  this.employeeForm.reset(
    {status: false}
  )};

  changePage(pageNumber : number) {
    this.paginationConfig.currentPage = pageNumber;
  }
}