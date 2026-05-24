using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DE6.Forms.Models;

internal static class TestCaseDocumentWriter
{
    public static void SaveResults(string templatePath, string outputPath, IReadOnlyList<TestCaseResult> results)
    {
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException("Файл шаблона ТестКейс.docx не найден.", templatePath);
        }

        if (results.Count == 0)
        {
            throw new ArgumentException("Нет результатов для записи.", nameof(results));
        }

        File.Copy(templatePath, outputPath, overwrite: true);

        using WordprocessingDocument wordDocument = WordprocessingDocument.Open(outputPath, true);
        MainDocumentPart mainDocumentPart = wordDocument.MainDocumentPart
            ?? throw new InvalidOperationException("Документ не содержит основной части.");
        Document document = mainDocumentPart.Document
            ?? throw new InvalidOperationException("Документ не содержит корневого элемента.");
        Body body = document.Body
            ?? throw new InvalidOperationException("Документ не содержит тела.");
        Table table = body.Elements<Table>().FirstOrDefault()
            ?? throw new InvalidOperationException("В документе не найдена таблица тест-кейсов.");

        for (int i = 0; i < results.Count; i++)
        {
            TestCaseResult result = results[i];
            TableRow row = FindRowByBookmark(table, result.BookmarkName) ?? GetOrCreateDataRow(table, i);
            List<TableCell> cells = row.Elements<TableCell>().ToList();

            while (cells.Count < 3)
            {
                TableCell cell = CreateCell();
                row.AppendChild(cell);
                cells.Add(cell);
            }

            SetCellText(cells[0], result.Action);
            SetCellText(cells[1], result.ExpectedResult);
            SetCellText(cells[2], result.ResultText);
        }

        document.Save();
    }

    private static TableRow? FindRowByBookmark(Table table, string bookmarkName)
    {
        return table
            .Descendants<BookmarkStart>()
            .FirstOrDefault(bookmark => bookmark.Name == bookmarkName)
            ?.Ancestors<TableRow>()
            .FirstOrDefault();
    }

    private static TableRow GetOrCreateDataRow(Table table, int resultIndex)
    {
        const int firstDataRowIndex = 2;
        int targetIndex = firstDataRowIndex + resultIndex;
        List<TableRow> rows = table.Elements<TableRow>().ToList();

        if (targetIndex < rows.Count)
        {
            return rows[targetIndex];
        }

        while (rows.Count <= targetIndex)
        {
            TableRow row = new();
            row.Append(CreateCell(), CreateCell(), CreateCell());
            table.AppendChild(row);
            rows.Add(row);
        }

        return rows[targetIndex];
    }

    private static TableCell CreateCell()
    {
        return new TableCell(
            new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }),
            new Paragraph());
    }

    private static void SetCellText(TableCell cell, string text)
    {
        OpenXmlElement? properties = cell.GetFirstChild<TableCellProperties>()?.CloneNode(true);

        cell.RemoveAllChildren();

        if (properties is not null)
        {
            cell.AppendChild(properties);
        }

        cell.AppendChild(
            new Paragraph(
                new Run(
                    new Text(text)
                    {
                        Space = SpaceProcessingModeValues.Preserve
                    })));
    }
}
