using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using log4net;
using System;
using System.IO;
using System.Windows.Forms;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Extensions;
using SeleniumExtras.WaitHelpers;
using System.Threading;
using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace Test
{
    public class SeleniumUtility
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SeleniumUtility));
        private static IWebDriver driver;
        private static string downloadPath; // 添加全局变量

        /// <summary>
        /// 启动Edge浏览器并访问AWS控制台
        /// </summary>
        /// <returns>WebDriver实例</returns>
        public static IWebDriver LaunchAWSConsole(string myDownloadPath)
        {
            try
            {
                log.Info("开始启动Edge浏览器...");

                // 获取下载路径
                downloadPath = myDownloadPath;

                // 设置 EdgeDriver 路径
                var service = EdgeDriverService.CreateDefaultService(
                    Path.GetDirectoryName(Application.ExecutablePath), 
                    "msedgedriver.exe");
                    
                // 创建Edge浏览器选项
                var options = new EdgeOptions();
                options.AddArgument("--start-maximized");
                // options.AddArgument("--inprivate");
                
                // 禁用各种对话框和通知
                options.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);  // 禁用通知
                options.AddUserProfilePreference("profile.default_content_settings.popups", 0);  // 禁用弹出窗口
                options.AddUserProfilePreference("profile.content_settings.exceptions.automatic_downloads.*.setting", 1);  // 允许自动下载
                options.AddUserProfilePreference("download.prompt_for_download", false);  // 禁用下载提示
                options.AddUserProfilePreference("safebrowsing.enabled", true);  // 启用安全浏览
                options.AddUserProfilePreference("credentials_enable_service", false);  // 禁用保存密码提示
                options.AddUserProfilePreference("profile.password_manager_enabled", false);  // 禁用密码管理器

                // 设置下载路径
                options.AddUserProfilePreference("download.default_directory", downloadPath);
                
                // 创建EdgeDriver实例
                driver = new EdgeDriver(service, options);
                
                // 设置隐式等待时间
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                
                // 访问AWS控制台
                string awsConsoleUrl = "https://adtec-int-cb-1.signin.aws.amazon.com/console";
                log.Info($"正在访问AWS控制台：{awsConsoleUrl}");
                driver.Navigate().GoToUrl(awsConsoleUrl);
                
                log.Info("Edge浏览器启动成功");
                return driver;
            }
            catch (Exception ex)
            {
                log.Error("启动Edge浏览器时发生错误", ex);
                throw new Exception($"启动Edge浏览器失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 关闭浏览器
        /// </summary>
        public static void CloseBrowser()
        {
            try
            {
                if (driver != null)
                {
                    log.Info("正在关闭Edge浏览器...");
                    driver.Quit();
                    driver = null;
                    log.Info("Edge浏览器已关闭");
                }
            }
            catch (Exception ex)
            {
                log.Error("关闭Edge浏览器时发生错误", ex);
            }
        }

        /// <summary>
        /// 导航到账单下载页面
        /// </summary>
        /// <param name="driver">WebDriver实例</param>
        public static void downloadInvoice(IWebDriver driver, string myHistoryFilePath, string startDate, string endDate, DataTable mergedTable, Hashtable hashTableFilePath)
        {
            try
            {
                log.Info("开始导航到账单下载页面...");


                // 等待页面加载完成
                Thread.Sleep(3000);

                // 等待并点击 "Billing and Cost Management"
                log.Info("等待 'Billing and Cost Management' 链接可点击...");
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                By byID_user_id = By.Id("nav-usernameMenu");
                var user_id = wait.Until(ExpectedConditions.ElementExists(By.Id("nav-usernameMenu"))).Text;
                user_id = user_id.Split(new string[] { " @ " }, StringSplitOptions.None)[1].Trim();

                // 尝试多种定位方式
                IWebElement billingLink = null;

                try
                {
                    // 尝试方法2：通过链接文本
                    billingLink = wait.Until(ExpectedConditions.ElementExists(
                        By.LinkText("Billing and Cost Management")));
                }
                catch
                {
                    // 尝试方法3：通过部分文本
                    billingLink = wait.Until(ExpectedConditions.ElementExists(
                        By.PartialLinkText("Billing")));
                }
                
                if (billingLink != null)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", billingLink);
                    log.Info("已点击 'Billing and Cost Management' 链接");
                }
                else
                {
                    throw new Exception("无法找到 Billing and Cost Management 链接");
                }

                // 等待新页面加载
                Thread.Sleep(3000);

                // 等待并点击 "支払い"
                log.Info("等待 '支払い' 链接可点击...");
                driver.SwitchTo().DefaultContent();
                var paymentLink = wait.Until(ExpectedConditions.ElementExists(By.LinkText("支払い")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", paymentLink);
                log.Info("已点击 '支払い' 链接");

                // 等待新页面加载
                Thread.Sleep(3000);

                // 等待并点击 "取引" tab
                log.Info("等待 '取引' tab可点击...");
                driver.SwitchTo().DefaultContent();
                var transactionTab = wait.Until(ExpectedConditions.ElementExists(By.LinkText("取引")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", transactionTab);
                log.Info("已点击 '取引' tab");

                for (int i = 0; i < mergedTable.Rows.Count; i++)
                {
                    if (mergedTable.Rows[i]["IAMエイリアス"].ToString().Equals(user_id))
                    {
                        continue;
                    }

                    // 点击用户菜单
                    log.Info("等待用户菜单可点击...");
                    var userMenu = wait.Until(ExpectedConditions.ElementExists(
                        By.CssSelector("button[id='nav-usernameMenu']")));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", userMenu);

                    // 点击角色切换选项
                    log.Info("等待'ロールの切り替え'选项可点击...");
                    var switchRoleLink = wait.Until(ExpectedConditions.ElementExists(
                        By.LinkText("ロールの切り替え")));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", switchRoleLink);
                    // 等待新页面加载
                    Thread.Sleep(3000);

                    // 输入账户ID
                    log.Info("输入账户ID...");
                    var accountInput = wait.Until(ExpectedConditions.ElementExists(
                        By.Id("accountId")));
                    accountInput.SendKeys(mergedTable.Rows[i]["IAMエイリアス"].ToString());

                    // 输入IAM角色名
                    log.Info("输入IAM角色名...");
                    var roleInput = wait.Until(ExpectedConditions.ElementExists(
                        By.Id("roleName")));
                    roleInput.SendKeys("rpa-billing-developer");

                    // 点击切换角色按钮
                    log.Info("点击'ロールの切り替え'按钮...");
                    var switchButton = wait.Until(ExpectedConditions.ElementExists(
                        By.CssSelector("button[type='submit']")));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", switchButton);

                    // 等待页面加载
                    Thread.Sleep(3000);

                    // 点击日期范围控件
                    log.Info("等待日期范围控件可点击...");
                    var dateRangeButton = wait.Until(ExpectedConditions.ElementExists(
                        By.CssSelector("span[id^='date-range-picker-trigger']")));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dateRangeButton);
                    log.Info("已点击日期范围控件");

                    // 等待页面加载
                    Thread.Sleep(3000);

                    // 定位日期输入框
                    log.Info("定位日期输入框...");
                    var dateInputs = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(
                        By.CssSelector("input[placeholder='YYYY/MM/DD'][class*='awsui_input']")));

                    // 输入起始日期
                    log.Info("输入起始日期: 2024/02/04");
                    dateInputs[0].Clear();
                    dateInputs[0].SendKeys(startDate);

                    // 输入结束日期
                    log.Info("输入结束日期: 2025/03/04");
                    dateInputs[1].Clear();
                    dateInputs[1].SendKeys(endDate);

                    // 点击適用按钮
                    log.Info("点击適用按钮...");
                    var applyButton = wait.Until(ExpectedConditions.ElementExists(
                        By.CssSelector("button[type='button'][class*='awsui_button'][class*='awsui_apply-button']")));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", applyButton);
                    log.Info("已点击適用按钮");

                    // 等待页面加载
                    Thread.Sleep(3000);

                    while (true) // 循环处理所有页面
                    {
                        // 等待发票下载按钮出现
                        log.Info("定位发票下载按钮...");
                        var downloadButtons = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(
                            By.CssSelector("a[aria-label*='請求書'][aria-label*='をダウンロード']")));

                        // 如果当前页面没有找到下载按钮，退出循环
                        if (downloadButtons.Count == 0)
                        {
                            log.Info("没有找到更多的发票下载按钮");
                            break;
                        }

                        // 循环点击每个下载按钮
                        foreach (var button in downloadButtons)
                        {
                            try
                            {
                                var buttonLabel = button.GetAttribute("aria-label")
                                    .Replace("請求書", "")
                                    .Replace("をダウンロード", "")
                                    .Trim();
                                log.Info($"点击下载按钮: {buttonLabel}");
                                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", button);

                                // 等待下载完成
                                log.Info("等待文件下载完成...");
                                bool isDownloadComplete = WaitForDownloadComplete(downloadPath, buttonLabel, mergedTable, hashTableFilePath, i);
                                if (isDownloadComplete)
                                {
                                    log.Info("文件下载完成");
                                    ExcelUtility.WriteDownloadHistory(buttonLabel, "文件下载完成", myHistoryFilePath, mergedTable, i);
                                }
                                else
                                {
                                    log.Warn("文件下载超时");
                                    ExcelUtility.WriteDownloadHistory(buttonLabel, "文件下载超时", myHistoryFilePath, mergedTable, i);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error($"点击下载按钮时发生错误: {ex.Message}");
                                continue;
                            }
                        }

                        // 尝试查找下一页按钮
                        try
                        {
                            var nextButton = driver.FindElement(By.CssSelector("button[aria-label*='次のページ']"));
                            if (nextButton.Enabled)
                            {
                                log.Info("点击下一页按钮");
                                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", nextButton);
                                Thread.Sleep(3000); // 等待新页面加载
                            }
                            else
                            {
                                log.Info("已到达最后一页");
                                break;
                            }
                        }
                        catch
                        {
                            log.Info("没有找到下一页按钮或已到达最后一页");
                            break;
                        }
                    }
                }

                log.Info("成功导航到账单下载页面");
            }
            catch (Exception ex)
            {
                log.Error("导航到账单下载页面时发生错误", ex);
                // 添加关闭浏览器的调用
                CloseBrowser();
                throw new Exception($"导航失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 等待文件下载完成
        /// </summary>
        /// <param name="downloadPath">下载文件夹路径</param>
        /// <param name="buttonLabel">下载按钮的标签内容</param>
        /// <param name="timeout">超时时间（秒）</param>
        /// <returns>是否下载完成</returns>
        private static bool WaitForDownloadComplete(string downloadPath, string buttonLabel, DataTable mergedTable,Hashtable hashTableFilePath, int index, int timeout = 5)
        {
            var startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalSeconds < timeout)
            {
                // 检查下载文件夹中是否有正在下载的临时文件
                var tempFiles = Directory.GetFiles(downloadPath, "*.crdownload")
                    .Union(Directory.GetFiles(downloadPath, "*.tmp"))
                    .Union(Directory.GetFiles(downloadPath, "*.partial"));

                if (!tempFiles.Any())
                {
                    // 获取所有PDF文件
                    // var pdfFiles = Directory.GetFiles(downloadPath, "*.pdf")
                    //     .Where(f => File.GetLastWriteTime(f) > startTime)
                    //     .OrderByDescending(f => File.GetLastWriteTime(f));
                    var pdfFiles = Directory.GetFiles(downloadPath, "*.pdf");

                    foreach (var pdfFile in pdfFiles)
                    {
                        // 从按钮标签中提取关键信息（如发票编号等）
                        var fileName = Path.GetFileNameWithoutExtension(pdfFile);
                        if (fileName.Contains(buttonLabel))
                        {
                            var getFolderPath = (List<string>)hashTableFilePath[mergedTable.Rows[index]["IAMエイリアス"].ToString()];
                            //if (!string.IsNullOrEmpty(fileName))
                            //{
                            for (int pathIndex = 0; pathIndex < getFolderPath.Count; pathIndex++)
                            {
                                if (Directory.Exists(getFolderPath[pathIndex]))
                                {
                                    File.Copy(downloadPath + @"\\" + fileName + ".pdf", getFolderPath[pathIndex] + @"\\" + fileName + ".pdf", true);
                                    Thread.Sleep(2000);
                                }
                            }
                            //}
                            log.Info($"找到匹配的PDF文件: {pdfFile}");
                            return true;
                        }
                    }
                }

                Thread.Sleep(500); // 每500毫秒检查一次
            }

            return false;
        }


    }
} 