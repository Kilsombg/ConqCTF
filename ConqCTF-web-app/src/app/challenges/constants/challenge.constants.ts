export interface LabeledValue {
  value: number;
  label: string;
}

export const CHALLENGE_CATEGORIES: LabeledValue[] = [
  { value: 1, label: 'Cryptography' },
  { value: 2, label: 'Web' },
  { value: 3, label: 'Reverse Engineering' },
  { value: 4, label: 'Forensics' },
  { value: 5, label: 'General Skills' },
  { value: 6, label: 'Binary Exploitation' },
  { value: 7, label: 'Misc' }
];

export const CHALLENGE_DIFFICULTIES: LabeledValue[] = [
  { value: 1, label: 'Easy' },
  { value: 2, label: 'Medium' },
  { value: 3, label: 'Hard' }
];


export function getCategoryLabel(value: number): string {
  return CHALLENGE_CATEGORIES.find(c => c.value === value)?.label ?? 'Unknown';
}

export function getDifficultyLabel(value: number): string {
  return CHALLENGE_DIFFICULTIES.find(d => d.value === value)?.label ?? 'Unknown';
}