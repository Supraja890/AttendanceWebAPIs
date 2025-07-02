using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;

namespace WHOTrackingWebAPI.Helpers
{
    public class ExcelHelper
    {
        public static void WriteUserPresence(string filePath, string ipAddress, string location, string name)
        {
            bool fileExists = File.Exists(filePath);
            if (!fileExists)
            {
                CreateExcelFile(filePath);
            }

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, true))
            {
                WorkbookPart workbookPart = document.WorkbookPart!;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>()!;

                // Add new row
                Row newRow = new Row();
                UserInfo userInfo = new UserInfo();
                newRow.Append(
                    CreateCell(DateTime.Now.ToString("yyyy-MM-dd")),
                    CreateCell(ipAddress),
                    CreateCell(location),
                    CreateCell(name)
                );

                sheetData.AppendChild(newRow);
                worksheetPart.Worksheet.Save();
            }
        }

        private static void CreateExcelFile(string filePath)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = document.WorkbookPart!.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Presence"
                };
                sheets.Append(sheet);

                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>()!;
                Row headerRow = new Row();
                headerRow.Append(
                    CreateCell("Timestamp"),
                    CreateCell("IP Address"),
                    CreateCell("Location"),
                    CreateCell("Name")
                );
                sheetData.AppendChild(headerRow);

                workbookPart.Workbook.Save();
            }
        }

        private static Cell CreateCell(string text)
        {
            return new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(text)
            };
        }
    }

}
