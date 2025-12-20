import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgIf,NgFor, NgClass, NgStyle } from '@angular/common';
import { RandomColorDirective } from './directives/random-color.directive';

// Decorator
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NgIf, NgFor, NgClass, NgStyle, RandomColorDirective],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = "Welcome To Angular From TypeScript";
  number = 52.55;
  boolean = true;
  arr = ["one", "two", "three"];

  students = [
    {id : 0 ,name : "student 1", mark : 49},
    {id : 1 ,name : "student 2", mark : 59},
    {id : 2 ,name : "student 3", mark : 79},
    {id : 3 ,name : "student 4", mark : 29},
    {id : 4 ,name : "student 5", mark : 99},
  ];

  /*
  test(x : number, y : string) : string
  {
    let num : string = "s"; // Type Script same JS but with types
    num = "LUFFY"; // You Can't in TS

    let arr : number[] = ["s",1,true] there is no random variables in TS

    return "";
  }
  */

  
} 
 