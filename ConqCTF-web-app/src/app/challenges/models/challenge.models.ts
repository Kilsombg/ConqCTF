export interface PaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface ChallengeDto {
  id: number;
  title?: string;
  category: number;
  difficulty: number;
  points: number;
  isSolved: boolean;
}

export interface ChallengeFileDto {
  fileName?: string;
}

export interface ChallengeDetailsDto {
  id: number;
  title?: string;
  description?: string;
  category: number;
  difficulty: number;
  points: number;
  files?: ChallengeFileDto[];
  hints?: string[];
}