using System;
using System.Drawing;
using System.Windows.Forms;

namespace Test
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblAWSAccount = new System.Windows.Forms.Label();
            this.txtAWSAccount = new System.Windows.Forms.TextBox();
            this.btnAWSAccount = new System.Windows.Forms.Button();
            this.lblExcludeAccount = new System.Windows.Forms.Label();
            this.txtExcludeAccount = new System.Windows.Forms.TextBox();
            this.btnExcludeAccount = new System.Windows.Forms.Button();
            this.lblFileSorting = new System.Windows.Forms.Label();
            this.txtFileSorting = new System.Windows.Forms.TextBox();
            this.btnFileSorting = new System.Windows.Forms.Button();
            this.lblDownloadHistory = new System.Windows.Forms.Label();
            this.txtDownloadHistory = new System.Windows.Forms.TextBox();
            this.btnDownloadHistory = new System.Windows.Forms.Button();
            this.lblStoragePath = new System.Windows.Forms.Label();
            this.txtStoragePath = new System.Windows.Forms.TextBox();
            this.btnStoragePath = new System.Windows.Forms.Button();
            this.lblDateRange = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.btnDateRange = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblAWSAccount);
            this.groupBox1.Controls.Add(this.txtAWSAccount);
            this.groupBox1.Controls.Add(this.btnAWSAccount);
            this.groupBox1.Controls.Add(this.lblExcludeAccount);
            this.groupBox1.Controls.Add(this.txtExcludeAccount);
            this.groupBox1.Controls.Add(this.btnExcludeAccount);
            this.groupBox1.Controls.Add(this.lblFileSorting);
            this.groupBox1.Controls.Add(this.txtFileSorting);
            this.groupBox1.Controls.Add(this.btnFileSorting);
            this.groupBox1.Controls.Add(this.lblDownloadHistory);
            this.groupBox1.Controls.Add(this.txtDownloadHistory);
            this.groupBox1.Controls.Add(this.btnDownloadHistory);
            this.groupBox1.Controls.Add(this.lblStoragePath);
            this.groupBox1.Controls.Add(this.txtStoragePath);
            this.groupBox1.Controls.Add(this.btnStoragePath);
            this.groupBox1.Controls.Add(this.lblDateRange);
            this.groupBox1.Controls.Add(this.dtpStart);
            this.groupBox1.Controls.Add(this.dtpEnd);
            this.groupBox1.Controls.Add(this.btnDateRange);
            this.groupBox1.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 270);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "関連ファイル設定";
            // 
            // lblAWSAccount
            // 
            this.lblAWSAccount.Location = new System.Drawing.Point(20, 30);
            this.lblAWSAccount.Name = "lblAWSAccount";
            this.lblAWSAccount.Size = new System.Drawing.Size(150, 23);
            this.lblAWSAccount.TabIndex = 0;
            this.lblAWSAccount.Text = "AWSアカウント一覧";
            // 
            // txtAWSAccount
            // 
            this.txtAWSAccount.Location = new System.Drawing.Point(180, 30);
            this.txtAWSAccount.Name = "txtAWSAccount";
            this.txtAWSAccount.ReadOnly = true;
            this.txtAWSAccount.Size = new System.Drawing.Size(450, 29);
            this.txtAWSAccount.TabIndex = 1;
            // 
            // btnAWSAccount
            // 
            this.btnAWSAccount.Location = new System.Drawing.Point(640, 30);
            this.btnAWSAccount.Name = "btnAWSAccount";
            this.btnAWSAccount.Size = new System.Drawing.Size(100, 30);
            this.btnAWSAccount.TabIndex = 2;
            this.btnAWSAccount.Text = "ファイル選択";
            this.btnAWSAccount.Click += new System.EventHandler(this.btnAWSAccount_Click);
            // 
            // lblExcludeAccount
            // 
            this.lblExcludeAccount.Location = new System.Drawing.Point(20, 65);
            this.lblExcludeAccount.Name = "lblExcludeAccount";
            this.lblExcludeAccount.Size = new System.Drawing.Size(150, 23);
            this.lblExcludeAccount.TabIndex = 3;
            this.lblExcludeAccount.Text = "対象外アカウント設定";
            // 
            // txtExcludeAccount
            // 
            this.txtExcludeAccount.Location = new System.Drawing.Point(180, 65);
            this.txtExcludeAccount.Name = "txtExcludeAccount";
            this.txtExcludeAccount.ReadOnly = true;
            this.txtExcludeAccount.Size = new System.Drawing.Size(450, 29);
            this.txtExcludeAccount.TabIndex = 4;
            // 
            // btnExcludeAccount
            // 
            this.btnExcludeAccount.Location = new System.Drawing.Point(640, 65);
            this.btnExcludeAccount.Name = "btnExcludeAccount";
            this.btnExcludeAccount.Size = new System.Drawing.Size(100, 30);
            this.btnExcludeAccount.TabIndex = 5;
            this.btnExcludeAccount.Text = "ファイル選択";
            this.btnExcludeAccount.Click += new System.EventHandler(this.btnExcludeAccount_Click);
            // 
            // lblFileSorting
            // 
            this.lblFileSorting.Location = new System.Drawing.Point(20, 100);
            this.lblFileSorting.Name = "lblFileSorting";
            this.lblFileSorting.Size = new System.Drawing.Size(150, 23);
            this.lblFileSorting.TabIndex = 6;
            this.lblFileSorting.Text = "ファイル振分設定";
            // 
            // txtFileSorting
            // 
            this.txtFileSorting.Location = new System.Drawing.Point(180, 100);
            this.txtFileSorting.Name = "txtFileSorting";
            this.txtFileSorting.ReadOnly = true;
            this.txtFileSorting.Size = new System.Drawing.Size(450, 29);
            this.txtFileSorting.TabIndex = 7;
            // 
            // btnFileSorting
            // 
            this.btnFileSorting.Location = new System.Drawing.Point(640, 100);
            this.btnFileSorting.Name = "btnFileSorting";
            this.btnFileSorting.Size = new System.Drawing.Size(100, 30);
            this.btnFileSorting.TabIndex = 8;
            this.btnFileSorting.Text = "ファイル選択";
            this.btnFileSorting.Click += new System.EventHandler(this.btnFileSorting_Click);
            // 
            // lblDownloadHistory
            // 
            this.lblDownloadHistory.Location = new System.Drawing.Point(20, 135);
            this.lblDownloadHistory.Name = "lblDownloadHistory";
            this.lblDownloadHistory.Size = new System.Drawing.Size(150, 23);
            this.lblDownloadHistory.TabIndex = 9;
            this.lblDownloadHistory.Text = "ダウンロード履歴";
            // 
            // txtDownloadHistory
            // 
            this.txtDownloadHistory.Location = new System.Drawing.Point(180, 135);
            this.txtDownloadHistory.Name = "txtDownloadHistory";
            this.txtDownloadHistory.ReadOnly = true;
            this.txtDownloadHistory.Size = new System.Drawing.Size(450, 29);
            this.txtDownloadHistory.TabIndex = 10;
            // 
            // btnDownloadHistory
            // 
            this.btnDownloadHistory.Location = new System.Drawing.Point(640, 135);
            this.btnDownloadHistory.Name = "btnDownloadHistory";
            this.btnDownloadHistory.Size = new System.Drawing.Size(100, 30);
            this.btnDownloadHistory.TabIndex = 11;
            this.btnDownloadHistory.Text = "ファイル選択";
            this.btnDownloadHistory.Click += new System.EventHandler(this.btnDownloadHistory_Click);
            // 
            // lblStoragePath
            // 
            this.lblStoragePath.Location = new System.Drawing.Point(20, 170);
            this.lblStoragePath.Name = "lblStoragePath";
            this.lblStoragePath.Size = new System.Drawing.Size(150, 23);
            this.lblStoragePath.TabIndex = 12;
            this.lblStoragePath.Text = "請求書格納パス";
            // 
            // txtStoragePath
            // 
            this.txtStoragePath.Location = new System.Drawing.Point(180, 170);
            this.txtStoragePath.Name = "txtStoragePath";
            this.txtStoragePath.ReadOnly = true;
            this.txtStoragePath.Size = new System.Drawing.Size(450, 29);
            this.txtStoragePath.TabIndex = 13;
            // 
            // btnStoragePath
            // 
            this.btnStoragePath.Location = new System.Drawing.Point(640, 170);
            this.btnStoragePath.Name = "btnStoragePath";
            this.btnStoragePath.Size = new System.Drawing.Size(100, 30);
            this.btnStoragePath.TabIndex = 14;
            this.btnStoragePath.Text = "ファイル選択";
            this.btnStoragePath.Click += new System.EventHandler(this.btnStoragePath_Click);
            // 
            // lblDateRange
            // 
            this.lblDateRange.Location = new System.Drawing.Point(20, 205);
            this.lblDateRange.Name = "lblDateRange";
            this.lblDateRange.Size = new System.Drawing.Size(150, 23);
            this.lblDateRange.TabIndex = 15;
            this.lblDateRange.Text = "対象日付";
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "yyyy/MM/dd";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(180, 205);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(220, 29);
            this.dtpStart.TabIndex = 16;
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "yyyy/MM/dd";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(410, 205);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(220, 29);
            this.dtpEnd.TabIndex = 17;
            // 
            // btnDateRange
            // 
            this.btnDateRange.Location = new System.Drawing.Point(640, 205);
            this.btnDateRange.Name = "btnDateRange";
            this.btnDateRange.Size = new System.Drawing.Size(100, 30);
            this.btnDateRange.TabIndex = 18;
            this.btnDateRange.Text = "Download";
            this.btnDateRange.Click += new System.EventHandler(this.btnDateRange_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(380, 300);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 304);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private TextBox txtAWSAccount;
        private TextBox txtExcludeAccount;
        private TextBox txtFileSorting;
        private TextBox txtDownloadHistory;
        private TextBox txtStoragePath;
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private Label lblAWSAccount;
        private Button btnAWSAccount;
        private Label lblExcludeAccount;
        private Button btnExcludeAccount;
        private Label lblFileSorting;
        private Button btnFileSorting;
        private Label lblDownloadHistory;
        private Button btnDownloadHistory;
        private Label lblStoragePath;
        private Button btnStoragePath;
        private Label lblDateRange;
        private Button btnDateRange;
        private Button btnClose;
    }
}

