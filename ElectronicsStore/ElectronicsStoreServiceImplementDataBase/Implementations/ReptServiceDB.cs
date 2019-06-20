﻿using ElectronicsStoreServiceDAL.Interfaces;
using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ElectronicsStoreServiceDAL.BindingModel;
using ElectronicsStoreServiceDAL.ViewModel;
using System.Data.Entity.SqlServer;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Runtime.Serialization.Json;
using ElectronicsStoreModel;
using Task = System.Threading.Tasks.Task;

namespace ElectronicsStoreServiceImplementDataBase.Implementations
{
    public class ReptServiceDB : IReptService
    {
        private ElectronicsStoreDbContext context;
        private readonly IMainService service;
        public ReptServiceDB(ElectronicsStoreDbContext context, IMainService service)
        {
            this.context = context;
            this.service = service;
        }

        public void SaveProductPriceXls(ReptBindingModel model)
        {
            var excel = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                //или создаем excel-файл, или открываем существующий
                if (File.Exists(model.FileName))
                {
                    excel.Workbooks.Open(model.FileName, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing);
                }
                else
                {
                    excel.SheetsInNewWorkbook = 1;
                    excel.Workbooks.Add(Type.Missing);
                    excel.Workbooks[1].SaveAs(model.FileName, XlFileFormat.xlExcel8, Type.Missing,
                        Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }

                Sheets excelsheets = excel.Workbooks[1].Worksheets;
                //Получаем ссылку на лист
                var excelworksheet = (Worksheet)excelsheets.get_Item(1);
                //очищаем ячейки
                excelworksheet.Cells.Clear();
                //настройки страницы
                excelworksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                excelworksheet.PageSetup.CenterHorizontally = true;
                excelworksheet.PageSetup.CenterVertically = true;
                //получаем ссылку на первые 3 ячейки
                Microsoft.Office.Interop.Excel.Range excelcells = excelworksheet.get_Range("A1", "C1");
                //объединяем их
                excelcells.Merge(Type.Missing);
                //задаем текст, настройки шрифта и ячейки
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Список товаров";
                excelcells.RowHeight = 25;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 14;

                excelcells = excelworksheet.get_Range("A2", "C2");
                excelcells.Merge(Type.Missing);
                excelcells.Value2 = "на " + DateTime.Now.ToShortDateString();
                excelcells.RowHeight = 20;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 12;

                var dict = context.Products.ToList();
                for (int i = 0; i < dict.Count; i++)
                {
                    excelcells = excelworksheet.get_Range("C1", "C1");
                    excelcells = excelcells.get_Offset(i + 2, -2);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = dict[i].ProductName;
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = dict[i].Price;
                    excelcells.Font.Bold = true;
                }
                //сохраняем
                excel.Workbooks[1].Save();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //закрываем
                excel.Quit();
            }
        }

        public void SaveProductPriceW(ReptBindingModel model)
        {
            if (File.Exists(model.FileName))
            {
                File.Delete(model.FileName);
            }
            var winword = new Microsoft.Office.Interop.Word.Application();
            try
            {
                object missing = System.Reflection.Missing.Value;
                //создаем документ
                Microsoft.Office.Interop.Word.Document document =
                winword.Documents.Add(ref missing, ref missing, ref missing, ref
               missing);
                //получаем ссылку на параграф
                var paragraph = document.Paragraphs.Add(missing);
                var range = paragraph.Range;
                //задаем текст
                range.Text = "Прайс изделий";
                //задаем настройки шрифта
                var font = range.Font;
                font.Size = 16;
                font.Name = "Times New Roman";
                font.Bold = 1;
                //задаем настройки абзаца
                var paragraphFormat = range.ParagraphFormat;
                paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphFormat.SpaceAfter = 10;
                paragraphFormat.SpaceBefore = 0;
                //добавляем абзац в документ
                range.InsertParagraphAfter();
                var products = context.Products.ToList();
                //создаем таблицу
                var paragraphTable = document.Paragraphs.Add(Type.Missing);
                var rangeTable = paragraphTable.Range;
                var table = document.Tables.Add(rangeTable, products.Count, 2, ref
               missing, ref missing);
                font = table.Range.Font;
                font.Size = 14;
                font.Name = "Times New Roman";
                var paragraphTableFormat = table.Range.ParagraphFormat;
                paragraphTableFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphTableFormat.SpaceAfter = 0;
                paragraphTableFormat.SpaceBefore = 0;
                for (int i = 0; i < products.Count; ++i)
                {
                    table.Cell(i + 1, 1).Range.Text = products[i].ProductName;
                    table.Cell(i + 1, 2).Range.Text = products[i].Price.ToString();
                }
                //задаем границы таблицы
                table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleInset;
                table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
                paragraph = document.Paragraphs.Add(missing);
                range = paragraph.Range;
                range.Text = "Дата: " + DateTime.Now.ToLongDateString();
                font = range.Font;
                font.Size = 12;
                font.Name = "Times New Roman";
                paragraphFormat = range.ParagraphFormat;
                paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphFormat.SpaceAfter = 10;
                paragraphFormat.SpaceBefore = 10;
                range.InsertParagraphAfter();
                //сохраняем
                object fileFormat = WdSaveFormat.wdFormatXMLDocument;
                document.SaveAs(model.FileName, ref fileFormat, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing);
                document.Close(ref missing, ref missing, ref missing);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                winword.Quit();
            }
        }

        public List<IndentViewModel> GetCustomerIndents(ReptBindingModel model)
        {
            return context.Indents
            .Include(rec => rec.Customer)
           .Include(rec => rec.IndentProducts)
           .Where(rec => rec.DateCreate >= model.DateFrom &&
           rec.DateCreate <= model.DateTo)
            .Select(rec => new IndentViewModel
            {
                Id = rec.Id,
                CustomerFIO = rec.Customer.CustomerFIO,
                CustomerId = rec.Customer.Id,
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate)
           + " " +
            SqlFunctions.DateName("mm", rec.DateCreate) +
           " " +
            SqlFunctions.DateName("yyyy",
           rec.DateCreate),
                Sum = rec.Sum,
                Status = rec.Status,

                IndentPayments = context.IndentPayments
                .Where(recPC => recPC.IndentId == rec.Id)
                .Select(recPC => new IndentPaymentViewModel
                {
                    Id = recPC.Id,
                    IndentId = recPC.IndentId,
                    DatePayment = recPC.DatePayment,
                    SumPayment = recPC.SumPayment
                })
                .ToList()

            })
           .ToList();
        }

        public List<IndentViewModel> GetCustomerIndentsForClients(ReptBindingModel model, int customerId)
        {
            return context.Indents
            .Include(rec => rec.Customer)
           .Include(rec => rec.IndentProducts)
           .Where(rec => rec.DateCreate >= model.DateFrom &&
           rec.DateCreate <= model.DateTo)
           .Where(rec => rec.CustomerId == customerId)
            .Select(rec => new IndentViewModel
            {
                Id = rec.Id,
                CustomerFIO = rec.Customer.CustomerFIO,
                CustomerId = rec.Customer.Id,
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate)
           + " " +
            SqlFunctions.DateName("mm", rec.DateCreate) +
           " " +
            SqlFunctions.DateName("yyyy",
           rec.DateCreate),
                Sum = rec.Sum,
                Status = rec.Status,

                IndentPayments = context.IndentPayments
                .Where(recPC => recPC.IndentId == rec.Id)
                .Select(recPC => new IndentPaymentViewModel
                {
                    Id = recPC.Id,
                    IndentId = recPC.IndentId,
                    DatePayment = recPC.DatePayment,
                    SumPayment = recPC.SumPayment
                })
                .ToList()

            })
           .ToList();
        }

        public List<IndentPaymentViewModel> GetIndentInfo(ReptBindingModel model)
        {
            List<IndentPaymentViewModel> payment = new List<IndentPaymentViewModel>();
            foreach (var indent in GetCustomerIndents(model))
            {
                if (indent.IndentPayments.Count == 0)
                {
                    payment.Add(new IndentPaymentViewModel
                    {
                        DatePayment = null,
                        SumPayment = service.GetBalance(indent.Id)
                    });
                }
                else
                {
                    payment.Add(new IndentPaymentViewModel
                    {
                        DatePayment = indent.IndentPayments.ToList().LastOrDefault().DatePayment,
                        SumPayment = service.GetBalance(indent.Id)
                    });
                }
            }
            return payment;
        }

        public List<IndentPaymentViewModel> GetIndentInfoForClients(ReptBindingModel model, int customerId)
        {
            List<IndentPaymentViewModel> payment = new List<IndentPaymentViewModel>();
            foreach (var indent in GetCustomerIndentsForClients(model, customerId))
            {
                if (indent.IndentPayments.Count == 0)
                {
                    payment.Add(new IndentPaymentViewModel
                    {
                        DatePayment = null,
                        SumPayment = service.GetBalance(indent.Id)
                    });
                }
                else
                {
                    payment.Add(new IndentPaymentViewModel
                    {
                        DatePayment = indent.IndentPayments.ToList().LastOrDefault().DatePayment,
                        SumPayment = service.GetBalance(indent.Id)
                    });
                }
            }
            return payment;
        }

        public void SendEmail(ReptBindingModel model)
        {
            string mailAddress = null;
            string subject = "Оповещение по заказам";
            string text = null;
            System.Net.Mail.MailMessage objMailMessage = new System.Net.Mail.MailMessage();
            SmtpClient objSmtpClient = null;

            var list = GetCustomerIndents(model);
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Status == ElectronicsStoreModel.IndentStatus.Оплачен)
                    {
                        continue;
                    }
                    mailAddress = context.Customers.ToList().FirstOrDefault(rec1 => rec1.Id == list[i].CustomerId).Email;
                    text = $"{list[i].CustomerFIO}, Ваш заказ №{list[i].Id} от {list[i].DateCreate} не оплачен";

                    string login = ConfigurationManager.AppSettings["MailLogin"];
                    objMailMessage.From = new
                   MailAddress(login);
                    objMailMessage.To.Add(new MailAddress(mailAddress));
                    objMailMessage.Subject = subject;
                    objMailMessage.Body = text;
                    objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                    objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                    objSmtpClient.UseDefaultCredentials = false;
                    objSmtpClient.EnableSsl = true;
                    objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    objSmtpClient.Credentials = new
                   NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
                   ConfigurationManager.AppSettings["MailPassword"]);
                    objSmtpClient.Send(objMailMessage);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }

        private void SendEmailForClients(string mailAddress, string subject, string text, string attachmentPath)
        {
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage();
            SmtpClient smtpClient = null;
            try
            {
                m.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                m.To.Add(new MailAddress(mailAddress));
                m.Subject = subject;
                m.Body = text;
                m.SubjectEncoding = System.Text.Encoding.UTF8;
                m.BodyEncoding = System.Text.Encoding.UTF8;
                m.Attachments.Add(new Attachment(attachmentPath));
                smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["MailLogin"],
                    ConfigurationManager.AppSettings["MailPassword"]
                    );
                smtpClient.Send(m);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m = null;
                smtpClient = null;
            }
        }

        public void SaveCustomerIndents(ReptBindingModel model)
        {
            //из ресрусов получаем шрифт для кирилицы
            if (!File.Exists("TIMCYR.TTF"))
            {
                File.WriteAllBytes("TIMCYR.TTF", Properties.Resources.TIMCYR);
            }
            //открываем файл для работы
            FileStream fs = new FileStream(model.FileName, FileMode.OpenOrCreate,
           FileAccess.Write);
            //создаем документ, задаем границы, связываем документ и поток
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont("TIMCYR.TTF", BaseFont.IDENTITY_H,
           BaseFont.NOT_EMBEDDED);
            //вставляем заголовок
            var phraseTitle = new Phrase("Заказы клиентов",
            new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
            iTextSharp.text.Paragraph paragraph = new
           iTextSharp.text.Paragraph(phraseTitle)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);
            var phrasePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString()
           +
            " по " + model.DateTo.Value.ToShortDateString(),
           new iTextSharp.text.Font(baseFont, 14,
           iTextSharp.text.Font.BOLD));
            paragraph = new iTextSharp.text.Paragraph(phrasePeriod)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);
            //вставляем таблицу, задаем количество столбцов, и ширину колонок
            PdfPTable table = new PdfPTable(6)
            {
                TotalWidth = 800F
            };
            table.SetTotalWidth(new float[] { 160, 140, 160, 100, 100, 140 });
            //вставляем шапку
            PdfPCell cell = new PdfPCell();
            var fontForCellBold = new iTextSharp.text.Font(baseFont, 10,
           iTextSharp.text.Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("ФИО клиента", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Дата создания", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Сумма", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Статус", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Дата последней оплаты", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Остаток", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            //заполняем таблицу
            var list = GetCustomerIndents(model);
            var fontForCells = new iTextSharp.text.Font(baseFont, 10);
            for (int i = 0; i < list.Count; i++)
            {
                cell = new PdfPCell(new Phrase(list[i].CustomerFIO, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].DateCreate, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Sum.ToString(), fontForCells));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Status.ToString(), fontForCells));
                table.AddCell(cell);

                var payment = context.IndentPayments.ToList().LastOrDefault(rec1 => rec1.IndentId == list[i].Id);
                if (payment != null)
                {
                    cell = new PdfPCell(new Phrase(payment.DatePayment.ToShortDateString(), fontForCells));
                    table.AddCell(cell);
                }
                else
                {
                    cell = new PdfPCell(new Phrase("Оплат нет", fontForCells));
                    table.AddCell(cell);
                }
                cell = new PdfPCell(new Phrase(service.GetBalance(list[i].Id).ToString(), fontForCells));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
            }
            //вставляем итого
            cell = new PdfPCell(new Phrase("Итого:", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Colspan = 2,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(list.Sum(rec => rec.Sum).ToString(),
           fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", fontForCellBold))
            {
                Colspan = 3,
                Border = 0
            };
            table.AddCell(cell);
            //вставляем таблицу
            doc.Add(table);
            doc.Close();
        }

        public void SaveCustomerIndentsForClients(ReptBindingModel model, int customerId)
        {
            //из ресрусов получаем шрифт для кирилицы
            if (!File.Exists("TIMCYR.TTF"))
            {
                File.WriteAllBytes("TIMCYR.TTF", Properties.Resources.TIMCYR);
            }
            //открываем файл для работы
            FileStream fs = new FileStream(model.FileName, FileMode.OpenOrCreate,
           FileAccess.Write);
            //создаем документ, задаем границы, связываем документ и поток
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont("TIMCYR.TTF", BaseFont.IDENTITY_H,
           BaseFont.NOT_EMBEDDED);
            //вставляем заголовок
            var phraseTitle = new Phrase("Заказы клиента",
            new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
            iTextSharp.text.Paragraph paragraph = new
           iTextSharp.text.Paragraph(phraseTitle)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);
            var phrasePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString()
           +
            " по " + model.DateTo.Value.ToShortDateString(),
           new iTextSharp.text.Font(baseFont, 14,
           iTextSharp.text.Font.BOLD));
            paragraph = new iTextSharp.text.Paragraph(phrasePeriod)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);
            //вставляем таблицу, задаем количество столбцов, и ширину колонок
            PdfPTable table = new PdfPTable(6)
            {
                TotalWidth = 800F
            };
            table.SetTotalWidth(new float[] { 160, 140, 160, 100, 100, 140 });
            //вставляем шапку
            PdfPCell cell = new PdfPCell();
            var fontForCellBold = new iTextSharp.text.Font(baseFont, 10,
           iTextSharp.text.Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("ФИО клиента", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Дата создания", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Сумма", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Статус", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Дата последней оплаты", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Остаток", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            //заполняем таблицу
            var list = GetCustomerIndentsForClients(model, customerId);
            var fontForCells = new iTextSharp.text.Font(baseFont, 10);
            for (int i = 0; i < list.Count; i++)
            {
                cell = new PdfPCell(new Phrase(list[i].CustomerFIO, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].DateCreate, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Sum.ToString(), fontForCells));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Status.ToString(), fontForCells));
                table.AddCell(cell);

                var payment = context.IndentPayments.ToList().LastOrDefault(rec1 => rec1.IndentId == list[i].Id);
                if (payment != null)
                {
                    cell = new PdfPCell(new Phrase(payment.DatePayment.ToShortDateString(), fontForCells));
                    table.AddCell(cell);
                }
                else
                {
                    cell = new PdfPCell(new Phrase("Оплат нет", fontForCells));
                    table.AddCell(cell);
                }
                cell = new PdfPCell(new Phrase(service.GetBalance(list[i].Id).ToString(), fontForCells));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
            }
            //вставляем итого
            cell = new PdfPCell(new Phrase("Итого:", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Colspan = 2,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(list.Sum(rec => rec.Sum).ToString(),
           fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", fontForCellBold))
            {
                Colspan = 3,
                Border = 0
            };
            table.AddCell(cell);
            //вставляем таблицу
            doc.Add(table);
            doc.Close();

            SendEmailForClients(model.Email, "Оповещение по заказам", "", model.FileName);
        }

        public void SaveIndentCustomerXls(ReptBindingModel model, CustomerViewModel customer)
        {
            var excel = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                //или создаем excel-файл, или открываем существующий
                if (File.Exists(model.FileName))
                {
                    File.Delete(model.FileName);
                }
                    excel.SheetsInNewWorkbook = 1;
                    excel.Workbooks.Add(Type.Missing);
                    excel.Workbooks[1].SaveAs(model.FileName, XlFileFormat.xlExcel8, Type.Missing,
                        Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                

                Sheets excelsheets = excel.Workbooks[1].Worksheets;
                //Получаем ссылку на лист
                var excelworksheet = (Worksheet)excelsheets.get_Item(1);
                //очищаем ячейки
                excelworksheet.Cells.Clear();
                //настройки страницы
                excelworksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                excelworksheet.PageSetup.CenterHorizontally = true;
                excelworksheet.PageSetup.CenterVertically = true;
                //получаем ссылку на первые 3 ячейки
                Microsoft.Office.Interop.Excel.Range excelcells = excelworksheet.get_Range("A1", "G1");
                //объединяем их
                excelcells.Merge(Type.Missing);
                //задаем текст, настройки шрифта и ячейки
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Заказы клиента";
                excelcells.RowHeight = 25;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 14;

                excelcells = excelworksheet.get_Range("A2", "G2");
                excelcells.Merge(Type.Missing);
                excelcells.Value2 = "на " + DateTime.Now.ToShortDateString();
                excelcells.RowHeight = 20;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 12;

                var dict = customer.Indents.ToList();
                for (int i = 0; i < dict.Count; i++)
                {
                    excelcells = excelworksheet.get_Range("C1", "C1");
                    excelcells = excelcells.get_Offset(i + 2, -2);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = dict[i].Id.ToString();
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = dict[i].DateCreate;
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = dict[i].Sum.ToString();
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = dict[i].Status.ToString();

                    var payment = context.IndentPayments.ToList().LastOrDefault(rec1 => rec1.IndentId == dict[i].Id);
                    if (payment != null)
                    {
                        excelcells = excelcells.get_Offset(0, 1);
                        excelcells.ColumnWidth = 15;
                        excelcells.Value2 = payment.DatePayment.ToShortDateString();
                    }
                    else
                    {
                        excelcells = excelcells.get_Offset(0, 1);
                        excelcells.ColumnWidth = 15;
                        excelcells.Value2 = "Оплат нет";
                    }

                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = service.GetBalance(dict[i].Id).ToString();

                    excelcells.Font.Bold = true;
                }
                //сохраняем
                excel.Workbooks[1].Save();
                excel.Workbooks[1].Close();
            }
            catch (Exception)
            {
                excel.Workbooks[1].Close();
                excel.Quit();
                throw;
            }
            finally
            {
                excel.Quit();
                SendEmailForClients(customer.Email, "Оповещение по заказам", "", model.FileName);
            }
        }

        public void SaveIndentCustomerW(ReptBindingModel model, CustomerViewModel customer)
        {
            if (File.Exists(model.FileName))
            {
                File.Delete(model.FileName);
            }
            var winword = new Microsoft.Office.Interop.Word.Application();
            try
            {
                object missing = System.Reflection.Missing.Value;
                //создаем документ
                Microsoft.Office.Interop.Word.Document document =
                winword.Documents.Add(ref missing, ref missing, ref missing, ref
               missing);
                //получаем ссылку на параграф
                var paragraph = document.Paragraphs.Add(missing);
                var range = paragraph.Range;
                //задаем текст
                range.Text = "Заказы клиента";
                //задаем настройки шрифта
                var font = range.Font;
                font.Size = 16;
                font.Name = "Times New Roman";
                font.Bold = 1;
                //задаем настройки абзаца
                var paragraphFormat = range.ParagraphFormat;
                paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphFormat.SpaceAfter = 10;
                paragraphFormat.SpaceBefore = 0;
                //добавляем абзац в документ
                range.InsertParagraphAfter();
                var indents = customer.Indents;
                //создаем таблицу
                var paragraphTable = document.Paragraphs.Add(Type.Missing);
                var rangeTable = paragraphTable.Range;
                var table = document.Tables.Add(rangeTable, indents.Count, 7, ref
               missing, ref missing);
                font = table.Range.Font;
                font.Size = 14;
                font.Name = "Times New Roman";
                var paragraphTableFormat = table.Range.ParagraphFormat;
                paragraphTableFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphTableFormat.SpaceAfter = 0;
                paragraphTableFormat.SpaceBefore = 0;
                for (int i = 0; i < indents.Count; ++i)
                {
                    table.Cell(i + 1, 1).Range.Text = indents[i].Id.ToString();
                    table.Cell(i + 1, 2).Range.Text = indents[i].DateCreate;
                    table.Cell(i + 1, 3).Range.Text = indents[i].Sum.ToString();
                    table.Cell(i + 1, 4).Range.Text = indents[i].Status.ToString();
                                        
                    var payment = context.IndentPayments.ToList().LastOrDefault(rec1 => rec1.IndentId == indents[i].Id);
                    if (payment != null)
                    {
                        table.Cell(i + 1, 5).Range.Text = payment.DatePayment.ToShortDateString();
                    }
                    else
                    {
                        table.Cell(i + 1, 6).Range.Text = "Оплат нет";
                    }
                    table.Cell(i + 1, 7).Range.Text = service.GetBalance(indents[i].Id).ToString();
                }
                //задаем границы таблицы
                table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleInset;
                table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
                paragraph = document.Paragraphs.Add(missing);
                range = paragraph.Range;
                range.Text = "Дата: " + DateTime.Now.ToLongDateString();
                font = range.Font;
                font.Size = 12;
                font.Name = "Times New Roman";
                paragraphFormat = range.ParagraphFormat;
                paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphFormat.SpaceAfter = 10;
                paragraphFormat.SpaceBefore = 10;
                range.InsertParagraphAfter();
                //сохраняем
                object fileFormat = WdSaveFormat.wdFormatXMLDocument;
                document.SaveAs(model.FileName, ref fileFormat, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing);
                document.Close(ref missing, ref missing, ref missing);
            }
            catch (Exception)
            {
                winword.Quit();
                throw;
            }
            finally
            {
                winword.Quit();
                SendEmailForClients(customer.Email, "Оповещение по заказам", "", model.FileName);
            }
        }
    }
}
