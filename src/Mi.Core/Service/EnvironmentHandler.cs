using System.Diagnostics.CodeAnalysis;

namespace Mi.Core.Service
{
    public static class EnvironmentHandler
    {
        [NotNull]
        public static string? WebRootPath { get; set; } = "wwwroot";
    }
}