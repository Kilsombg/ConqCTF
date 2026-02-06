import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {

  constructor(private snackBar: MatSnackBar) {}

  success(message: string): void {
    this.open(message, 'success-snackbar');
  }

  error(message: string): void {
    this.open(message, 'error-snackbar');
  }

  info(message: string): void {
    this.open(message, 'info-snackbar');
  }

  private open(message: string, panelClass: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass
    });
  }
}