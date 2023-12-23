namespace Domain.Errors;

public readonly record struct StreamChatMessageErrors
{
    public static readonly Error MessageCannotBeEmpty =
        Error.Create("StreamChatMessage.MessageCannotBeEmpty", "Stream chat message cannot be empty");

    public static readonly Error MessageCannotBeLongerThan1000Characters = Error.Create(
        "StreamChatMessage.MessageCannotBeLongerThan1000Characters",
        "Stream chat message cannot be longer than 1000 characters");

    public static readonly Error MessageCannotBeShorterThan2Characters = Error.Create(
        "StreamChatMessage.MessageCannotBeShorterThan2Characters",
        "Stream chat message cannot be shorter than 2 characters");

    public static readonly Error MessageCannotContainWhiteSpaces =
        Error.Create("StreamChatMessage.MessageCannotContainWhiteSpaces", "Stream chat message cannot contain white spaces");

    public static readonly Error MessageCannotContainSpecialCharacters = Error.Create(
        "StreamChatMessage.MessageCannotContainSpecialCharacters", "Stream chat message cannot contain special characters");

    public static readonly Error MessageCannotBeDuplicated = Error.Create("StreamChatMessage.MessageCannotBeDuplicated",
        "Stream chat message cannot be duplicated");
    
}