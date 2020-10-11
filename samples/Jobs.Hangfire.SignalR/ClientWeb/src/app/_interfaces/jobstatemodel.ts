export interface JobStateModel {
  id: number;
  type: string;
  progress: string;
  details: string;
  status: string;
  ownerId: string;
  projectId: string;
  creationTime: Date;
  modificationTime: Date;
  completionTime: Date;
}

export class JobStateModelExtensions {
  public static GetStatusDisplayName(status: number): string {
    switch (status) {
      case 1: return 'Registered';
      case 2: return 'Queued';
      case 3: return 'Running';
      case 4: return 'Pausing';
      case 5: return 'Paused';
      case 6: return 'Resuming';
      case 7: return 'Cancelling';
      case 8: return 'Canceled';
      case 9: return 'Failed';
      case 10: return 'Succeeded';
      default: return '?';
    }
  }
}
