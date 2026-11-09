using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Reqnroll;

namespace ECommerceAutomation.CSharpTests.Support;

[Binding]
public sealed class ExtentReportHooks
{
    private static ExtentReports? _extent;
    private readonly ScenarioContext _scenarioContext;
    private ExtentTest? _scenarioTest;

    public ExtentReportHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static void StartReport()
    {
        var reportPath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "TestResults",
            "ExtentReport.html"));
        Directory.CreateDirectory(Path.GetDirectoryName(reportPath)!);

        _extent = new ExtentReports();
        _extent.AttachReporter(new ExtentSparkReporter(reportPath));
    }

    [BeforeScenario]
    public void StartScenario()
    {
        _scenarioTest = _extent!.CreateTest(_scenarioContext.ScenarioInfo.Title);
    }

    [AfterScenario]
    public void FinishScenario()
    {
        if (_scenarioContext.TestError is { } error)
        {
            _scenarioTest!.Fail(error.ToString());
        }
        else
        {
            _scenarioTest!.Pass("Scenario passed");
        }
    }

    [AfterTestRun]
    public static void FlushReport()
    {
        _extent?.Flush();
    }
}