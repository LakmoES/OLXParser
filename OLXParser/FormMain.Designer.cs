namespace OLXParser
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDownPages = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonExportDB = new System.Windows.Forms.Button();
            this.buttonClearDB = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.progressBarExport = new System.Windows.Forms.ProgressBar();
            this.labelAdvertCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarMain = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPages)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxURL
            // 
            this.textBoxURL.Location = new System.Drawing.Point(6, 19);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(264, 20);
            this.textBoxURL.TabIndex = 0;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(36, 164);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Старт";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxURL);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 51);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ссылка на раздел";
            // 
            // numericUpDownPages
            // 
            this.numericUpDownPages.Location = new System.Drawing.Point(9, 19);
            this.numericUpDownPages.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPages.Name = "numericUpDownPages";
            this.numericUpDownPages.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownPages.TabIndex = 3;
            this.numericUpDownPages.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDownPages);
            this.groupBox2.Location = new System.Drawing.Point(12, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(126, 47);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Количество страниц";
            // 
            // buttonExportDB
            // 
            this.buttonExportDB.Location = new System.Drawing.Point(6, 82);
            this.buttonExportDB.Name = "buttonExportDB";
            this.buttonExportDB.Size = new System.Drawing.Size(75, 23);
            this.buttonExportDB.TabIndex = 5;
            this.buttonExportDB.Text = "Выгрузить";
            this.buttonExportDB.UseVisualStyleBackColor = true;
            this.buttonExportDB.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttonClearDB
            // 
            this.buttonClearDB.Location = new System.Drawing.Point(6, 19);
            this.buttonClearDB.Name = "buttonClearDB";
            this.buttonClearDB.Size = new System.Drawing.Size(75, 23);
            this.buttonClearDB.TabIndex = 6;
            this.buttonClearDB.Text = "Очистить";
            this.buttonClearDB.UseVisualStyleBackColor = true;
            this.buttonClearDB.Click += new System.EventHandler(this.buttonClearDB_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.progressBarExport);
            this.groupBox3.Controls.Add(this.labelAdvertCount);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.buttonExportDB);
            this.groupBox3.Controls.Add(this.buttonClearDB);
            this.groupBox3.Location = new System.Drawing.Point(175, 135);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(107, 145);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "База данных";
            // 
            // progressBarExport
            // 
            this.progressBarExport.Location = new System.Drawing.Point(6, 59);
            this.progressBarExport.Name = "progressBarExport";
            this.progressBarExport.Size = new System.Drawing.Size(75, 23);
            this.progressBarExport.TabIndex = 9;
            // 
            // labelAdvertCount
            // 
            this.labelAdvertCount.AutoSize = true;
            this.labelAdvertCount.Location = new System.Drawing.Point(65, 119);
            this.labelAdvertCount.Name = "labelAdvertCount";
            this.labelAdvertCount.Size = new System.Drawing.Size(10, 13);
            this.labelAdvertCount.TabIndex = 9;
            this.labelAdvertCount.Text = "-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Записей:";
            // 
            // progressBarMain
            // 
            this.progressBarMain.Location = new System.Drawing.Point(12, 135);
            this.progressBarMain.Name = "progressBarMain";
            this.progressBarMain.Size = new System.Drawing.Size(126, 23);
            this.progressBarMain.TabIndex = 8;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(287, 289);
            this.Controls.Add(this.progressBarMain);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "OLX Parser";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPages)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDownPages;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonExportDB;
        private System.Windows.Forms.Button buttonClearDB;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelAdvertCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBarMain;
        private System.Windows.Forms.ProgressBar progressBarExport;
    }
}

