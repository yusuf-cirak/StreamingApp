namespace Domain.Errors;

public readonly record struct RoleOperationClaimErrors
{
    public static readonly Error RoleIdCannotBeEmpty =
        Error.Create("RoleOperationClaim.RoleIdCannotBeEmpty", "Role id cannot be empty");

    public static readonly Error RoleIdCannotBeLongerThan100Characters = Error.Create(
        "RoleOperationClaim.RoleIdCannotBeLongerThan100Characters",
        "Role id cannot be longer than 100 characters");

    public static readonly Error RoleIdCannotBeShorterThan2Characters = Error.Create(
        "RoleOperationClaim.RoleIdCannotBeShorterThan2Characters",
        "Role id cannot be shorter than 2 characters");

    public static readonly Error RoleIdCannotContainWhiteSpaces =
        Error.Create("RoleOperationClaim.RoleIdCannotContainWhiteSpaces", "Role id cannot contain white spaces");

    public static readonly Error RoleIdCannotContainSpecialCharacters = Error.Create(
        "RoleOperationClaim.RoleIdCannotContainSpecialCharacters", "Role id cannot contain special characters");

    public static readonly Error RoleIdCannotBeDuplicated = Error.Create("RoleOperationClaim.RoleIdCannotBeDuplicated",
        "Role id cannot be duplicated");

    public static readonly Error OperationClaimIdCannotBeEmpty =
        Error.Create("RoleOperationClaim.OperationClaimIdCannotBeEmpty", "Operation claim id cannot be empty");

    public static readonly Error OperationClaimIdCannotBeLongerThan100Characters = Error.Create(
        "RoleOperationClaim.OperationClaimIdCannotBeLongerThan100Characters",
        "Operation claim id cannot be longer than 100 characters");

    public static readonly Error OperationClaimIdCannotBeShorterThan2Characters = Error.Create(
        "RoleOperationClaim.OperationClaimIdCannotBeShorterThan2Characters",
        "Operation claim id cannot be shorter than 2 characters");

    public static readonly Error OperationClaimIdCannotContainWhiteSpaces =
        Error.Create("RoleOperationClaim.OperationClaimIdCannotContainWhiteSpaces",
            "Operation claim id cannot contain white spaces");

    public static readonly Error OperationClaimIdCannotContainSpecialCharacters = Error.Create(
        "RoleOperationClaim.OperationClaimIdCannotContainSpecialCharacters",
        "Operation claim id cannot contain special characters");

    public static readonly Error OperationClaimIdCannotBeDuplicated = Error.Create(
        "RoleOperationClaim.OperationClaimIdCannotBeDuplicated",
        "Operation claim id cannot be duplicated");

    public static readonly Error OperationClaimDoesNotExist = Error.Create(
        "RoleOperationClaim.OperationClaimDoesNotExist",
        "Operation claim id does not exist");
}