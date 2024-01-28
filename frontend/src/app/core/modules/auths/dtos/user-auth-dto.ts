import { UserClaimsDto } from '../models/claim';

export type UserAuthDto = {
  id: string;
  username: string;
  profileImageUrl: string;
  accessToken: string;
  refreshToken: string;
  tokenExpiration: Date;
  claims: UserClaimsDto;
};
