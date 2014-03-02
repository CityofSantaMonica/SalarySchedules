using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace SalarySchedules.Parser
{
    public static class Extensions
    {
        public static string TextFromPage(this PdfReader reader, int pageNumber)
        {
            var strategy = new LocationTextExtractionStrategy();
            return PdfTextExtractor.GetTextFromPage(reader, pageNumber, strategy);
        }
    }
}
