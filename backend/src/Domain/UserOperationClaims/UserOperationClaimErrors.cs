namespace Domain.Errors;

public readonly record struct UserOperationClaimErrors
{
    public static readonly Error UserIdCannotBeEmpty =
        Error.Create("UserOperationClaims.UserIdCannotBeEmpty", "User operation claims user id cannot be empty");

    public static readonly Error UserIdCannotBeLongerThan100Characters = Error.Create(
        "UserOperationClaims.UserIdCannotBeLongerThan100Characters",
        "User operation claims user id cannot be longer than 100 characters");

    public static readonly Error UserIdCannotBeShorterThan2Characters = Error.Create(
        "UserOperationClaims.UserIdCannotBeShorterThan2Characters",
        "User operation claims user id cannot be shorter than 2 characters");

    public static readonly Error OperationClaimIdCannotBeEmpty =
        Error.Create("UserOperationClaims.OperationClaimIdCannotBeEmpty",
            "User operation claims operation claim id cannot be empty");

    public static readonly Error OperationClaimIdCannotBeLongerThan100Characters = Error.Create(
        "UserOperationClaims.OperationClaimIdCannotBeLongerThan100Characters",
        "User operation claims operation claim id cannot be longer than 100 characters");

    public static readonly Error OperationClaimIdCannotBeShorterThan2Characters = Error.Create(
        "UserOperationClaims.OperationClaimIdCannotBeShorterThan2Characters",
        "User operation claims operation claim id cannot be shorter than 2 characters");
}