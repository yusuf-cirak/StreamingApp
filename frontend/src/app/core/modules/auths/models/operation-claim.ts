export type UserOperationClaimDto = {
  operationClaim: OperationClaimDto;
  value: string;
};

export type OperationClaimDto = {
  id: number;
  name: string;
};
