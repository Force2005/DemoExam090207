namespace DE6.Forms.Models;

internal enum TestResult
{
    Success,
    Failure
}

internal sealed record TestCaseResult(
    string BookmarkName,
    string Action,
    string ExpectedResult,
    TestResult Result)
{
    public string ResultText => Result == TestResult.Success ? "Успешно" : "Не успешно";
}
