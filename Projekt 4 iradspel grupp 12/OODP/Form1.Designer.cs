namespace FourInARow
{
    partial class Form1 // Form1 är huvudformuläret för spelet
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) // Metod för att rensa upp resurser
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Designer support method – do not modify.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ClientSize = new System.Drawing.Size(700, 600);
            this.Name = "Form1";
            this.Text = "4 i rad";
        }

        #endregion
    }
}