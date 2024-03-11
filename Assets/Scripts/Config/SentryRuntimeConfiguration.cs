using Sentry;
using Sentry.Unity;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Assets/Resources/Sentry/SentryRuntimeConfiguration.asset",
    menuName = "Sentry/SentryRuntimeConfiguration",
    order = 999
)]
public class SentryRuntimeConfiguration : SentryRuntimeOptionsConfiguration
{
    public override void Configure(SentryUnityOptions options)
    {
        options.ExperimentalMetrics = new ExperimentalMetricsOptions { EnableCodeLocations = true };
    }
}
