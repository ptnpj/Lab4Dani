namespace FormLb4
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
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            txtSearchId = new TextBox();
            txtFilterId = new TextBox();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(28, 32);
            button1.Name = "button1";
            button1.Size = new Size(117, 60);
            button1.TabIndex = 0;
            button1.Text = "Створити XML з таблиці";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnExportTable_Click;
            // 
            // button2
            // 
            button2.Location = new Point(162, 32);
            button2.Name = "button2";
            button2.Size = new Size(115, 60);
            button2.TabIndex = 1;
            button2.Text = "Створити XML із SELECT";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnExportSelect_Click;
            // 
            // button3
            // 
            button3.Location = new Point(298, 32);
            button3.Name = "button3";
            button3.Size = new Size(98, 60);
            button3.TabIndex = 2;
            button3.Text = "Запис з XML до БД";
            button3.UseVisualStyleBackColor = true;
            button3.Click += btnImportToDb_Click;
            // 
            // button4
            // 
            button4.Location = new Point(441, 32);
            button4.Name = "button4";
            button4.Size = new Size(113, 41);
            button4.TabIndex = 3;
            button4.Text = "Пошук у XML";
            button4.UseVisualStyleBackColor = true;
            button4.Click += btnSearch_Click;
            // 
            // button5
            // 
            button5.Location = new Point(600, 32);
            button5.Name = "button5";
            button5.Size = new Size(125, 41);
            button5.TabIndex = 4;
            button5.Text = "Фільтрація XML";
            button5.UseVisualStyleBackColor = true;
            button5.Click += btnFilter_Click;
            // 
            // txtSearchId
            // 
            txtSearchId.Location = new Point(441, 103);
            txtSearchId.Name = "txtSearchId";
            txtSearchId.Size = new Size(125, 27);
            txtSearchId.TabIndex = 5;
            // 
            // txtFilterId
            // 
            txtFilterId.Location = new Point(600, 88);
            txtFilterId.Name = "txtFilterId";
            txtFilterId.Size = new Size(125, 27);
            txtFilterId.TabIndex = 6;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(210, 203);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(515, 188);
            dataGridView1.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView1);
            Controls.Add(txtFilterId);
            Controls.Add(txtSearchId);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private TextBox txtSearchId;
        private TextBox txtFilterId;
        private DataGridView dataGridView1;
    }
}
