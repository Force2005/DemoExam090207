using System.Text.RegularExpressions;

namespace DE6.Forms.Models;

internal static class FullNameValidator
{
    private static readonly Regex DigitsRegex = new(@"\d", RegexOptions.Compiled);
    private static readonly Regex AllowedCharactersRegex = new(
        @"^[А-Яа-яЁёA-Za-z\s\-]+$",
        RegexOptions.Compiled);

    public static IReadOnlyList<TestCaseResult> Validate(string fullName)
    {
        bool hasNoDigits = !DigitsRegex.IsMatch(fullName);
        bool hasOnlyAllowedCharacters = AllowedCharactersRegex.IsMatch(fullName);

        return
        [
            new TestCaseResult(
                "СпецСимвол1",
                "Проверить отсутствие цифр в ФИО клиента",
                "ФИО не содержит цифры",
                ToDocumentResult(hasNoDigits)),

            new TestCaseResult(
                "СпецСимвол2",
                "Проверить отсутствие специальных символов в ФИО клиента",
                "ФИО содержит только буквы, пробелы и дефисы",
                ToDocumentResult(hasOnlyAllowedCharacters))
        ];
    }

    public static bool HasForbiddenCharacters(IEnumerable<TestCaseResult> results)
    {
        return results.Any(result => result.Result != TestResult.Success);
    }

    private static TestResult ToDocumentResult(bool passed)
    {
        return passed ? TestResult.Success : TestResult.Failure;
    }
}
