

namespace CleanArchitecture.Domain.Abstraction;
public abstract class Entity
{

    public Entity()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    #region AuditLog

    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreatedUserId { get; set; } = default!;


    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid? UpdatedUserId { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? DeletedUserId { get; set; }
    public bool IsDeleted { get; set; }

    #endregion

}
