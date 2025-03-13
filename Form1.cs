using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using OpenQA.Selenium;
using DocumentFormat.OpenXml;
using System.IO;
using System.Collections;

namespace Test
{
    public partial class Form1 : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Form1));

        public Form1()
        {
            // 初始化 log4net
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
            InitializeComponent();
            
            // 设置窗体标题
            this.Text = "INVOICE DOWNLOAD123";
            
            // 设置窗体启动位置为屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // 禁用最大化和最小化按钮
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // 设置窗体大小
            this.Width = 800;
            this.Height = 400;
            
            // 设置窗体边框样式
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        // 添加文件选择事件处理程序
        private void btnAWSAccount_Click(object sender, EventArgs e)
        {
            log.Info("AWSアカウント一覧ファイル選択処理を開始します。");
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv|All Files|*.*";
                    openFileDialog.FilterIndex = 1;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        txtAWSAccount.Text = openFileDialog.FileName;
                        log.Info($"AWSアカウント一覧ファイルを選択しました：{openFileDialog.FileName}");
                    }
                }
                log.Info("AWSアカウント一覧ファイル選択処理が完了しました。");
            }
            catch (Exception ex)
            {
                log.Error("AWSアカウント一覧ファイル選択でエラーが発生しました。", ex);
                MessageBox.Show($"エラーが発生しました：{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcludeAccount_Click(object sender, EventArgs e)
        {
            log.Info("対象外アカウント設定ファイル選択処理を開始します。");
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv|All Files|*.*";
                    openFileDialog.FilterIndex = 1;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        txtExcludeAccount.Text = openFileDialog.FileName;
                        log.Info($"対象外アカウント設定ファイルを選択しました：{openFileDialog.FileName}");
                    }
                }
                log.Info("対象外アカウント設定ファイル選択処理が完了しました。");
            }
            catch (Exception ex)
            {
                log.Error("対象外アカウント設定ファイル選択でエラーが発生しました。", ex);
                MessageBox.Show($"エラーが発生しました：{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFileSorting_Click(object sender, EventArgs e)
        {
            log.Info("ファイル振分設定ファイル選択処理を開始します。");
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv|All Files|*.*";
                    openFileDialog.FilterIndex = 1;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        txtFileSorting.Text = openFileDialog.FileName;
                        log.Info($"ファイル振分設定ファイルを選択しました：{openFileDialog.FileName}");
                    }
                }
                log.Info("ファイル振分設定ファイル選択処理が完了しました。");
            }
            catch (Exception ex)
            {
                log.Error("ファイル振分設定ファイル選択でエラーが発生しました。", ex);
                MessageBox.Show($"エラーが発生しました：{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDownloadHistory_Click(object sender, EventArgs e)
        {
            log.Info("ダウンロード履歴ファイル選択処理を開始します。");
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv|All Files|*.*";
                    openFileDialog.FilterIndex = 1;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        txtDownloadHistory.Text = openFileDialog.FileName;
                        log.Info($"ダウンロード履歴ファイルを選択しました：{openFileDialog.FileName}");
                    }
                }
                log.Info("ダウンロード履歴ファイル選択処理が完了しました。");
            }
            catch (Exception ex)
            {
                log.Error("ダウンロード履歴ファイル選択でエラーが発生しました。", ex);
                MessageBox.Show($"エラーが発生しました：{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 添加文件夹选择事件处理程序
        private void btnStoragePath_Click(object sender, EventArgs e)
        {
            log.Info("請求書格納パス選択処理を開始します。");
            try
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "請求書格納パスを選択してください。";
                    folderDialog.ShowNewFolderButton = true;

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        txtStoragePath.Text = folderDialog.SelectedPath;
                        log.Info($"請求書格納パスを選択しました：{folderDialog.SelectedPath}");
                    }
                }
                log.Info("請求書格納パス選択処理が完了しました。");
            }
            catch (Exception ex)
            {
                log.Error("請求書格納パス選択でエラーが発生しました。", ex);
                MessageBox.Show($"エラーが発生しました：{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDateRange_Click(object sender, EventArgs e)
        {
            log.Info("Download処理を開始します。");
            try
            {
                // 检查所有必填项
                if (string.IsNullOrEmpty(txtAWSAccount.Text))
                {
                    log.Warn("AWSアカウント一覧が選択されていません。");
                    MessageBox.Show("AWSアカウント一覧を選択してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(txtExcludeAccount.Text))
                {
                    MessageBox.Show("対象外アカウント設定を選択してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(txtFileSorting.Text))
                {
                    MessageBox.Show("ファイル振分設定を選択してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(txtDownloadHistory.Text))
                {
                    MessageBox.Show("ダウンロード履歴を選択してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(txtStoragePath.Text))
                {
                    MessageBox.Show("請求書格納パスを選択してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查日期范围
                if (dtpEnd.Value < dtpStart.Value)
                {
                    MessageBox.Show("終了日は開始日より後の日付を選択してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 读取对象外账户设置文件
                log.Info($"対象外アカウント設定ファイルの読み込みを開始します：{txtExcludeAccount.Text}");
                DataTable dtExclude = ExcelUtility.ExcelToDataTable(
                    filePath: txtExcludeAccount.Text,
                    isFirstRowAsHeader: true,
                    sheetName: "Payer",
                    startRowIndex: 2
                );
                log.Info($"対象外アカウント設定ファイルの読み込みが完了しました。行数：{dtExclude.Rows.Count}");

                // 生成対象外アカウント設定的输出文件路径
                string excludeTxtFilePath = Path.ChangeExtension(txtExcludeAccount.Text, ".txt");
                
                // 将対象外アカウント設定写入文件
                using (StreamWriter writer = new StreamWriter(excludeTxtFilePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("=== 対象外アカウント設定 ===\n");
                    WriteDataTableToFile(writer, dtExclude);
                }

                // 读取AWS账户列表文件
                log.Info($"AWSアカウント一覧ファイルの読み込みを開始します：{txtAWSAccount.Text}");
                DataSet dsAWS = ExcelUtility.ExcelToDataSet(
                    filePath: txtAWSAccount.Text,
                    sheetName: "Payer",
                    headerColumnName: "NO"
                );
                log.Info($"AWSアカウント一覧ファイルの読み込みが完了しました。テーブル数：{dsAWS.Tables.Count}");

                // 读取文件分类设置
                log.Info($"ファイル振分設定ファイルの読み込みを開始します：{txtFileSorting.Text}");
                
                // 1. パス定義
                DataTable dtPath = ExcelUtility.ExcelToDataTable(
                    filePath: txtFileSorting.Text,
                    isFirstRowAsHeader: true,
                    sheetName: "パス定義",
                    startRowIndex: 1
                );
                log.Info($"パス定義シートの読み込みが完了しました。行数：{dtPath.Rows.Count}");

                // 2. リセラー対象
                DataTable dtReseller = ExcelUtility.ExcelToDataTable(
                    filePath: txtFileSorting.Text,
                    isFirstRowAsHeader: true,
                    sheetName: "リセラー対象",
                    startRowIndex: 1
                );
                log.Info($"リセラー対象シートの読み込みが完了しました。行数：{dtReseller.Rows.Count}");

                // 3. 会員名義（カードNO）変換
                DataTable dtMember = ExcelUtility.ExcelToDataTable(
                    filePath: txtFileSorting.Text,
                    isFirstRowAsHeader: true,
                    sheetName: "会員名義（カードNO）変換",
                    startRowIndex: 1
                );
                log.Info($"会員名義変換シートの読み込みが完了しました。行数：{dtMember.Rows.Count}");

                // 4. テナント名変換
                DataTable dtTenant = ExcelUtility.ExcelToDataTable(
                    filePath: txtFileSorting.Text,
                    isFirstRowAsHeader: true,
                    sheetName: "テナント名変換",
                    startRowIndex: 1
                );
                log.Info($"テナント名変換シートの読み込みが完了しました。行数：{dtTenant.Rows.Count}");

                // 将文件分类设置数据写入txt文件
                string sortingTxtFilePath = Path.ChangeExtension(txtFileSorting.Text, ".txt");
                using (StreamWriter writer = new StreamWriter(sortingTxtFilePath, false, Encoding.UTF8))
                {
                    // パス定義
                    writer.WriteLine("=== パス定義 ===");
                    WriteDataTableToFile(writer, dtPath);
                    writer.WriteLine();

                    // リセラー対象
                    writer.WriteLine("=== リセラー対象 ===");
                    WriteDataTableToFile(writer, dtReseller);
                    writer.WriteLine();

                    // 会員名義変換
                    writer.WriteLine("=== 会員名義（カードNO）変換 ===");
                    WriteDataTableToFile(writer, dtMember);
                    writer.WriteLine();

                    // テナント名変換
                    writer.WriteLine("=== テナント名変換 ===");
                    WriteDataTableToFile(writer, dtTenant);
                    writer.WriteLine();
                }

                // 克隆第一个DataTable的结构
                DataTable mergedTable = null;
                if (dsAWS.Tables.Count > 0)
                {
                    mergedTable = dsAWS.Tables[0].Clone();
                    
                    // 合并所有DataTable的数据
                    foreach (DataTable table in dsAWS.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            mergedTable.Rows.Add(row.ItemArray);
                        }
                    }

                    // 找到需要比较的列
                    var commonColumns = new List<string>();
                    foreach (DataColumn column in mergedTable.Columns)
                    {
                        if (dtExclude.Columns.Contains(column.ColumnName))
                        {
                            commonColumns.Add(column.ColumnName);
                        }
                    }

                    if (commonColumns.Count > 0)
                    {
                        log.Info($"开始根据対象外アカウント设定进行筛选，比较的列：{string.Join(", ", commonColumns)}");
                        
                        // 创建要删除的行的列表
                        var rowsToDelete = new List<DataRow>();

                        // 遍历合并表的每一行
                        foreach (DataRow mergedRow in mergedTable.Rows)
                        {
                            // 检查是否在排除列表中
                            bool shouldExclude = dtExclude.AsEnumerable().Any(excludeRow =>
                            {
                                // 只要有一个共同列的值包含在对方中就返回true
                                return commonColumns.Any(columnName =>
                                {
                                    string mergedValue = mergedRow[columnName].ToString();
                                    string excludeValue = excludeRow[columnName].ToString();
                                    return (!string.IsNullOrEmpty(mergedValue) && 
                                           !string.IsNullOrEmpty(excludeValue) && 
                                           (mergedValue.Contains(excludeValue) || excludeValue.Contains(mergedValue))) 
                                           || string.IsNullOrEmpty(mergedValue);
                                });
                            });

                            if (shouldExclude)
                            {
                                rowsToDelete.Add(mergedRow);
                            }
                        }

                        // 删除匹配的行
                        foreach (var row in rowsToDelete)
                        {
                            mergedTable.Rows.Remove(row);
                        }

                        log.Info($"筛选完成，删除了 {rowsToDelete.Count} 行数据");
                    }
                    else
                    {
                        log.Warn("未找到可比较的共同列");
                    }

                    // 生成筛选后的数据文件
                    string filteredTxtFilePath = Path.Combine(
                        Path.GetDirectoryName(txtAWSAccount.Text),
                        Path.GetFileNameWithoutExtension(txtAWSAccount.Text) + "_filtered.txt"
                    );

                    // 将筛选后的数据写入文件
                    using (StreamWriter writer = new StreamWriter(filteredTxtFilePath, false, Encoding.UTF8))
                    {
                        writer.WriteLine("=== AWS Account List Filtered Data ===\n");
                        writer.WriteLine($"筛选条件：排除対象外アカウント設定中的账户\n");
                        writer.WriteLine($"比较的列：{string.Join(", ", commonColumns)}\n");
                        WriteDataTableToFile(writer, mergedTable);
                    }

                    var strPathLink = @"\";
                    Hashtable hashTableFilePath = new Hashtable();
                    for (int i = 0; i < mergedTable.Rows.Count; i++)
                    {
                        List<string> pathList = new List<string>();
                        var thirdBeforeChangeFolder = mergedTable.Rows[i]["テナント名"].ToString();
                        var thirdAfterChangeFolder = thirdBeforeChangeFolder;
                        var row = dtTenant.Select("変更前テナント名 ='" + thirdBeforeChangeFolder + "'");
                        if (row.Length > 0)
                        {
                            thirdAfterChangeFolder = dtTenant.Select("変更前テナント名 ='" + thirdBeforeChangeFolder + "'")[0][1].ToString();
                        }
                        var secondBeforeChangeFolder = string.Empty;
                        var secondAfterChangeFolder = string.Empty;
                        row = dtMember.Select("変更前カードNO = '" + mergedTable.Rows[i]["カードNo."].ToString() + "'");
                        if (row.Length > 0)
                        {
                            secondBeforeChangeFolder = row[0][0].ToString();
                            secondAfterChangeFolder = row[0][1].ToString();
                        }
                        var firstBeforeChangeFolder = string.Empty;
                        var firstAfterChangeFolder = string.Empty;
                        row = dtReseller.Select("テナント名 = '" + thirdBeforeChangeFolder + "'");
                        if (row.Length > 0)
                        {
                            firstBeforeChangeFolder = dtPath.Select("シート名 = 'リセラー対象'")[0][1].ToString();
                        }
                        else
                        {
                            firstBeforeChangeFolder = dtPath.Select("シート名 = ''")[0][1].ToString();

                        }
                        firstAfterChangeFolder = firstBeforeChangeFolder;
                        if (!string.IsNullOrEmpty(secondBeforeChangeFolder))
                        {
                            pathList.Add(txtStoragePath.Text + strPathLink + "Before" + strPathLink + replaceSpecialChar(firstBeforeChangeFolder) + strPathLink
                                + replaceSpecialChar(secondBeforeChangeFolder) + strPathLink + replaceSpecialChar(thirdBeforeChangeFolder));
                        }
                        else
                        {
                            pathList.Add(txtStoragePath.Text + strPathLink + "Before" + strPathLink + replaceSpecialChar(firstBeforeChangeFolder) + strPathLink
                                + replaceSpecialChar(thirdBeforeChangeFolder));
                        }
                        if (!string.IsNullOrEmpty(secondAfterChangeFolder))
                        {
                            pathList.Add(txtStoragePath.Text + strPathLink + "After" + strPathLink + replaceSpecialChar(firstAfterChangeFolder) + strPathLink
                                + replaceSpecialChar(secondAfterChangeFolder) + strPathLink + replaceSpecialChar(thirdAfterChangeFolder));
                        }
                        else
                        {
                            pathList.Add(txtStoragePath.Text + strPathLink + "After" + strPathLink + replaceSpecialChar(firstAfterChangeFolder) + strPathLink
                                + replaceSpecialChar(thirdAfterChangeFolder));
                        }
                        hashTableFilePath.Add(mergedTable.Rows[i]["IAMエイリアス"].ToString(), pathList);
                        foreach (string path in pathList)
                        {
                            if (Directory.Exists(path) == false)
                            {
                                Directory.CreateDirectory(path);
                            }
                        }
                    }

                    // 在所有数据处理完成后，启动AWS控制台
                    log.Info("启动AWS控制台浏览器...");
                    IWebDriver driver = null;  // 声明在外部以便后续使用
                    try
                    {
                        driver = SeleniumUtility.LaunchAWSConsole(txtStoragePath.Text);
                        
                        // 显示等待登录的对话框
                        var result = MessageBox.Show(
                            "AWSコンソールにログインしてください。\n" +
                            "ログイン完了後、OKボタンをクリックしてください。",
                            "ログイン待ち",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Information
                        );

                        // 根据用户选择决定是否关闭浏览器
                        if (result == DialogResult.Cancel)
                        {
                            log.Info("用户取消了操作，正在关闭浏览器...");
                            SeleniumUtility.CloseBrowser();
                            return; // 退出当前方法
                        }
                        
                        log.Info("用户确认已完成登录，继续执行后续操作");
                    }
                    catch (Exception ex)
                    {
                        log.Error("启动AWS控制台失败", ex);
                        MessageBox.Show(
                            $"AWS控制台启动失败：{ex.Message}\n" +
                            "请确认网络连接和浏览器设置。", 
                            "エラー",
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error
                        );
                    }

                    // 如果浏览器启动成功，继续执行导航操作
                    if (driver != null)
                    {
                        try
                        {
                            // 导航到账单下载页面
                            SeleniumUtility.downloadInvoice(driver, txtDownloadHistory.Text, dtpStart.Text, dtpEnd.Text, mergedTable, hashTableFilePath);
                            
                            // 正常完成后关闭浏览器
                            log.Info("下载操作完成，准备关闭浏览器...");
                            SeleniumUtility.CloseBrowser();
                        }
                        catch (Exception ex)
                        {
                            log.Error("导航到账单下载页面失败", ex);
                            // 确保关闭浏览器
                            SeleniumUtility.CloseBrowser();
                            MessageBox.Show(
                                $"导航失败：{ex.Message}\n" +
                                "请确认页面是否正常加载。", 
                                "エラー",
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error
                            );
                        }
                    }

                    // 更新成功消息
                    MessageBox.Show(
                        $"データ読み込み完了。\n" +
                        $"対象外アカウント設定：{dtExclude.Rows.Count}行\n" +
                        $"AWSアカウント一覧：{dsAWS.Tables.Count}テーブル\n" +
                        $"AWSアカウント一覧（統合）：{mergedTable.Rows.Count}行\n" +
                        $"ファイル振分設定：\n" +
                        $"  - パス定義：{dtPath.Rows.Count}行\n" +
                        $"  - リセラー対象：{dtReseller.Rows.Count}行\n" +
                        $"  - 会員名義変換：{dtMember.Rows.Count}行\n" +
                        $"  - テナント名変換：{dtTenant.Rows.Count}行\n" +
                        $"保存先：\n" +
                        $"1. {Path.GetFileName(excludeTxtFilePath)}\n" +
                        $"2. {Path.GetFileName(sortingTxtFilePath)}\n" +
                        $"3. {Path.GetFileName(filteredTxtFilePath)}", 
                        "情報", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    log.Warn("AWSアカウント一覧ファイルにデータが見つかりませんでした。");
                }

                log.Info("Download処理が正常に完了しました。");
            }
            catch (Exception ex)
            {
                log.Error("Download処理でエラーが発生しました。", ex);
                MessageBox.Show($"エラーが発生しました：{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// fmtPath
        /// </summary>
        /// <param name="strPathIn">path</param>
        /// <param name="blnAdd">flag of "\" add </param>
        private static string replaceSpecialChar(string strPathIn)
        {
            string strOut = string.Empty;
            char cRe = ' ';
            strOut = strPathIn;
            strOut = strOut.Replace('\\', cRe);
            strOut = strOut.Replace('/', cRe);
            strOut = strOut.Replace(':', cRe);
            strOut = strOut.Replace('*', cRe);
            strOut = strOut.Replace('?', cRe);
            strOut = strOut.Replace('\"', cRe);
            strOut = strOut.Replace('<', cRe);
            strOut = strOut.Replace('>', cRe);
            strOut = strOut.Replace('|', cRe);
            return strOut;
        }

        /// <summary>
        /// 将DataTable写入到文件
        /// </summary>
        private void WriteDataTableToFile(StreamWriter writer, DataTable dt)
        {
            // 写入列名
            foreach (DataColumn column in dt.Columns)
            {
                writer.Write($"{column.ColumnName,-20}");
            }
            writer.WriteLine();
            
            // 写入分隔线
            writer.WriteLine(new string('-', dt.Columns.Count * 20));
            
            // 写入数据行
            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    writer.Write($"{item,-20}");
                }
                writer.WriteLine();
            }
            
            writer.WriteLine($"\n総行数: {dt.Rows.Count}");
        }

        // 在Form关闭时添加清理代码
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            
            // 关闭浏览器
            SeleniumUtility.CloseBrowser();
        }
    }
}
