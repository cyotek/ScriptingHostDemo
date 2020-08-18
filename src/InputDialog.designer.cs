namespace Cyotek.Windows.Forms
{
  partial class InputDialog
  {
    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.promptLabel = new System.Windows.Forms.Label();
      this.inputTextBox = new System.Windows.Forms.TextBox();
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.footerLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // promptLabel
      // 
      this.promptLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.promptLabel.AutoEllipsis = true;
      this.promptLabel.Location = new System.Drawing.Point(9, 9);
      this.promptLabel.Name = "promptLabel";
      this.promptLabel.Size = new System.Drawing.Size(358, 13);
      this.promptLabel.TabIndex = 0;
      // 
      // inputTextBox
      // 
      this.inputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.inputTextBox.Location = new System.Drawing.Point(12, 25);
      this.inputTextBox.Name = "inputTextBox";
      this.inputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.inputTextBox.Size = new System.Drawing.Size(355, 20);
      this.inputTextBox.TabIndex = 1;
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.Location = new System.Drawing.Point(373, 12);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 2;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(373, 41);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 3;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // footerLabel
      // 
      this.footerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.footerLabel.AutoEllipsis = true;
      this.footerLabel.Location = new System.Drawing.Point(9, 75);
      this.footerLabel.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
      this.footerLabel.Name = "footerLabel";
      this.footerLabel.Size = new System.Drawing.Size(358, 13);
      this.footerLabel.TabIndex = 4;
      this.footerLabel.Visible = false;
      // 
      // InputDialog
      // 
      this.AcceptButton = this.okButton;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(460, 97);
      this.Controls.Add(this.footerLabel);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.inputTextBox);
      this.Controls.Add(this.promptLabel);
      this.Name = "InputDialog";
      this.Text = "Input";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label promptLabel;
    private System.Windows.Forms.TextBox inputTextBox;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Label footerLabel;
  }
}