namespace CustomerManager.WinForms.Views
{
    partial class CustomerEditView
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
            this.textBoxName = new TextBox();
            this.textBoxKana = new TextBox();
            this.textBoxPhoneNumber = new TextBox();
            this.textBoxEmail = new TextBox();
            this.buttonSave = new Button();
            this.buttonCancel = new Button();
            this.labelName = new Label();
            this.labelKana = new Label();
            this.labelPhoneNumber = new Label();
            this.labelEmail = new Label();
            this.labelNameError = new Label();
            this.labelKanaError = new Label();
            this.labelPhoneNumberError = new Label();
            this.labelEmailError = new Label();
            this.labelGeneralError = new Label();
            this.progressBar = new ProgressBar();
            this.panelButtons = new Panel();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new Point(120, 20);
            this.textBoxName.MaxLength = 255;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new Size(250, 23);
            this.textBoxName.TabIndex = 1;
            // 
            // textBoxKana
            // 
            this.textBoxKana.Location = new Point(120, 70);
            this.textBoxKana.MaxLength = 255;
            this.textBoxKana.Name = "textBoxKana";
            this.textBoxKana.Size = new Size(250, 23);
            this.textBoxKana.TabIndex = 2;
            // 
            // textBoxPhoneNumber
            // 
            this.textBoxPhoneNumber.Location = new Point(120, 120);
            this.textBoxPhoneNumber.MaxLength = 20;
            this.textBoxPhoneNumber.Name = "textBoxPhoneNumber";
            this.textBoxPhoneNumber.Size = new Size(250, 23);
            this.textBoxPhoneNumber.TabIndex = 3;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new Point(120, 170);
            this.textBoxEmail.MaxLength = 255;
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new Size(250, 23);
            this.textBoxEmail.TabIndex = 4;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new Point(114, 10);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new Size(100, 35);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "登録(&S)";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(220, 10);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(100, 35);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "キャンセル(&C)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new Point(20, 23);
            this.labelName.Name = "labelName";
            this.labelName.Size = new Size(55, 15);
            this.labelName.TabIndex = 7;
            this.labelName.Text = "氏名 (*)";
            // 
            // labelKana
            // 
            this.labelKana.AutoSize = true;
            this.labelKana.Location = new Point(20, 73);
            this.labelKana.Name = "labelKana";
            this.labelKana.Size = new Size(54, 15);
            this.labelKana.TabIndex = 8;
            this.labelKana.Text = "フリガナ";
            // 
            // labelPhoneNumber
            // 
            this.labelPhoneNumber.AutoSize = true;
            this.labelPhoneNumber.Location = new Point(20, 123);
            this.labelPhoneNumber.Name = "labelPhoneNumber";
            this.labelPhoneNumber.Size = new Size(55, 15);
            this.labelPhoneNumber.TabIndex = 9;
            this.labelPhoneNumber.Text = "電話番号";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new Point(20, 173);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new Size(94, 15);
            this.labelEmail.TabIndex = 10;
            this.labelEmail.Text = "メールアドレス (*)";
            // 
            // labelNameError
            // 
            this.labelNameError.AutoSize = true;
            this.labelNameError.ForeColor = Color.Red;
            this.labelNameError.Location = new Point(120, 46);
            this.labelNameError.Name = "labelNameError";
            this.labelNameError.Size = new Size(0, 15);
            this.labelNameError.TabIndex = 11;
            this.labelNameError.Visible = false;
            // 
            // labelKanaError
            // 
            this.labelKanaError.AutoSize = true;
            this.labelKanaError.ForeColor = Color.Red;
            this.labelKanaError.Location = new Point(120, 96);
            this.labelKanaError.Name = "labelKanaError";
            this.labelKanaError.Size = new Size(0, 15);
            this.labelKanaError.TabIndex = 12;
            this.labelKanaError.Visible = false;
            // 
            // labelPhoneNumberError
            // 
            this.labelPhoneNumberError.AutoSize = true;
            this.labelPhoneNumberError.ForeColor = Color.Red;
            this.labelPhoneNumberError.Location = new Point(120, 146);
            this.labelPhoneNumberError.Name = "labelPhoneNumberError";
            this.labelPhoneNumberError.Size = new Size(0, 15);
            this.labelPhoneNumberError.TabIndex = 13;
            this.labelPhoneNumberError.Visible = false;
            // 
            // labelEmailError
            // 
            this.labelEmailError.AutoSize = true;
            this.labelEmailError.ForeColor = Color.Red;
            this.labelEmailError.Location = new Point(120, 196);
            this.labelEmailError.Name = "labelEmailError";
            this.labelEmailError.Size = new Size(0, 15);
            this.labelEmailError.TabIndex = 14;
            this.labelEmailError.Visible = false;
            // 
            // labelGeneralError
            // 
            this.labelGeneralError.AutoSize = true;
            this.labelGeneralError.ForeColor = Color.Red;
            this.labelGeneralError.Location = new Point(20, 220);
            this.labelGeneralError.MaximumSize = new Size(350, 0);
            this.labelGeneralError.Name = "labelGeneralError";
            this.labelGeneralError.Size = new Size(0, 15);
            this.labelGeneralError.TabIndex = 15;
            this.labelGeneralError.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new Point(20, 250);
            this.progressBar.MarqueeAnimationSpeed = 50;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(350, 23);
            this.progressBar.Style = ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 16;
            this.progressBar.Visible = false;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.buttonSave);
            this.panelButtons.Controls.Add(this.buttonCancel);
            this.panelButtons.Dock = DockStyle.Bottom;
            this.panelButtons.Location = new Point(0, 285);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new Size(394, 56);
            this.panelButtons.TabIndex = 17;
            // 
            // CustomerEditView
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(394, 341);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelGeneralError);
            this.Controls.Add(this.labelEmailError);
            this.Controls.Add(this.labelPhoneNumberError);
            this.Controls.Add(this.labelKanaError);
            this.Controls.Add(this.labelNameError);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelPhoneNumber);
            this.Controls.Add(this.labelKana);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxEmail);
            this.Controls.Add(this.textBoxPhoneNumber);
            this.Controls.Add(this.textBoxKana);
            this.Controls.Add(this.textBoxName);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomerEditView";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "顧客登録";
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Panel panelButtons;
    }
}