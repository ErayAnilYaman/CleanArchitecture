using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Options;

public sealed class KeycloakConfiguration
{
    public string HostName { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string Realm { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
}