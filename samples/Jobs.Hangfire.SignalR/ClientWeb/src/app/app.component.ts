import { Component, Input, Output, OnInit } from '@angular/core';
import { SignalRService } from './services/signal-r.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jobStateId: string = '';

  constructor(public signalRService: SignalRService, private http: HttpClient) { }

  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.addJobStateListener();
    // this.signalRService.addTransferChartDataListener();
    // this.signalRService.addBroadcastChartDataListener();
    // this.startHttpRequest();
  }

  public queueClicked = (event) => {
    console.log('Queue');
    this.http.post('https://localhost:5001/api/job/queue', {})
      .subscribe(res => {
        console.log(res);
        this.jobStateId = (res as any).jobStateId;
      });
  }

  public pauseClicked = (event) => {
    console.log(`Pause ${this.jobStateId}`);
    this.http.post(`https://localhost:5001/api/job/pause/${this.jobStateId}`, {})
      .subscribe(res => {
        console.log(res);
      });
  }

  public resumeClicked = (event) => {
    console.log(`Resume ${this.jobStateId}`);
    this.http.post(`https://localhost:5001/api/job/resume/${this.jobStateId}`, {})
      .subscribe(res => {
        console.log(res);
      });
  }

  public cancelClicked = (event) => {
    console.log(`Cancel: ${this.jobStateId}`);
    this.http.post(`https://localhost:5001/api/job/cancel/${this.jobStateId}`, {})
      .subscribe(res => {
        console.log(res);
      });
  }

}
