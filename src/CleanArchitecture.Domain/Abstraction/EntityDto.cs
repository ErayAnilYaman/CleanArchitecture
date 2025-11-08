using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Abstraction
{
    public abstract class EntityDto
    {
        // Donen veri
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreatedUserId { get; set; }
        public string CreatedUserName { get; set; } = default!;

        public DateTimeOffset? UpdatedAt { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public string? UpdatedUserName { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedUserId { get; set; }
        public string? DeletedUserName{ get; set; }
        public bool IsDeleted { get; set; }


    }
}
