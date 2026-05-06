using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using System.Data;

namespace McsCoreLib.Tools.Report
{
    public class ExcelHelper
    {
        public static DataTable ExcelToDatatable(Stream stream, string fileType, string sheetName = null)
        {
            DataTable dt = new();
            IWorkbook workbook;
            #region 判断excel版本
            //2007以上版本excel
            if (fileType == ".xlsx")
            {
                workbook = new XSSFWorkbook(stream);
            }
            //2007以下版本excel
            else if (fileType == ".xls")
            {
                workbook = new HSSFWorkbook(stream);
            }
            else
            {
                throw new Exception("传入的不是Excel文件！");
            }
            ISheet sheet;
            #endregion

            if (!string.IsNullOrEmpty(sheetName))
            {
                sheet = workbook.GetSheet(sheetName);
                sheet ??= workbook.GetSheetAt(0);
            }
            else
            {
                sheet = workbook.GetSheetAt(0);
            }

            if (sheet != null)
            {
                IRow firstRow = sheet.GetRow(0);
                int cellCount = firstRow.LastCellNum;
                for (int i = firstRow.FirstCellNum; i < cellCount; i++)
                {
                    ICell cell = firstRow.GetCell(i);
                    if (cell != null)
                    {
                        string cellValue = cell.StringCellValue.Trim();
                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            DataColumn dataColumn = new(cellValue);
                            dt.Columns.Add(dataColumn);
                        }
                    }
                }
                //遍历行
                for (int j = sheet.FirstRowNum + 1; j <= sheet.LastRowNum; j++)
                {
                    IRow row = sheet.GetRow(j);
                    DataRow dataRow = dt.NewRow();
                    if (row == null || row.FirstCellNum < 0)
                    {
                        continue;
                    }
                    //遍历列
                    for (int i = row.FirstCellNum; i < cellCount; i++)
                    {
                        ICell cellData = row.GetCell(i);
                        if (cellData != null)
                        {
                            //判断是否为数字型，必须加这个判断不然下面的日期判断会异常
                            if (cellData.CellType == CellType.Numeric)
                            {
                                //判断是否日期类型
                                if (DateUtil.IsCellDateFormatted(cellData))
                                {
                                    dataRow[i] = cellData.DateCellValue;
                                }
                                else
                                {
                                    dataRow[i] = cellData.ToString().Trim();
                                }
                            }
                            else
                            {
                                dataRow[i] = cellData.ToString().Trim();
                            }
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
            }
            else
            {
                throw new Exception("没有获取到Excel中的数据表！");
            }

            return dt;
        }
    }
}
