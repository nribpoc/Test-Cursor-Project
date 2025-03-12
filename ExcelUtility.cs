using System;
using System.Data;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;
using log4net;
using System.IO;

namespace Test
{
    public class ExcelUtility
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExcelUtility));

        /// <summary>
        /// 读取Excel表格内容并转换为DataTable
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="isFirstRowAsHeader">是否将第一行作为列名</param>
        /// <param name="sheetName">要读取的工作表名称</param>
        /// <param name="startRowIndex">开始读取的行索引（从1开始）</param>
        /// <returns>包含Excel数据的DataTable</returns>
        public static DataTable ExcelToDataTable(string filePath, bool isFirstRowAsHeader, string sheetName, int startRowIndex)
        {
            DataTable dt = new DataTable();
            
            try
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    Sheet sheet = workbookPart.Workbook.Descendants<Sheet>()
                        .FirstOrDefault(s => s.Name.Value.Equals(sheetName, StringComparison.OrdinalIgnoreCase));
                    
                    if (sheet == null)
                        throw new Exception($"未找到名为 {sheetName} 的工作表");

                    WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;

                    var rows = sheetData.Elements<Row>().ToList();
                    if (rows.Count == 0)
                    {
                        log.Warn($"工作表 {sheetName} 中没有数据。");
                        return dt;
                    }

                    // 获取最后一列的引用
                    string lastCellReference = worksheetPart.Worksheet
                        .Descendants<Cell>()
                        .Where(c => c.CellReference?.Value != null)
                        .Max(c => c.CellReference.Value);
                    
                    // 计算最大列数
                    string lastColumnName = new string(lastCellReference.TakeWhile(c => !char.IsDigit(c)).ToArray());
                    int columnCount = GetColumnIndex(lastColumnName) + 1;

                    // 处理列头
                    if (isFirstRowAsHeader)
                    {
                        // 对于テナント名変換定義等文件，使用第一行作为列头
                        if (startRowIndex <= 1)
                        {
                            var headerRow = rows[0];
                            for (int i = 0; i < columnCount; i++)
                            {
                                string columnName = GetCellValue(GetCellByColumnIndex(headerRow, i), stringTablePart);
                                dt.Columns.Add(string.IsNullOrEmpty(columnName) ? $"Column{i + 1}" : columnName);
                            }
                            startRowIndex = 1; // 从第二行开始读取数据
                        }
                        // 对于対象外設定等文件，使用指定行作为列头
                        else
                        {
                            var headerRow = rows[startRowIndex - 1]; // 因为Excel行号从1开始
                            for (int i = 0; i < columnCount; i++)
                            {
                                string columnName = GetCellValue(GetCellByColumnIndex(headerRow, i), stringTablePart);
                                dt.Columns.Add(string.IsNullOrEmpty(columnName) ? $"Column{i + 1}" : columnName);
                            }
                        }
                    }
                    else
                    {
                        // 使用默认列名
                        for (int i = 0; i < columnCount; i++)
                        {
                            dt.Columns.Add($"Column{i + 1}");
                        }
                    }

                    // 读取数据行
                    int dataStartRow = isFirstRowAsHeader ? startRowIndex : startRowIndex - 1;
                    for (int rowIndex = dataStartRow; rowIndex < rows.Count; rowIndex++)
                    {
                        var row = rows[rowIndex];
                        DataRow dataRow = dt.NewRow();

                        // 获取该行所有单元格
                        var cellsDictionary = row.Elements<Cell>()
                            .ToDictionary(
                                cell => GetColumnIndexFromReference(cell.CellReference),
                                cell => cell
                            );

                        // 填充每一列的数据
                        for (int i = 0; i < columnCount; i++)
                        {
                            if (cellsDictionary.TryGetValue(i, out Cell cell))
                            {
                                dataRow[i] = GetCellValue(cell, stringTablePart);
                            }
                            else
                            {
                                dataRow[i] = string.Empty; // 空单元格
                            }
                        }

                        dt.Rows.Add(dataRow);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Excelファイル読み込みでエラーが発生しました。ファイル：{filePath}, シート：{sheetName}, 開始行：{startRowIndex}", ex);
                throw new Exception($"读取Excel文件时发生错误: {ex.Message}");
            }
            
            return dt;
        }

        /// <summary>
        /// 获取单元格的值
        /// </summary>
        private static string GetCellValue(Cell cell, SharedStringTablePart stringTablePart)
        {
            if (cell == null) return string.Empty;
            
            string value = cell.InnerText;
            
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
            }
            
            return value;
        }

        /// <summary>
        /// 获取单元格的显示值（忽略公式）
        /// </summary>
        private static string GetCellDisplayValue(Cell cell, SharedStringTablePart stringTablePart)
        {
            if (cell == null) return string.Empty;

            // 如果单元格有值
            if (cell.CellValue != null)
            {
                // 如果是共享字符串
                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    return stringTablePart.SharedStringTable
                        .ElementAt(int.Parse(cell.CellValue.Text))
                        .InnerText;
                }
                // 返回实际值
                return cell.CellValue.Text;
            }
            
            return string.Empty;
        }

        /// <summary>
        /// 根据列索引获取单元格
        /// </summary>
        private static Cell GetCellByColumnIndex(Row row, int columnIndex)
        {
            string columnName = GetColumnName(columnIndex);
            return row.Elements<Cell>()
                .FirstOrDefault(c => string.Equals(
                    GetColumnName(GetColumnIndexFromReference(c.CellReference)),
                    columnName,
                    StringComparison.OrdinalIgnoreCase
                )) ?? new Cell();
        }

        /// <summary>
        /// 从单元格引用获取列索引
        /// </summary>
        private static int GetColumnIndexFromReference(string cellReference)
        {
            if (string.IsNullOrEmpty(cellReference)) return 0;
            string columnName = new string(cellReference.TakeWhile(c => !char.IsDigit(c)).ToArray());
            return GetColumnIndex(columnName);
        }

        /// <summary>
        /// 将列名转换为索引
        /// </summary>
        private static int GetColumnIndex(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return 0;
            
            int sum = 0;
            for (int i = 0; i < columnName.Length; i++)
            {
                sum *= 26;
                sum += (columnName[i] - 'A' + 1);
            }
            return sum - 1;
        }

        /// <summary>
        /// 将索引转换为列名
        /// </summary>
        private static string GetColumnName(int columnIndex)
        {
            string columnName = string.Empty;
            columnIndex++;

            while (columnIndex > 0)
            {
                int modulo = (columnIndex - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnIndex = (columnIndex - modulo) / 26;
            }

            return columnName;
        }

        /// <summary>
        /// 根据指定的列名将Excel数据读取为多个DataTable并存入DataSet
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetName">要读取的工作表名称</param>
        /// <param name="headerColumnName">作为表头的列名</param>
        /// <returns>包含多个DataTable的DataSet</returns>
        public static DataSet ExcelToDataSet(string filePath, string sheetName, string headerColumnName)
        {
            DataSet ds = new DataSet();
            
            try
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    Sheet sheet = workbookPart.Workbook.Descendants<Sheet>()
                        .FirstOrDefault(s => s.Name.Value.Equals(sheetName, StringComparison.OrdinalIgnoreCase));
                    
                    if (sheet == null)
                        throw new Exception($"未找到名为 {sheetName} 的工作表");

                    WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;

                    var rows = sheetData.Elements<Row>().ToList();
                    DataTable currentTable = null;
                    int startColumnIndex = -1;
                    int awsAccountIdColumnIndex = -1;

                    for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
                    {
                        var row = rows[rowIndex];
                        var cells = row.Elements<Cell>().ToList();

                        // 检查是否有■标记的行
                        string firstCellValue = GetCellValue(cells.FirstOrDefault(), stringTablePart);
                        if (firstCellValue.Contains("■"))
                        {
                            // 如果已有表在处理中，先保存它
                            if (currentTable != null && currentTable.Rows.Count > 0)
                            {
                                ds.Tables.Add(currentTable);
                            }

                            // 获取表名（去除■符号）
                            string tableName = firstCellValue.Replace("■", "").Trim();

                            // 获取下一行作为列头
                            if (rowIndex + 1 < rows.Count)
                            {
                                var headerRow = rows[rowIndex + 1];
                                var headerCells = headerRow.Elements<Cell>().ToList();

                                // 查找headerColumnName和AWS Account ID所在的列索引
                                startColumnIndex = -1;
                                awsAccountIdColumnIndex = -1;
                                for (int i = 0; i < headerCells.Count; i++)
                                {
                                    string cellValue = GetCellValue(GetCellByColumnIndex(headerRow, i), stringTablePart);
                                    if (cellValue == headerColumnName)
                                    {
                                        startColumnIndex = i;
                                    }
                                    else if (cellValue == "AWS Account ID")
                                    {
                                        awsAccountIdColumnIndex = i;
                                    }

                                    if (startColumnIndex != -1 && awsAccountIdColumnIndex != -1)
                                    {
                                        break;
                                    }
                                }

                                if (startColumnIndex == -1)
                                {
                                    log.Warn($"表 {tableName} 中未找到列 {headerColumnName}");
                                    continue;
                                }

                                // 创建新表
                                currentTable = new DataTable(tableName);

                                // 添加列（从headerColumnName所在列开始）
                                for (int i = startColumnIndex; i < headerCells.Count; i++)
                                {
                                    string columnName = GetCellValue(GetCellByColumnIndex(headerRow, i), stringTablePart).Replace("メイ", "");
                                    if (!string.IsNullOrEmpty(columnName))
                                    {
                                        currentTable.Columns.Add(columnName);
                                    }
                                }

                                rowIndex++; // 跳过列头行
                            }
                        }
                        // 处理数据行
                        else if (currentTable != null && startColumnIndex != -1)
                        {
                            // 检查headerColumnName列的值
                            string headerColumnValue = GetCellDisplayValue(GetCellByColumnIndex(row, startColumnIndex), stringTablePart);
                            
                            // 检查AWS Account ID列是否有数据
                            string awsAccountId = string.Empty;
                            if (awsAccountIdColumnIndex != -1)
                            {
                                awsAccountId = GetCellDisplayValue(GetCellByColumnIndex(row, awsAccountIdColumnIndex), stringTablePart);
                            }

                            // 只有当AWS Account ID有值时才添加行
                            if (!string.IsNullOrEmpty(awsAccountId))
                            {
                                DataRow dataRow = currentTable.NewRow();
                                int columnIndex = 0;

                                // 从headerColumnName列开始读取数据
                                for (int i = startColumnIndex; i < cells.Count && columnIndex < currentTable.Columns.Count; i++)
                                {
                                    var cell = GetCellByColumnIndex(row, i);
                                    // 对于第一列使用 GetCellDisplayValue，其他列使用原来的 GetCellValue
                                    string value = (columnIndex == 0) 
                                        ? GetCellDisplayValue(cell, stringTablePart)
                                        : GetCellValue(cell, stringTablePart);
                                    dataRow[columnIndex++] = value;
                                }

                                currentTable.Rows.Add(dataRow);
                            }
                        }
                    }

                    // 添加最后一个表
                    if (currentTable != null && currentTable.Rows.Count > 0)
                    {
                        ds.Tables.Add(currentTable);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Excelファイルの読み込みでエラーが発生しました。ファイル：{filePath}", ex);
                throw new Exception($"读取Excel文件时发生错误: {ex.Message}");
            }
            
            return ds;
        }

        /// <summary>
        /// 写入下载历史记录
        /// </summary>
        /// <param name="reportId">帳票ID</param>
        /// <param name="result">比較結果</param>
        /// <param name="historyFilePath">历史文件路径</param>
        public static void WriteDownloadHistory(string reportId, string result, string historyFilePath,DataTable mergedTable,int index)
        {
            try
            {
                log.Info($"写入下载历史到文件: {historyFilePath}");
                var currentAccount = mergedTable.Rows[index]["IAMエイリアス"].ToString(); // 使用当前账号

                using (SpreadsheetDocument spreadsheet = SpreadsheetDocument.Open(historyFilePath, true))
                {
                    WorkbookPart workbookPart = spreadsheet.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                    // 找到最后一行的行号
                    uint lastRowNumber = 1;
                    if (sheetData.Elements<Row>().Any())
                    {
                        lastRowNumber = sheetData.Elements<Row>().Max(r => r.RowIndex.Value) + 1;
                    }

                    // 创建新行
                    Row newRow = new Row { RowIndex = lastRowNumber };

                    // 创建单元格
                    Cell[] cells = new Cell[6];
                    string[] values = new string[]
                    {
                        DateTime.Now.ToString("yyyy/MM/dd"),
                        DateTime.Now.ToLongTimeString(),
                        currentAccount,
                        mergedTable.Rows[index]["テナント名"].ToString(),
                        reportId,
                        result
                    };

                    // 创建单元格并设置值
                    for (int i = 0; i < 6; i++)
                    {
                        cells[i] = new Cell
                        {
                            CellReference = GetColumnName(i) + lastRowNumber,
                            DataType = CellValues.String,
                            CellValue = new CellValue(values[i])
                        };
                        newRow.AppendChild(cells[i]);
                    }

                    // 添加新行到工作表
                    sheetData.AppendChild(newRow);

                    // 保存更改
                    worksheetPart.Worksheet.Save();
                    log.Info("下载历史记录已写入");
                }
            }
            catch (Exception ex)
            {
                log.Error("写入下载历史时发生错误", ex);
            }
        }
    }
} 