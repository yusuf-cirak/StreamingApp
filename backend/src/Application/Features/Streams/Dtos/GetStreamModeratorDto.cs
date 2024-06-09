namespace Application.Features.Streams.Dtos;

public sealed record GetStreamModeratorDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string ProfileImageUrl { get; set; }
    public List<Guid> RoleIds { get; set; }
    public List<Guid> OperationClaimIds { get; set; }
}