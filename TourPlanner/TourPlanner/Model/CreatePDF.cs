using iText.IO.Font.Constants;
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

namespace TourPlanner.Model
{
    internal class CreatePDF
    {
        private int tour_id { get; set; }
        private string path { get; set; }
        private string title { get; set; }

        public CreatePDF(string path)
        {
            this.path = path;
            title = "default";
        }

        public async Task CreateTourPDF(string title,int tour_id,bool pfad_mit_pdf_namen_angegeben)
        {
            this.title= title;
            this.tour_id = tour_id;
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(path))
                throw new Exception("Kein Title, Path oder Tour ID angegeben bei PDF erstellen");

            if (!pfad_mit_pdf_namen_angegeben) 
            {
                path = path + title + ".pdf";
            }

            database database = new database();
            Tour mytour = await database.GetTourAsync(tour_id);

            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph Title = new Paragraph("TOUR: " +title)
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

            /*
             * Bild dafür bracuhe ich die API NEMANJAAAA
             * */
            document.Close();
        }

        




    }
}
