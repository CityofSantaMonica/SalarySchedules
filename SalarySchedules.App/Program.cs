using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SalarySchedules;

namespace SalarySchedules.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }

            List<SalarySchedule> shedules = new List<SalarySchedule>();

            foreach (var file in args)
            {
                
            }
        }

        static DateTime readReportDate(string file)
        {
            DateTime reportDate = DateTime.MinValue;

            using (var reader = new PdfReader(file))
            {
                var pageContent 
            }
        }

        static IEnumerable<string> readDataToText(string file)
        {
            List<string> content = new List<string>();

            using (var reader = new PdfReader(file))
            {
                for (int i = 2; i <= reader.NumberOfPages; i++)
                {
                    var pageContent = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());
                    content.Add(pageContent);
                }
            }

            return content;
        }
    }
}
