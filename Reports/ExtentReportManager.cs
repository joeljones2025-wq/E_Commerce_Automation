using System.Collections.Concurrent;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

[assembly: ECommerceAutomation.Reports.ExtentReportAction]

namespace ECommerceAutomation.Reports;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class ExtentReportAction : Attribute, ITestAction
{
    public ActionTargets Targets => ActionTargets.Test;

    public void BeforeTest(ITest test)
    {
        ExtentReportManager.StartTest(test);
    }

    public void AfterTest(ITest test)
    {
        ExtentReportManager.FinishTest(test);
    }
}

public abstract class ReportedPageTest : PageTest
{
    [TearDown]
    public async Task CaptureExtentScreenshot()
    {
        await ExtentReportManager.CaptureScreenshotAsync(
            TestContext.CurrentContext.Test.FullName,
            Page);
    }
}

public static class ExtentReportManager
{
    private static readonly object Sync = new();
    private static readonly ConcurrentDictionary<string, ExtentTest> Tests = new();
    private static ExtentReports? extent;

    public static ExtentReports GetExtent()
    {
        lock (Sync)
        {
            if (extent == null)
            {
                string testCasesDirectory = Path.GetFullPath(Path.Combine(
                    AppContext.BaseDirectory,
                    "..", "..", ".."));
                string resultsDirectory = Path.Combine(testCasesDirectory, "TestResults");
                Directory.CreateDirectory(resultsDirectory);

                var reporter = new ExtentSparkReporter(
                    Path.Combine(resultsDirectory, "ExtentReport.html"));

                extent = new ExtentReports();
                extent.AttachReporter(reporter);
                extent.AddSystemInfo("Project", "E-Commerce Automation");
                extent.AddSystemInfo("Framework", "Playwright C#");
                extent.AddSystemInfo("Test Framework", "NUnit");
            }

            return extent;
        }
    }

    internal static void StartTest(ITest test)
    {
        Tests[test.FullName] = GetExtent().CreateTest(test.FullName);
    }

    internal static async Task CaptureScreenshotAsync(string testName, IPage page)
    {
        if (!Tests.TryGetValue(testName, out var extentTest))
        {
            return;
        }

        try
        {
            string screenshotsDirectory = Path.Combine(
                Path.GetDirectoryName(GetReportPath())!, "Screenshots");
            Directory.CreateDirectory(screenshotsDirectory);
            string screenshotPath = Path.Combine(
                screenshotsDirectory,
                $"{SanitizeFileName(testName)}-{Guid.NewGuid():N}.png");

            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = screenshotPath,
                FullPage = true
            });
            extentTest.AddScreenCaptureFromPath(screenshotPath);
        }
        catch (Exception exception)
        {
            extentTest.Warning($"Screenshot could not be captured: {exception.Message}");
        }
    }

    internal static void FinishTest(ITest test)
    {
        if (!Tests.TryRemove(test.FullName, out var extentTest))
        {
            return;
        }

        var context = TestContext.CurrentContext;
        if (context.Result.Outcome.Status == TestStatus.Passed)
        {
            extentTest.Pass("Test passed");
        }
        else
        {
            extentTest.Fail(context.Result.Message ?? context.Result.Outcome.Status.ToString());
        }

        GetExtent().Flush();
    }

    private static string GetReportPath() => Path.Combine(
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..")),
        "TestResults",
        "ExtentReport.html");

    private static string SanitizeFileName(string fileName)
    {
        foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(invalidCharacter, '_');
        }

        return fileName;
    }
}