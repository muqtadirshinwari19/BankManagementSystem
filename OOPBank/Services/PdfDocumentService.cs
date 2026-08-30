using UglyToad.PdfPig;

namespace BankSystem.Services;

public class PdfDocumentService
{
    public string ExtractText(string filePath)
    {
        using var document = PdfDocument.Open(filePath);

        var text = "";

        foreach (var page in document.GetPages())
        {
            text += page.Text + Environment.NewLine;
        }

        return text;
    }
}