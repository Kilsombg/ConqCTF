export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  aaccessToken : string;
  refreshToken : string;
}

export interface RefreshTokenResponce {
  accessToken : string;
}