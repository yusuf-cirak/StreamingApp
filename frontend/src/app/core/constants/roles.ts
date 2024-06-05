import { UserRoleDto } from '../modules/auths/models/role';

export const Roles = {
  Admin: 'Admin',
  StreamSuperModerator: 'Stream.SuperModerator',
  StreamModerator: 'Stream.Moderator',
};

export const getRequiredRoles = (streamerId: string): UserRoleDto[] => {
  return [
    { name: Roles.Admin },
    { name: Roles.StreamSuperModerator, value: streamerId },
    { name: Roles.StreamModerator, value: streamerId },
  ];
};
