namespace WcfClient
{
  partial class Form1
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
      this.btnResult = new System.Windows.Forms.Button();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.btnClose = new System.Windows.Forms.Button();
      this.rbResult = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // btnResult
      // 
      this.btnResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnResult.Location = new System.Drawing.Point(122, 103);
      this.btnResult.Name = "btnResult";
      this.btnResult.Size = new System.Drawing.Size(75, 23);
      this.btnResult.TabIndex = 0;
      this.btnResult.Text = "Get result";
      this.btnResult.UseVisualStyleBackColor = true;
      this.btnResult.Click += new System.EventHandler(this.btnResult_Click);
      // 
      // textBox1
      // 
      this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox1.Location = new System.Drawing.Point(12, 12);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(266, 20);
      this.textBox1.TabIndex = 2;
      this.textBox1.Text = "5";
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.Location = new System.Drawing.Point(203, 103);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 3;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // rbResult
      // 
      this.rbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.rbResult.Location = new System.Drawing.Point(12, 38);
      this.rbResult.Name = "rbResult";
      this.rbResult.ReadOnly = true;
      this.rbResult.Size = new System.Drawing.Size(266, 59);
      this.rbResult.TabIndex = 4;
      this.rbResult.Text = "";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(290, 138);
      this.Controls.Add(this.rbResult);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.btnResult);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnResult;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.RichTextBox rbResult;
  }
}

