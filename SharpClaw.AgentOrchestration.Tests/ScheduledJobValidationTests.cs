using NUnit.Framework;
using SharpClaw.Modules.AgentOrchestration.Services;

namespace SharpClaw.AgentOrchestration.Tests;

[TestFixture]
public sealed class ScheduledJobValidationTests
{
    [Test]
    public void ValidateCronFieldsRejectsCronAndRepeatIntervalTogether()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            ScheduledJobService.ValidateCronFields(
                "*/5 * * * *",
                cronTimezone: null,
                repeatInterval: TimeSpan.FromMinutes(5),
                suppliedNextRunAt: null));

        Assert.That(exception!.Message, Does.StartWith($"{ScheduledJobService.ErrBothSchedules}:"));
    }

    [Test]
    public void PreviewExpressionReturnsRequestedNumberOfFutureOccurrences()
    {
        var preview = ScheduledJobService.PreviewExpression(
            "*/10 * * * *",
            timezone: null,
            count: 3);

        Assert.Multiple(() =>
        {
            Assert.That(preview.Expression, Is.EqualTo("*/10 * * * *"));
            Assert.That(preview.Timezone, Is.Null);
            Assert.That(preview.NextOccurrences, Has.Count.EqualTo(3));
            Assert.That(preview.NextOccurrences, Is.Ordered);
        });
    }
}
