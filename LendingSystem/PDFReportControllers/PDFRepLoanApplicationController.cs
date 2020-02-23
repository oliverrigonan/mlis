using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LendingSystem.PDFReportControllers
{
    public class PDFRepLoanApplicationController : Controller
    {
        public Data.lendingsystemDataContext db = new Data.lendingsystemDataContext();

        public ActionResult PDFRepLoanApplication(Int32 loanId)
        {
            var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

            MemoryStream memoryStream = new MemoryStream();
            Document document = new Document(PageSize.LETTER, 30f, 30f, 60f, 60f);

            PdfWriter pdfWriter = PdfWriter.GetInstance(document, memoryStream);
            pdfWriter.PageEvent = new LoanHeaderFooter();

            document.Open();

            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);

            var loan = from d in db.TrnLoans where d.Id == loanId && d.IsLocked == true select d;
            if (loan.Any())
            {
                String customer = loan.FirstOrDefault().MstCustomer.FullName;
                String term = loan.FirstOrDefault().MstTerm.Term;
                String loanNumber = loan.FirstOrDefault().LoanNumber;
                String loanDate = loan.FirstOrDefault().LoanDate.ToShortDateString();

                String PrincipalAmount = loan.FirstOrDefault().PrincipalAmount.ToString("#,##0.00");
                String InterestAmount = loan.FirstOrDefault().InterestAmount.ToString("#,##0.00");
                String LoanAmount = loan.FirstOrDefault().LoanAmount.ToString("#,##0.00");
                String NetAmount = loan.FirstOrDefault().NetAmount.ToString("#,##0.00");
                String AmortizationAmount = loan.FirstOrDefault().AmortizationAmount.ToString("#,##0.00");
                String PaidAmount = loan.FirstOrDefault().PaidAmount.ToString("#,##0.00");
                String Remarks = loan.FirstOrDefault().Remarks;
                String Status = loan.FirstOrDefault().Status;

                PdfPTable tableLoan = new PdfPTable(4);
                tableLoan.SetWidths(new float[] { 40f, 150f, 70f, 70f });
                tableLoan.WidthPercentage = 100;
                tableLoan.AddCell(new PdfPCell(new Phrase("Customer:", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableLoan.AddCell(new PdfPCell(new Phrase(customer, fontArial10)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableLoan.AddCell(new PdfPCell(new Phrase("Loan No.:", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableLoan.AddCell(new PdfPCell(new Phrase(loanNumber, fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableLoan.AddCell(new PdfPCell(new Phrase("Term:", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableLoan.AddCell(new PdfPCell(new Phrase(term, fontArial10)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableLoan.AddCell(new PdfPCell(new Phrase("Loan Date:", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableLoan.AddCell(new PdfPCell(new Phrase(loanDate, fontArial10)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableLoan.AddCell(new PdfPCell(new Phrase("Status:", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableLoan.AddCell(new PdfPCell(new Phrase(Status, fontArial10)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableLoan.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { Colspan = 2, Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                document.Add(tableLoan);

                PdfPTable spaceTable = new PdfPTable(1);
                spaceTable.SetWidths(new float[] { 100f });
                spaceTable.WidthPercentage = 100;
                spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { Border = 0, PaddingTop = 5f });
                document.Add(spaceTable);
                document.Add(spaceTable);
                document.Add(spaceTable);

                PdfPTable tableAmounts = new PdfPTable(3);
                tableAmounts.SetWidths(new float[] { 100f, 50f, 200f });
                tableAmounts.WidthPercentage = 80;
                tableAmounts.AddCell(new PdfPCell(new Phrase("Transaction", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase("Amount", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase("Remarks", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase("Principal", fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase(PrincipalAmount, fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase(" ", fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase("Interest", fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase(InterestAmount, fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase(" ", fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase("Total Loan", fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase(LoanAmount, fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase(" ", fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase("Net", fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase(NetAmount, fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase("Pricipal amount less interest", fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase("Monthly Amortization", fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase(AmortizationAmount, fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                tableAmounts.AddCell(new PdfPCell(new Phrase(" ", fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });

                document.Add(tableAmounts);
                document.Add(spaceTable);
                document.Add(spaceTable);
                document.Add(spaceTable);
                document.Add(spaceTable);
                document.Add(spaceTable);

                Paragraph signatureLine = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0F, 100.0F, BaseColor.BLACK, Element.ALIGN_MIDDLE, 5F)));

                PdfPTable tableUsers = new PdfPTable(2);
                tableUsers.SetWidths(new float[] { 100f, 200f });
                tableUsers.WidthPercentage = 100;
                tableUsers.AddCell(new PdfPCell(new Phrase(customer.ToUpper(), fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 0f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 0f, PaddingLeft = 5f, PaddingRight = 5f });

                tableUsers.AddCell(new PdfPCell(new Phrase(signatureLine)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 0f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 0f, PaddingLeft = 5f, PaddingRight = 5f });

                tableUsers.AddCell(new PdfPCell(new Phrase("Signature over printer name", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 0f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 0f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(tableUsers);
            }

            document.Close();

            byte[] bytesStream = memoryStream.ToArray();

            memoryStream = new MemoryStream();
            memoryStream.Write(bytesStream, 0, bytesStream.Length);
            memoryStream.Position = 0;

            return new FileStreamResult(memoryStream, "application/pdf");
        }
    }

    // =================
    // Header and Footer
    // =================
    class LoanHeaderFooter : PdfPageEventHelper
    {
        public LoanHeaderFooter()
        {

        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            Font fontArial09 = FontFactory.GetFont("Arial", 09);
            Font fontArial09Italic = FontFactory.GetFont("Arial", 09, Font.ITALIC);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD);

            Paragraph headerLine = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(2F, 100.0F, BaseColor.BLACK, Element.ALIGN_MIDDLE, 5F)));
            Paragraph footerLine = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0F, 100.0F, BaseColor.BLACK, Element.ALIGN_MIDDLE, 5F)));

            String softwareVersion = "Lending System V.202002240936";

            PdfPTable tableHeader = new PdfPTable(2);
            tableHeader.SetWidths(new float[] { 70f, 30f });
            tableHeader.DefaultCell.Border = 0;
            tableHeader.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            tableHeader.AddCell(new PdfPCell(new Phrase("LENDING SYSTEM INC.", fontArial13Bold)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Loan Details", fontArial13Bold)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
            tableHeader.AddCell(new PdfPCell(new Phrase(headerLine)) { Border = 0, Colspan = 2 });
            tableHeader.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 30, writer.DirectContent);

            PdfPTable tableFooter = new PdfPTable(2);
            tableFooter.SetWidths(new float[] { 70f, 50f });
            tableFooter.DefaultCell.Border = 0;
            tableFooter.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            tableFooter.AddCell(new PdfPCell(new Phrase(footerLine)) { Border = 0, Colspan = 2 });
            tableFooter.AddCell(new PdfPCell(new Phrase(softwareVersion, fontArial09)) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase("(Printed " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt") + ") Page " + writer.PageNumber, fontArial09)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
            tableFooter.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.BottomMargin) - 10, writer.DirectContent);
        }
    }
}