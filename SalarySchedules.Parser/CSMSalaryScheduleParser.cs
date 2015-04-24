using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text.pdf;
using SalarySchedules.Models;

namespace SalarySchedules.Parser
{
    /// <summary>
    /// ISalaryScheduleParser implementation for the City of Santa Monica.
    /// </summary>
    public class CSMSalaryScheduleParser : ISalaryScheduleParser
    {
        Dictionary<string, BargainingUnit> bargainingUnits;

        /// <summary>
        /// Process a Salary Schedule Report PDF.
        /// </summary>
        /// <param name="pdfPath">Full path to a readable salary schedule report PDF.</param>
        /// <returns>An <c>ISalarySchedule</c> representation of the report data.</returns>
        public ISalarySchedule Process(string pdfPath) 
        {
            ISalarySchedule schedule = new SalarySchedule();
            byte[] fileData = File.ReadAllBytes(pdfPath);

            using (var reader = new PdfReader(fileData))
            {
                schedule.FiscalYear = readFiscalYear(reader);
                schedule.ReportRunDate = readReportDate(reader);

                schedule.BargainingUnits = readBargainingUnits(reader);

                schedule.JobClasses = 
                    getAlignmentCorrectedClassData(reader).SelectMany(page => processClassesOnPage(page)).ToArray();
            }

            return schedule;
        }

        /// <summary>
        /// Get a collection of normalized page data for each page in a file.
        /// </summary>
        /// <param name="filePath">The full path of the file to be read.</param>
        /// <returns>An IEnumerable of IEnumerable of strings, representing the normalized lines of each page.</returns>
        public IEnumerable<IEnumerable<string>> GetAlignmentCorrectedClassData(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            IEnumerable<IEnumerable<string>> alignedPages = null;
            
            using (var reader = new PdfReader(fileData))
            {
                alignedPages = getAlignmentCorrectedClassData(reader);
            }

            return alignedPages;
        }

        /// <summary>
        /// Private implementation that acts on an open PdfReader.
        /// </summary>
        private IEnumerable<IEnumerable<string>> getAlignmentCorrectedClassData(PdfReader reader)
        {
            return
                Enumerable.Range(2, reader.NumberOfPages - 1)
                          .Select(n => reader.TextFromPage(n))
                          .Select(page => getPageChunks(page))
                          .Select(chunks => fixAlignment(chunks))
                          .ToArray();
        }

        /// <summary>
        /// Split a page of data into its chunks.
        /// </summary>
        /// <param name="page">A string containing chunks of textual data, separated by whitespace.</param>
        /// <returns>The chunks, split on newlines.</returns>
        IEnumerable<string> getPageChunks(string page)
        {
            //a page is made up of a series of chunks, which contain field data
            IEnumerable<string> chunks = page.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            //skip the header rows when starting a new page
            if (FieldPatterns.RunDate.IsMatch(chunks.First()))
                chunks = chunks.SkipWhile(s => !FieldPatterns.DataHeader.IsMatch(s)).Skip(1);

            return chunks;
        }

        /// <summary>
        /// Preprocess and condenses the chunks collection to normalize each line.
        /// </summary>
        /// <example>
        /// Given lines:
        ///     1120 ATA 032 1 2,476.62
        ///     Accountant I 30.96 5,366.00 64,392.00
        ///     032 2 2,598.92
        ///     32.49 5,631.00 67,572.00
        ///     032 3 2,752.15
        ///     34.40 5,963.00 71,556.00
        ///     032 4 36.31 6,294.00 75,528.00 2,904.92
        ///     032 5 3,057.69
        ///     38.22 6,625.00 79,500.00
        ///    
        /// This preprocess produces:
        ///     1120 ATA 032 1 2,476.62 Accountant I 30.96 5,366.00 64,392.00
        ///     032 2 2,598.92 32.49 5,631.00 67,572.00
        ///     032 3 2,752.15 34.40 5,963.00 71,556.00
        ///     032 4 36.31 6,294.00 75,528.00 2,904.92
        ///     032 5 3,057.69 38.22 6,625.00 79,500.00
        /// </example>
        /// <returns>Complete records for the fractured lines in <paramref name="chunks"/>.</returns>
        IEnumerable<string> fixAlignment(IEnumerable<string> chunks)
        {
            string replace = " ";
            List<string> final = new List<string>();
            Queue<string> queue = new Queue<string>(chunks);
            
            while (queue.Any())
            {
                string current = FieldPatterns.ConsecutiveSpaces.Replace(queue.Dequeue(), replace).Trim();

                if (queue.Any())
                {
                    string next = queue.Peek().Trim();

                    if (FieldPatterns.ClassCode.IsMatch(current) && FieldPatterns.ClassTitle.IsMatch(next))
                    {
                        current += " " + FieldPatterns.ConsecutiveSpaces.Replace(queue.Dequeue(), replace).Trim();
                    }
                    else if (FieldPatterns.Grade.IsMatch(current) && FieldPatterns.Rate.IsMatch(next)
                         && !FieldPatterns.Grade.IsMatch(next) && !FieldPatterns.ClassCode.IsMatch(next))
                    {
                        current += " " + FieldPatterns.ConsecutiveSpaces.Replace(queue.Dequeue(), replace).Trim();
                    }
                }

                final.Add(current.Replace(" -", "-").Replace("- ", "-"));
            }

            return final;
        }

        /// <summary>
        /// Interprets the fiscal year from the first page of this report.
        /// </summary>
        /// <returns>A complete FiscalYear object, or null if it cannot be read.</returns>
        FiscalYear readFiscalYear(PdfReader reader)
        {
            FiscalYear fiscalYear = null;

            var text = reader.TextFromPage(1);

            if (FieldPatterns.FiscalYear.IsMatch(text))
            {
                var match = FieldPatterns.FiscalYear.Match(text);
                fiscalYear = new FiscalYear(match.Groups[1].Value, match.Groups[2].Value);
            }

            return fiscalYear;
        }

        /// <summary>
        /// Reads the the run date of this report from the first page of data (page 2).
        /// </summary>
        /// <returns>The run date if it is found, otherwise null</returns>
        DateTime? readReportDate(PdfReader reader)
        {
            DateTime reportDate = DateTime.MinValue;

            var text = reader.TextFromPage(2);

            if (FieldPatterns.RunDate.IsMatch(text))
            {
                DateTime.TryParse(FieldPatterns.RunDate.Match(text).Groups[1].Value.Trim(), out reportDate);
            }

            if (reportDate != DateTime.MinValue)
                return reportDate;
            else
                return null;
        }

        /// <summary>
        /// Reads the Bargaining Unit information on the first page of the report.
        /// </summary>
        /// <returns>A collection BargainingUnit objects for this report.</returns>
        IEnumerable<BargainingUnit> readBargainingUnits(PdfReader reader)
        {
            string replace = " ";
            var units = new List<BargainingUnit>();
            //the Fire BU is never output as a code in the BU table?
            var text = reader.TextFromPage(1).Replace("Fire ", "FIR ");
            //get all the BU chunks
            var chunks = getPageChunks(text).Where(c => FieldPatterns.BargainingUnit.IsMatch(c));
            
            foreach (var chunk in chunks)
            {
                //get each of the BUs in this chunk
                var matches = FieldPatterns.BargainingUnit.Matches(chunk);
                //split the chunk at the BU code points (leaving their names)
                var names = FieldPatterns.BargainingUnit.Split(chunk)
                                                        .Where(s => !String.IsNullOrEmpty(s.Trim()))
                                                        .Select(s => FieldPatterns.ConsecutiveSpaces.Replace(s, replace).Trim());
                for (int i = 0; i < matches.Count; i++)
                {
                    var match = matches[i];
                    //there will probably be chunks containing BU codes that were already processed
                    if(!units.Any(bu => bu.Code == match.Value))
                        units.Add(new BargainingUnit() { Code = match.Value, Name = names.ElementAt(i) });
                }
            }

            bargainingUnits = units.ToDictionary(bu => bu.Code, bu => bu);

            return units;
        }

        /// <summary>
        /// Process the job classes on a single page.
        /// </summary>
        /// <param name="page">The textual data of a single page, broken by newline and realigned.</param>
        /// <returns>A collection of JobClass objects from the page.</returns>
        IEnumerable<JobClass> processClassesOnPage(IEnumerable<string> page)
        {
            var jobClasses = new List<JobClass>();

            //to process the chunks sequentially (considering more than one at a time)
            var queue = new Queue<string>(page);

            while (queue.Any())
            {
                var jobClass = new JobClass();
                var steps = new List<JobClassStep>();
                var currentStep = new JobClassStep();

                IEnumerable<string> dataChunks = queue.Dequeue().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                //assign the title, code, bargaining unit, grade for this class
                dataChunks = assignClassData(dataChunks.ToList(), jobClass);
                
                //if there is leftover data -> step definition
                if (dataChunks.Any())
                {
                    currentStep = assignStepData(dataChunks);
                    steps.Add(currentStep);
                }

                //add each subsequent step for this class
                while (queue.Any() && queue.Peek().StartsWith(jobClass.Grade))
                {
                    dataChunks = queue.Dequeue().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Skip(1);
                    currentStep = assignStepData(dataChunks);
                    steps.Add(currentStep);
                }

                jobClass.Steps = steps;
                jobClasses.Add(jobClass);
            }

            return jobClasses;
        }

        /// <summary>
        /// Consumes a line of data to populate a JobClass
        /// </summary>
        /// <param name="dataChunks">A collection of job class data points.</param>
        /// <param name="jobClass">A JobClass object to consume the line data.</param>
        /// <returns>Any remaining data after processing the JobClass.</returns>
        IEnumerable<string> assignClassData(IList<string> dataChunks, JobClass jobClass)
        {
            //the process order here is important, since we're relying on simple(ish) Regex 
            //which may overlap one another

            // 1. code is always 4 consecutive integers
            string code = dataChunks.FirstOrDefault(c => FieldPatterns.ClassCode.IsMatch(c));
            if (!String.IsNullOrEmpty(code))
            {
                jobClass.Code = code;
                dataChunks.Remove(code);
            }
            // 2. grade is always 3 consecutive integers
            string grade = dataChunks.FirstOrDefault(c => FieldPatterns.Grade.IsMatch(c));
            if (!String.IsNullOrEmpty(grade))
            {
                jobClass.Grade = grade;
                dataChunks.Remove(grade);
            }
            // 3. bargaining unit code is always 3 consecutive capital letters
            string bu = dataChunks.FirstOrDefault(c => FieldPatterns.BargainingUnit.IsMatch(c));
            if (!String.IsNullOrEmpty(bu))
            {
                jobClass.BargainingUnit =
                    bargainingUnits.ContainsKey(bu) ?
                    bargainingUnits[bu] : new BargainingUnit() { Name = string.Empty, Code = bu };

                dataChunks.Remove(bu);
            }
            // 4. the remaining chunks are either part of the job title or part of a job step
            //    job step data is all numeric (integer or decimal numbers)
            //    so separate out the title parts and form the title
            decimal dec;
            var titleChunks = dataChunks.Where(c => !decimal.TryParse(c, out dec)).ToArray();
            string title = String.Join(" ", titleChunks).Trim();

            if (!String.IsNullOrEmpty(title))
            {
                jobClass.Title = title;
                foreach (var chunk in titleChunks)
                {
                    dataChunks.Remove(chunk);
                }
            }

            //the job step chunks are all that should remain
            return dataChunks;
        }

        /// <summary>
        /// Consumes a line of data to populate a JobClassStep
        /// </summary>
        /// <param name="dataChunks">A collection of salary step data points.</param>
        /// <returns>A JobClassStep object representation of the data.</returns>
        JobClassStep assignStepData(IEnumerable<string> dataChunks)
        {
            var step = new JobClassStep();
            //a job class step consists of 5 data points
            if (dataChunks.Count() == 5)
            {
                //convert to numeric and order increasing
                var numberChunks = dataChunks.Select(d => decimal.Parse(d)).OrderBy(d => d).ToArray();
                step.StepNumber = (int)numberChunks[0];                
                step.HourlyRate = numberChunks[1];
                step.BiWeeklyRate = numberChunks[2];
                step.MonthlyRate = numberChunks[3];
                step.AnnualRate = numberChunks[4];
            }
            else
            {
                throw new InvalidOperationException(String.Format("Couldn't parse step data: {0}", String.Join(" ", dataChunks)));
            }
                        
            return step;
        }
    }
}
