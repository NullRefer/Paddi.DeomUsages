using System.Diagnostics.CodeAnalysis;

namespace Paddi.DemoUsages.Core;

/// <summary>
/// Provide runner description
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class RunnerAttribute : Attribute
{
    /// <summary>
    /// Name of the runner
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of what the runner does
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The reference url that the runner aims to propose
    /// </summary>
    public string ReferenceUrl { get; set; }

    public RunnerAttribute([NotNull] string name, [NotNull] string desc, [NotNull] string referenceUrl)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
        }

        if (string.IsNullOrEmpty(desc))
        {
            throw new ArgumentException($"“{nameof(desc)}”不能为 null 或空。", nameof(desc));
        }

        if (string.IsNullOrEmpty(referenceUrl))
        {
            throw new ArgumentException($"“{nameof(referenceUrl)}”不能为 null 或空。", nameof(referenceUrl));
        }

        Name = name;
        Description = desc;
        ReferenceUrl = referenceUrl;
    }
}
