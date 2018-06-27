namespace imageRecognition
{
    partial class imgRecForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.rasp = new System.Windows.Forms.Button();
            this.camListBox = new System.Windows.Forms.ComboBox();
            this.resetBtn = new System.Windows.Forms.Button();
            this.etalonsDgv = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.learnBtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.inputImageBox = new System.Windows.Forms.PictureBox();
            this.rbContur = new System.Windows.Forms.RadioButton();
            this.rbHull = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbFisher = new System.Windows.Forms.RadioButton();
            this.rbLine = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.etalonsDgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputImageBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rasp
            // 
            this.rasp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rasp.Location = new System.Drawing.Point(466, 29);
            this.rasp.Name = "rasp";
            this.rasp.Size = new System.Drawing.Size(134, 35);
            this.rasp.TabIndex = 3;
            this.rasp.Text = "Распознать";
            this.rasp.UseVisualStyleBackColor = true;
            this.rasp.Click += new System.EventHandler(this.rasp_Click);
            // 
            // camListBox
            // 
            this.camListBox.FormattingEnabled = true;
            this.camListBox.Location = new System.Drawing.Point(12, 41);
            this.camListBox.Name = "camListBox";
            this.camListBox.Size = new System.Drawing.Size(170, 21);
            this.camListBox.TabIndex = 4;
            this.camListBox.SelectedIndexChanged += new System.EventHandler(this.camListBox_SelectedIndexChanged);
            this.camListBox.Click += new System.EventHandler(this.camListBox_Click);
            // 
            // resetBtn
            // 
            this.resetBtn.Enabled = false;
            this.resetBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.resetBtn.Location = new System.Drawing.Point(188, 29);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(134, 35);
            this.resetBtn.TabIndex = 5;
            this.resetBtn.Text = "Сброс";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // etalonsDgv
            // 
            this.etalonsDgv.AllowUserToAddRows = false;
            this.etalonsDgv.AllowUserToDeleteRows = false;
            this.etalonsDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.etalonsDgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.etalonsDgv.Location = new System.Drawing.Point(659, 93);
            this.etalonsDgv.Name = "etalonsDgv";
            this.etalonsDgv.ReadOnly = true;
            this.etalonsDgv.RowTemplate.Height = 150;
            this.etalonsDgv.Size = new System.Drawing.Size(424, 463);
            this.etalonsDgv.TabIndex = 6;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "Класс";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Эталон";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 256;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(657, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Загруженные эталоны:";
            // 
            // learnBtn
            // 
            this.learnBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.learnBtn.Location = new System.Drawing.Point(326, 29);
            this.learnBtn.Name = "learnBtn";
            this.learnBtn.Size = new System.Drawing.Size(134, 35);
            this.learnBtn.TabIndex = 9;
            this.learnBtn.Text = "Обучить";
            this.learnBtn.UseVisualStyleBackColor = true;
            this.learnBtn.Click += new System.EventHandler(this.learnBtn_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(-2, -2);
            this.progressBar1.Maximum = 180;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1098, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 10;
            // 
            // inputImageBox
            // 
            this.inputImageBox.Location = new System.Drawing.Point(13, 76);
            this.inputImageBox.Name = "inputImageBox";
            this.inputImageBox.Size = new System.Drawing.Size(640, 480);
            this.inputImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.inputImageBox.TabIndex = 0;
            this.inputImageBox.TabStop = false;
            // 
            // rbContur
            // 
            this.rbContur.AutoSize = true;
            this.rbContur.Location = new System.Drawing.Point(13, 3);
            this.rbContur.Name = "rbContur";
            this.rbContur.Size = new System.Drawing.Size(81, 17);
            this.rbContur.TabIndex = 11;
            this.rbContur.Text = "По контуру";
            this.rbContur.UseVisualStyleBackColor = true;
            this.rbContur.CheckedChanged += new System.EventHandler(this.rbContur_CheckedChanged);
            // 
            // rbHull
            // 
            this.rbHull.AutoSize = true;
            this.rbHull.Checked = true;
            this.rbHull.Location = new System.Drawing.Point(13, 23);
            this.rbHull.Name = "rbHull";
            this.rbHull.Size = new System.Drawing.Size(141, 17);
            this.rbHull.TabIndex = 12;
            this.rbHull.TabStop = true;
            this.rbHull.Text = "По выпуклой оболочке";
            this.rbHull.UseVisualStyleBackColor = true;
            this.rbHull.CheckedChanged += new System.EventHandler(this.rbHull_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.rbContur);
            this.panel1.Controls.Add(this.rbHull);
            this.panel1.Location = new System.Drawing.Point(910, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(167, 47);
            this.panel1.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.rbFisher);
            this.panel2.Controls.Add(this.rbLine);
            this.panel2.Location = new System.Drawing.Point(607, 23);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(297, 47);
            this.panel2.TabIndex = 14;
            // 
            // rbFisher
            // 
            this.rbFisher.AutoSize = true;
            this.rbFisher.Location = new System.Drawing.Point(13, 24);
            this.rbFisher.Name = "rbFisher";
            this.rbFisher.Size = new System.Drawing.Size(261, 17);
            this.rbFisher.TabIndex = 1;
            this.rbFisher.Text = "Методом дискриминантного анализа Фишера";
            this.rbFisher.UseVisualStyleBackColor = true;
            // 
            // rbLine
            // 
            this.rbLine.AutoSize = true;
            this.rbLine.Checked = true;
            this.rbLine.Location = new System.Drawing.Point(13, 3);
            this.rbLine.Name = "rbLine";
            this.rbLine.Size = new System.Drawing.Size(271, 17);
            this.rbLine.TabIndex = 0;
            this.rbLine.TabStop = true;
            this.rbLine.Text = "Методом линейного дискриминантного анализа";
            this.rbLine.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Текущая Web-камера:";
            // 
            // imgRecForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 562);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.learnBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.etalonsDgv);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.camListBox);
            this.Controls.Add(this.rasp);
            this.Controls.Add(this.inputImageBox);
            this.MaximizeBox = false;
            this.Name = "imgRecForm";
            this.Text = "Распознавание образов";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.imgRecForm_FormClosing);
            this.Load += new System.EventHandler(this.imgRecForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.etalonsDgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputImageBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox inputImageBox;
        private System.Windows.Forms.Button rasp;
        private System.Windows.Forms.ComboBox camListBox;
        private System.Windows.Forms.Button resetBtn;
        public System.Windows.Forms.DataGridView etalonsDgv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button learnBtn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RadioButton rbContur;
        private System.Windows.Forms.RadioButton rbHull;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbFisher;
        private System.Windows.Forms.RadioButton rbLine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewImageColumn Column2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

