import { UserOperationClaimDto } from '../modules/auths/models/operation-claim';

export const OperationClaims = {
  Stream: {
    Read: {
      BlockFromChat: 'Stream.Read.BlockFromChat',
      DelayChat: 'Stream.Read.DelayChat',
      TitleDescription: 'Stream.Read.TitleDescription',
      ChatOptions: 'Stream.Read.ChatOptions',
    },

    Write: {
      BlockFromChat: 'Stream.Write.BlockFromChat',
      DelayChat: 'Stream.Write.DelayChat',
      TitleDescription: 'Stream.Write.TitleDescription',
      ChatOptions: 'Stream.Write.ChatOptions',
    },
  },
};

export const getRequiredWriteOperationClaims = (
  streamerId: string
): UserOperationClaimDto[] => {
  return [
    { name: OperationClaims.Stream.Write.BlockFromChat, value: streamerId },
    { name: OperationClaims.Stream.Write.DelayChat, value: streamerId },
    { name: OperationClaims.Stream.Write.TitleDescription, value: streamerId },
    { name: OperationClaims.Stream.Write.ChatOptions, value: streamerId },
  ];
};

export const getRequiredReadOperationClaims = (
  streamerId: string
): UserOperationClaimDto[] => {
  return [
    { name: OperationClaims.Stream.Read.BlockFromChat, value: streamerId },
    { name: OperationClaims.Stream.Read.DelayChat, value: streamerId },
    { name: OperationClaims.Stream.Read.TitleDescription, value: streamerId },
    { name: OperationClaims.Stream.Read.ChatOptions, value: streamerId },
  ];
};
