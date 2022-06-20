using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataLayer;
using TourPlanner.Model;

namespace TourPlanner.BusinessLayer.PDF
{
    internal class CreatePDF
    {
        private int tour_id { get; set; }
        private string path { get; set; }
        private string title { get; set; }

        public CreatePDF(string path)
        {
            this.path = path;
        }
        public CreatePDF()
        {

        }
        public async Task CreateTourPDF(string title, Tour mytour, bool pfad_mit_pdf_namen_angegeben)
        {
            this.title = title;
            this.tour_id = tour_id;
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(path))
                throw new Exception("Kein Title, Path oder Tour ID angegeben bei PDF erstellen");

            if (!pfad_mit_pdf_namen_angegeben)
            {
                path = "./" + title + ".pdf";
            }
            else
            {
                path = path + title + ".pdf";
            }

            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph Title = new Paragraph("TOUR: " + title)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(24)
                    .SetBold()
                    .SetFontColor(ColorConstants.BLACK);
            document.Add(Title);

            Paragraph Tour_name = new Paragraph($"Tour Name: {mytour.Name}")
              .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
              .SetFontSize(12);
            document.Add(Tour_name);

            Paragraph descipton = new Paragraph($"Description: {mytour.Tour_desc}")
               .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
               .SetFontSize(12);
            document.Add(descipton);

            Paragraph From = new Paragraph($"From: {mytour.From}")
              .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
              .SetFontSize(12);
            document.Add(From);

            Paragraph To = new Paragraph($"To: {mytour.To}")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(12);
            document.Add(To);

            Paragraph type = new Paragraph($"Transport Type: {mytour.Transport_type}")
              .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
              .SetFontSize(12);
            document.Add(type);

            Paragraph distance = new Paragraph($"Distance: {mytour.Distance}")
              .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
              .SetFontSize(12);
            document.Add(distance);

            Paragraph Time = new Paragraph($"Time: {mytour.Time}")
              .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
              .SetFontSize(12);
            document.Add(Time);

            Paragraph image = new Paragraph($"Image of " + mytour.Name)
              .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
              .SetFontSize(12);
            document.Add(image);

            ImageData imageData = ImageDataFactory.Create($"../../../PresentationLayer/tour_images/{mytour.ID}.png");
            document.Add(new Image(imageData));

            if (mytour.TourLogs.Count > 0)
            {
                document.Add(new AreaBreak());
                Paragraph Header_Tour_Los = new Paragraph($"Tour Logs")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(18);
                document.Add(Header_Tour_Los);
                foreach (var item in mytour.TourLogs)
                {
                    Paragraph Header = new Paragraph($"Tour Log " + mytour.TourLogs.IndexOf(item))
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(14);
                    document.Add(Header);

                    Paragraph Comment = new Paragraph($"Log Comment: {item.Comment}")
              .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
              .SetFontSize(12);
                    document.Add(Comment);

                    Paragraph Rating = new Paragraph($"Log Rating: {item.rating.ToString()}")
              .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
              .SetFontSize(12);
                    document.Add(Rating);

                    Paragraph Diff = new Paragraph($"Log Difficulty: {item.difficulty.ToString()}")
              .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
              .SetFontSize(12);
                    document.Add(Diff);

                    Paragraph total_time = new Paragraph($"Log Total Time: {item.total_time}")
              .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
              .SetFontSize(12);
                    document.Add(total_time);

                    Paragraph Date = new Paragraph($"Date/Time: {item.date_time.ToString()}")
              .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
              .SetFontSize(12);
                    document.Add(Date);
                }
            }

            document.Close();
        }

        public async Task CreateSummarize_report(List<Tour> tours)
        {
            title = "Summarize";
            path += "Summarize.pdf";

            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph Title = new Paragraph(title + " of Tours")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(24)
                    .SetBold()
                    .SetFontColor(ColorConstants.BLACK);
            document.Add(Title);


            foreach (var item in tours)
            {
                if (item.TourLogs.Count > 0)
                {
                    Paragraph Tour_name = new Paragraph($"Tour Name: {item.Name}")
                   .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                   .SetFontSize(18);
                    document.Add(Tour_name);

                    Paragraph total_time = new Paragraph($"Average Total Time: {item.TourLogs.Average(x => x.total_time)}")
                  .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                  .SetFontSize(12);
                    document.Add(total_time);

                    Paragraph distance = new Paragraph($"Distance: {item.Distance}")
                  .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                  .SetFontSize(12);
                    document.Add(distance);


                    Paragraph Rating = new Paragraph($"Average Rating: {item.TourLogs.Average(x => x.rating)}")
                  .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                  .SetFontSize(12);
                    document.Add(Rating);
                }
            }
            document.Close();

        }
    }
}
