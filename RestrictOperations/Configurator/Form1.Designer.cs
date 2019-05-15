namespace Configurator
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
            this.m_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_tableLayoutPanel
            // 
            this.m_tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tableLayoutPanel.AutoScroll = true;
            this.m_tableLayoutPanel.ColumnCount = 1;
            this.m_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tableLayoutPanel.Location = new System.Drawing.Point(12, 48);
            this.m_tableLayoutPanel.Name = "m_tableLayoutPanel";
            this.m_tableLayoutPanel.RowCount = 1;
            this.m_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.m_tableLayoutPanel.Size = new System.Drawing.Size(260, 502);
            this.m_tableLayoutPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Check the boxes to restrict specific Vault commands:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 562);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_tableLayoutPanel);
            this.Name = "Form1";
            this.Text = "Configure Restrictions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel m_tableLayoutPanel;
        private System.Windows.Forms.Label label1;
    }
}

