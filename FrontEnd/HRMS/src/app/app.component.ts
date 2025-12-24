import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgIf, NgFor, NgClass, NgStyle, CommonModule } from '@angular/common';
import { RandomColorDirective } from './directives/random-color.directive';
import { FormsModule, FormGroup, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { ReversePipe } from './pipes/reverse.pipe';

// Decorator
@Component({
  selector: 'app-root',
  imports: [RouterOutlet,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    ReversePipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = "Welcome To Angular From TypeScript";
  price = 5252323.55;
  boolean = true;
  arr = ["one", "two", "three"];
  date = new Date();

  name: string = "Mohammad Frehat";
  age: number = 22;

  infoForm = new FormGroup({
    name: new FormControl("Mohammad Frehat"),
    age: new FormControl(22)
  });

  // Invalid -> valid
  form = new FormGroup({
    email: new FormControl("", [Validators.required, Validators.email]),
    name: new FormControl(null, Validators.required),
    phone: new FormControl(null, [Validators.required, Validators.minLength(9), Validators.maxLength(10)]),
    age: new FormControl(null,),
    course: new FormControl(1, Validators.required)
  });

  courseOptions = [
    { id: 1, name: "Angular" },
    { id: 2, name: "Asp.net" },
    { id: 3, name: "Python" },
    { id: 4, name: "Java" },
  ]

  students = [
    { id: 0, name: "student 1", mark: 49 },
    { id: 1, name: "student 2", mark: 59 },
    { id: 2, name: "student 3", mark: 79 },
    { id: 3, name: "student 4", mark: 29 },
    { id: 4, name: "student 5", mark: 99 },
  ];

  formReset() {
    this.form.reset({
      course: 1
    });
  }

  submit() {
    let name = this.form.value?.name;
    let course = this.form.value?.course;
    let courseName = this.courseOptions.find(x => x.id == course)?.name;
    alert(`Welcome: ${name} to The Acadmey. 
      We will Contact you shortly about the Course : ${courseName}`);
  }
  /*
  test(x : number, y : string) : string
  {
    let num : string = "s"; // Type Script same JS but with types
    num = "LUFFY"; // You Can't in TS

    let arr : number[] = ["s",1,true] there is no random variables in TS

    return "";
  }
  */

  images = [
    "https://i0.wp.com/picjumbo.com/wp-content/uploads/beautiful-fall-nature-scenery-free-image.jpeg?w=2210&quality=70",
    "https://t3.ftcdn.net/jpg/02/70/35/00/360_F_270350073_WO6yQAdptEnAhYKM5GuA9035wbRnVJSr.jpg",
    "https://images.pexels.com/photos/26151151/pexels-photo-26151151/free-photo-of-night-sky-filled-with-stars-reflecting-in-the-lake.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500"
  ];

  currentIndex: number = 0;

  nextImage() {
    if (this.currentIndex < this.images.length - 1) {
      this.currentIndex++;
    }

  }

  previousImage() {
    if (this.currentIndex > 0) {
      this.currentIndex--;
    }
  }


}
