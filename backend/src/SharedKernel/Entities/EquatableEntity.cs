namespace SharedKernel;

public class EquatableEntity : Entity, IEquatable<Entity>
{
    public bool Equals(Entity other)
    {
        if (other == null) return false;
        return this.Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        if (obj is Entity other)
        {
            return Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}