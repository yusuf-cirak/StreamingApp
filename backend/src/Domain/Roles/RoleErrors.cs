﻿namespace Domain.Errors;

public readonly record struct RoleErrors
{
    public static Error RoleNotFound(string roleName) => Error.Create("Role.NotFound", $"Role {roleName} not found");

    public static Error DoesNotExist => Error.Create("Role.DoesNotExist", $"User does not have any role");

    public static Error FailedToCreate => Error.Create("Role.FailedToCreate", $"Failed to create role");
    
    public static Error FailedToDelete => Error.Create("Role.FailedToDelete", $"Failed to delete role");

    public static Error RoleAlreadyExists(string roleName) =>
        Error.Create("Role.AlreadyExists", $"Role {roleName} already exists");

    public static Error RoleCannotBeDeleted(string roleName) =>
        Error.Create("Role.CannotBeDeleted", $"Role {roleName} cannot be deleted");

    public static Error RoleCannotBeDeleted(Guid id) =>
        Error.Create("Role.CannotBeDeleted", $"Role with {id} cannot be deleted because it does not exist");

    public static Error RoleCannotBeUpdated(string roleName) =>
        Error.Create("Role.CannotBeUpdated", $"Role {roleName} cannot be updated");

    public static Error RoleCannotBeCreated(string roleName) =>
        Error.Create("Role.CannotBeCreated", $"Role {roleName} cannot be created");

    public static Error RoleCannotBeEmpty(string roleName) =>
        Error.Create("Role.CannotBeEmpty", $"Role {roleName} cannot be empty");

    public static Error RoleCannotBeLongerThan100Characters(string roleName) =>
        Error.Create("Role.CannotBeLongerThan100Characters", $"Role {roleName} cannot be longer than 100 characters");
}