namespace HelloWorld
{
    partial class MyCustomTabControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // mPropertyGrid
            // 
            this.mPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.mPropertyGrid.Name = "mPropertyGrid";
            this.mPropertyGrid.Size = new System.Drawing.Size(215, 150);
            this.mPropertyGrid.TabIndex = 0;
            // 
            // MyCustomTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mPropertyGrid);
            this.Name = "MyCustomTabControl";
            this.Size = new System.Drawing.Size(215, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid mPropertyGrid;
    }
}
