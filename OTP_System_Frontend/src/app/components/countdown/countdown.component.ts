import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'countdown',
  standalone: true,
  imports: [],
  templateUrl: './countdown.component.html',
  styleUrl: './countdown.component.scss'
})
export class CountdownComponent implements OnInit {
  endTime!:Date;
  timeRemaining!: number;
  intervalId: any;
  @Output() countdownFinished = new EventEmitter<void>();

  constructor() { }

  ngOnInit(): void {
  }

  startCountdown(duration : number): void {
    this.endTime = new Date(new Date().getTime() + duration*1000);
    this.updateTimeRemaining(); // Initial calculation
    this.intervalId = setInterval(() => {
      this.updateTimeRemaining();
    }, 1000);
  }

  updateTimeRemaining(): void {
    const now = new Date();
    this.timeRemaining = Math.floor((this.endTime.getTime() - now.getTime()) / 1000);
    if (this.timeRemaining <= 0) {
      clearInterval(this.intervalId);
      this.countdownFinished.emit();
    }
  }
  formatTime(seconds: number): string {
    const hours = Math.floor(seconds / 3600);
    const minutes = Math.floor((seconds % 3600) / 60);
    const remainingSeconds = seconds % 60;

    return `${this.padZero(hours)}:${this.padZero(minutes)}:${this.padZero(remainingSeconds)}`;
  }

  padZero(num: number): string {
    return num < 10 ? `0${num}` : `${num}`;
  }
  ngOnDestroy(): void {
    clearInterval(this.intervalId);
  }
}
