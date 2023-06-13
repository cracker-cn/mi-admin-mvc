using System.Diagnostics.CodeAnalysis;

namespace Mi.Core.Service
{
    public class EnvironmentHandler
    {
        [NotNull]
        public string? WebRootPath { get; set; }
    }
}