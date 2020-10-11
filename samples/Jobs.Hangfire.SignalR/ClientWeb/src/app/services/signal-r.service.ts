import { Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { JobStateModel, JobStateModelExtensions } from '../_interfaces/jobstatemodel';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  public jobs: Map<number, JobStateModel> = new Map<number, JobStateModel>();
  private hubConnection: signalR.HubConnection

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/jobstate')
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  addJobStateListener = () => {
    this.hubConnection.on('ReceiveJobStateCreateNotification', (data) => {
      this.updateJob(data);
      console.log(data);
    });
    this.hubConnection.on('ReceiveJobStateUpdateNotification', (data) => {
      this.updateJob(data);
      console.log(data);
    });
  }

  updateJob(data: any) {
    const job: JobStateModel = {
      id: data.id,
      progress: data.progress,
      details: data.details,
      type: data.type,
      status: JobStateModelExtensions.GetStatusDisplayName(data.status),
      ownerId: data.ownerId,
      projectId: data.projectId,
      creationTime: data.creationTime,
      modificationTime: data.modificationTime,
      completionTime: data.completionTime
    };

    this.jobs.set(job.id, job);
  }
}
