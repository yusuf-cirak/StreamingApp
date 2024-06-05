using Application.Abstractions.Helpers;
using Bogus;
using Domain.Constants;
using Infrastructure.Helpers.Hashing;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace Infrastructure.Persistence.EntityFramework;

public class SeedDataGenerator
{
    private readonly IServiceProvider _services;

    public SeedDataGenerator(IServiceProvider services)
    {
        _services = services;
    }

    private List<User> GenerateUsers(int count = 1000)
    {
        new HashingHelper().CreatePasswordHash("deneme", out byte[] passwordHash, out byte[] passwordSalt);
        return new Faker<User>()
            .RuleFor(u => u.Username, f => f.Person.UserName.ToLower())
            .RuleFor(u => u.PasswordHash, f => passwordHash)
            .RuleFor(u => u.PasswordSalt, f => passwordSalt)
            .Generate(count);
    }

    private List<StreamOption> GenerateStreamOptions(List<User> users, IEncryptionHelper encryptionHelper)
    {
        var streamOptions = new List<StreamOption>();
        users.ForEach(user =>
        {
            var streamer = new Faker<StreamOption>()
                .RuleFor(s => s.Id, user.Id)
                .RuleFor(s => s.StreamKey, f => encryptionHelper.Encrypt(user.Username))
                .RuleFor(s => s.StreamTitle, f => f.Random.Words(5))
                .RuleFor(s => s.StreamDescription, f => f.Random.Words(10));

            streamOptions.Add(streamer);
        });
        return streamOptions;
    }

    private List<OperationClaim> GenerateOperationClaims()
    {
        var operationClaims = new List<OperationClaim>();
        operationClaims.Add(OperationClaim.Create(
            GuidExtensions.GenerateGuidFromString(OperationClaimConstants.Stream.Read.BlockFromChat),
            OperationClaimConstants.Stream.Read.BlockFromChat));
        operationClaims.Add(OperationClaim.Create(
            GuidExtensions.GenerateGuidFromString(OperationClaimConstants.Stream.Write.BlockFromChat),
            OperationClaimConstants.Stream.Write.BlockFromChat));
        
        
        operationClaims.Add(OperationClaim.Create(
            GuidExtensions.GenerateGuidFromString(OperationClaimConstants.Stream.Read.DelayChat),
            OperationClaimConstants.Stream.Read.DelayChat));
        operationClaims.Add(OperationClaim.Create(
            GuidExtensions.GenerateGuidFromString(OperationClaimConstants.Stream.Write.DelayChat),
            OperationClaimConstants.Stream.Write.DelayChat));
        
        
        operationClaims.Add(OperationClaim.Create(
            GuidExtensions.GenerateGuidFromString(OperationClaimConstants.Stream.Read.TitleDescription),
            OperationClaimConstants.Stream.Read.TitleDescription));
        
        operationClaims.Add(OperationClaim.Create(
            GuidExtensions.GenerateGuidFromString(OperationClaimConstants.Stream.Write.TitleDescription),
            OperationClaimConstants.Stream.Write.TitleDescription));
        

        operationClaims.Add(OperationClaim.Create(
            GuidExtensions.GenerateGuidFromString(OperationClaimConstants.Stream.Read.ChatOptions),
            OperationClaimConstants.Stream.Read.ChatOptions));
        
        operationClaims.Add(OperationClaim.Create(
            GuidExtensions.GenerateGuidFromString(OperationClaimConstants.Stream.Write.ChatOptions),
            OperationClaimConstants.Stream.Write.ChatOptions));

        return operationClaims;
    }

    private List<Role> GenerateRoles()
    {
        var roles = new List<Role>();

        roles.Add(Role.Create(GuidExtensions.GenerateGuidFromString(RoleConstants.Admin),
            RoleConstants.Admin));
        roles.Add(Role.Create(GuidExtensions.GenerateGuidFromString(RoleConstants.StreamSuperModerator),
            RoleConstants.StreamSuperModerator));
        roles.Add(Role.Create(GuidExtensions.GenerateGuidFromString(RoleConstants.StreamModerator),
            RoleConstants.StreamModerator));

        return roles;
    }

    private List<RoleOperationClaim> GenerateRoleOperationClaims(List<Role> roles, List<OperationClaim> operationClaims)
    {
        var roleOperationClaims = new List<RoleOperationClaim>();

        var superModeratorRole = roles.Find(r => r.Name == RoleConstants.StreamSuperModerator);

        var superModeratorOperationClaims = operationClaims.FindAll(oc => oc.Name.Contains("Stream."));

        superModeratorOperationClaims.ForEach(oc =>
        {
            roleOperationClaims.Add(RoleOperationClaim.Create(superModeratorRole.Id, oc.Id));
        });

        return roleOperationClaims;
    }

    private List<UserOperationClaim> GenerateUserOperationClaims(List<User> users, List<OperationClaim> operationClaims)
    {
        var userOperationClaims = new List<UserOperationClaim>();
        users.ForEach(user =>
        {
            operationClaims.ForEach(operationClaim =>
            {
                userOperationClaims.Add(UserOperationClaim.Create(user.Id, operationClaim.Id,
                    user.Id.ToString()));
            });
        });

        return userOperationClaims;
    }

    private List<UserRoleClaim> GenerateUserRoleClaims(List<User> users, List<Role> roles)
    {
        var userRoleClaims = new List<UserRoleClaim>();
        users.ForEach(user =>
        {
            roles.ForEach(role =>
            {
                userRoleClaims.Add(UserRoleClaim.Create(user.Id, role.Id, user.Id.ToString()));
            });
        });

        return userRoleClaims;
    }

    public void GenerateSeedDataAndPersist()
    {
        var context = _services.GetRequiredService<BaseDbContext>();
        // var encryptionHelper = _services.GetRequiredService<IEncryptionHelper>();


        var dataCount = context.Roles.Count();

        if (dataCount > 0)
        {
            return;
        }

        // var users = GenerateUsers();
        // var streamOptions = GenerateStreamOptions(users, encryptionHelper);

        var operationClaims = GenerateOperationClaims();
        var roles = GenerateRoles();

        // var userRoleClaims = GenerateUserRoleClaims(users, roles);

        var roleOperationClaims = GenerateRoleOperationClaims(roles, operationClaims);
        // var userOperationClaims = GenerateUserOperationClaims(users, operationClaims);

        // context.Users.AddRange(users);
        // context.StreamOptions.AddRange(streamOptions);
        context.OperationClaims.AddRange(operationClaims);
        context.Roles.AddRange(roles);
        context.RoleOperationClaims.AddRange(roleOperationClaims);
        // context.UserOperationClaims.AddRange(userOperationClaims);
        // context.UserRoleClaims.AddRange(userRoleClaims);

        context.SaveChanges();
    }
}