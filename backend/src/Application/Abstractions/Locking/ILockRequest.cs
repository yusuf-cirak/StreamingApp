namespace Application.Abstractions.Locking;

public interface ILockRequest
{
    string Key { get; }
    int Expiration { get; }
    bool ReleaseImmediately { get; }
}