import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({ 
    providedIn: 'root' 
})
export class ChallengeEventsService {
  private challengeSolvedSource = new Subject<number>();
  challengeSolved$ = this.challengeSolvedSource.asObservable();

  notifySolved(challengeId: number) {
    this.challengeSolvedSource.next(challengeId);
  }
}
