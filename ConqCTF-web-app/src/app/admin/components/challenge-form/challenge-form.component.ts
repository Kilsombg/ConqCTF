import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-challenge-form',
  templateUrl: './challenge-form.component.html',
  styleUrls: ['./challenge-form.component.css']
})
export class ChallengeFormComponent {

  @Input() submitLabel = 'Save';
  @Input() initialData?: any;
  @Input() success?: string;
  @Input() error?: string;

  @Output() submitForm = new EventEmitter<FormData>();

  title = '';
  description = '';
  category = 0;
  difficulty = 0;
  points = 1;
  flag = '';

  files: File[] = [];
  hints: string[] = [];
  newHint = '';

  ngOnInit(): void {
    if (!this.initialData) return;

    const c = this.initialData;

    this.title = c.title;
    this.description = c.description;
    this.category = c.category;
    this.difficulty = c.difficulty;
    this.points = c.points;
    this.hints = [...(c.hints ?? [])];
  }

  addHint(): void {
    if (!this.newHint.trim()) return;
    this.hints.push(this.newHint.trim());
    this.newHint = '';
  }

  removeHint(index: number): void {
    this.hints.splice(index, 1);
  }

  onFilesSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files) return;
    this.files = Array.from(input.files);
  }

  submit(): void {
    const formData = new FormData();

    formData.append('Title', this.title);
    formData.append('Description', this.description);
    formData.append('Category', this.category.toString());
    formData.append('Difficulty', this.difficulty.toString());
    formData.append('Points', this.points.toString());
    formData.append('Flag', this.flag);

    this.files.forEach(f => formData.append('Files', f, f.name));
    this.hints.forEach(h => formData.append('Hints', h));

    this.submitForm.emit(formData);
  }
}
