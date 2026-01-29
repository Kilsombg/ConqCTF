import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ChallengesService } from 'src/app/challenges/services/challenges.service';

@Component({
  selector: 'app-edit-challenge',
  templateUrl: './edit-challenge.component.html',
  styleUrls: ['./edit-challenge.component.css']
})
export class EditChallengeComponent {
  challenge: any;

  constructor(
    private route: ActivatedRoute,
    private challengesService: ChallengesService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.challengesService.getChallenge(id).subscribe(c => {
      this.challenge = c;
    });
  }

  update(formData: FormData): void {
    const id = this.challenge.id;

    this.challengesService.updateChallenge(id, formData).subscribe(() => {
      this.router.navigate(['/challenges', id]);
    });
  }
}
