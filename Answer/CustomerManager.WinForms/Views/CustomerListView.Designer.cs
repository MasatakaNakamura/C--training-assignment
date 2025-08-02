namespace CustomerManager.WinForms.Views
{
    partial class CustomerListView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewCustomers = new DataGridView();
            this.buttonAdd = new Button();
            this.buttonEdit = new Button();
            this.buttonDelete = new Button();
            this.buttonRefresh = new Button();
            this.labelStatus = new Label();
            this.progressBar = new ProgressBar();
            this.panel1 = new Panel();
            this.panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomers)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewCustomers
            // 
            this.dataGridViewCustomers.AllowUserToAddRows = false;
            this.dataGridViewCustomers.AllowUserToDeleteRows = false;
            this.dataGridViewCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCustomers.BackgroundColor = SystemColors.Window;
            this.dataGridViewCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCustomers.Dock = DockStyle.Fill;
            this.dataGridViewCustomers.Location = new Point(0, 0);
            this.dataGridViewCustomers.MultiSelect = false;
            this.dataGridViewCustomers.Name = "dataGridViewCustomers";
            this.dataGridViewCustomers.ReadOnly = true;
            this.dataGridViewCustomers.RowHeadersVisible = false;
            this.dataGridViewCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCustomers.Size = new Size(784, 361);
            this.dataGridViewCustomers.TabIndex = 0;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new Point(12, 12);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new Size(100, 35);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "新規登録(&N)";
            this.buttonAdd.UseVisualStyleBackColor = true;
            // 
            // buttonEdit
            // 
            this.buttonEdit.Enabled = false;
            this.buttonEdit.Location = new Point(118, 12);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new Size(100, 35);
            this.buttonEdit.TabIndex = 2;
            this.buttonEdit.Text = "編集(&E)";
            this.buttonEdit.UseVisualStyleBackColor = true;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Location = new Point(224, 12);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new Size(100, 35);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "削除(&D)";
            this.buttonDelete.UseVisualStyleBackColor = true;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new Point(330, 12);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new Size(100, 35);
            this.buttonRefresh.TabIndex = 4;
            this.buttonRefresh.Text = "更新(&R)";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new Point(12, 7);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new Size(67, 15);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "顧客数: 0件";
            // 
            // progressBar
            // 
            this.progressBar.Location = new Point(200, 3);
            this.progressBar.MarqueeAnimationSpeed = 50;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(200, 23);
            this.progressBar.Style = ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 6;
            this.progressBar.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonAdd);
            this.panel1.Controls.Add(this.buttonEdit);
            this.panel1.Controls.Add(this.buttonDelete);
            this.panel1.Controls.Add(this.buttonRefresh);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(784, 59);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelStatus);
            this.panel2.Controls.Add(this.progressBar);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 420);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(784, 30);
            this.panel2.TabIndex = 8;
            // 
            // CustomerListView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(784, 450);
            this.Controls.Add(this.dataGridViewCustomers);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.MinimumSize = new Size(600, 400);
            this.Name = "CustomerListView";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "顧客管理システム";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomers)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
    }
}