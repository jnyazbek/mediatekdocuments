namespace MediaTekDocuments
{
    partial class FormAuth
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
            this.textBoxMdp = new System.Windows.Forms.TextBox();
            this.textBoxId = new System.Windows.Forms.TextBox();
            this.labelMdp = new System.Windows.Forms.Label();
            this.labelId = new System.Windows.Forms.Label();
            this.buttonConnexion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxMdp
            // 
            this.textBoxMdp.Location = new System.Drawing.Point(305, 182);
            this.textBoxMdp.Name = "textBoxMdp";
            this.textBoxMdp.Size = new System.Drawing.Size(190, 20);
            this.textBoxMdp.TabIndex = 10;
            // 
            // textBoxId
            // 
            this.textBoxId.Location = new System.Drawing.Point(305, 87);
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.Size = new System.Drawing.Size(190, 20);
            this.textBoxId.TabIndex = 9;
            // 
            // labelMdp
            // 
            this.labelMdp.AutoSize = true;
            this.labelMdp.Location = new System.Drawing.Point(359, 136);
            this.labelMdp.Name = "labelMdp";
            this.labelMdp.Size = new System.Drawing.Size(71, 13);
            this.labelMdp.TabIndex = 8;
            this.labelMdp.Text = "Mot de passe";
            // 
            // labelId
            // 
            this.labelId.AutoSize = true;
            this.labelId.Location = new System.Drawing.Point(359, 57);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(53, 13);
            this.labelId.TabIndex = 7;
            this.labelId.Text = "Identifiant";
            // 
            // buttonConnexion
            // 
            this.buttonConnexion.Location = new System.Drawing.Point(362, 237);
            this.buttonConnexion.Name = "buttonConnexion";
            this.buttonConnexion.Size = new System.Drawing.Size(75, 23);
            this.buttonConnexion.TabIndex = 6;
            this.buttonConnexion.Text = "Connexion ";
            this.buttonConnexion.UseVisualStyleBackColor = true;
            this.buttonConnexion.Click += new System.EventHandler(this.buttonConnexion_Click);
            // 
            // FormAuth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxMdp);
            this.Controls.Add(this.textBoxId);
            this.Controls.Add(this.labelMdp);
            this.Controls.Add(this.labelId);
            this.Controls.Add(this.buttonConnexion);
            this.Name = "FormAuth";
            this.Text = "Authetification";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxMdp;
        private System.Windows.Forms.TextBox textBoxId;
        private System.Windows.Forms.Label labelMdp;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.Button buttonConnexion;
    }
}