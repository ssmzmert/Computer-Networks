namespace client
{
    partial class username_label
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label_directory = new System.Windows.Forms.Label();
            this.browse_button = new System.Windows.Forms.Button();
            this.Upload_button = new System.Windows.Forms.Button();
            this.username_box = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.disconnect_button = new System.Windows.Forms.Button();
            this.message_box = new System.Windows.Forms.TextBox();
            this.Copy_button = new System.Windows.Forms.Button();
            this.delete_button = new System.Windows.Forms.Button();
            this.Download_button = new System.Windows.Forms.Button();
            this.browse_for_download = new System.Windows.Forms.Button();
            this.Get_a_File = new System.Windows.Forms.Button();
            this.change_to_public = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 74);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(70, 35);
            this.textBox_ip.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(100, 20);
            this.textBox_ip.TabIndex = 2;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(70, 67);
            this.textBox_port.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(100, 20);
            this.textBox_port.TabIndex = 3;
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(100, 145);
            this.button_connect.Margin = new System.Windows.Forms.Padding(2);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(70, 22);
            this.button_connect.TabIndex = 4;
            this.button_connect.Text = "connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(205, 120);
            this.logs.Margin = new System.Windows.Forms.Padding(2);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(525, 175);
            this.logs.TabIndex = 5;
            this.logs.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(314, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "File:";
            // 
            // label_directory
            // 
            this.label_directory.AutoSize = true;
            this.label_directory.Location = new System.Drawing.Point(357, 51);
            this.label_directory.Name = "label_directory";
            this.label_directory.Size = new System.Drawing.Size(58, 13);
            this.label_directory.TabIndex = 10;
            this.label_directory.Text = "Directory...";
            // 
            // browse_button
            // 
            this.browse_button.Enabled = false;
            this.browse_button.Location = new System.Drawing.Point(387, 77);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(75, 23);
            this.browse_button.TabIndex = 11;
            this.browse_button.Text = "Browse";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.browse_button_Click);
            // 
            // Upload_button
            // 
            this.Upload_button.Enabled = false;
            this.Upload_button.Location = new System.Drawing.Point(496, 77);
            this.Upload_button.Name = "Upload_button";
            this.Upload_button.Size = new System.Drawing.Size(75, 23);
            this.Upload_button.TabIndex = 12;
            this.Upload_button.Text = "Upload";
            this.Upload_button.UseVisualStyleBackColor = true;
            this.Upload_button.Click += new System.EventHandler(this.Upload_button_Click);
            // 
            // username_box
            // 
            this.username_box.Location = new System.Drawing.Point(70, 101);
            this.username_box.Name = "username_box";
            this.username_box.Size = new System.Drawing.Size(100, 20);
            this.username_box.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Username:";
            // 
            // disconnect_button
            // 
            this.disconnect_button.Enabled = false;
            this.disconnect_button.Location = new System.Drawing.Point(100, 172);
            this.disconnect_button.Name = "disconnect_button";
            this.disconnect_button.Size = new System.Drawing.Size(70, 23);
            this.disconnect_button.TabIndex = 15;
            this.disconnect_button.Text = "disconnect";
            this.disconnect_button.UseVisualStyleBackColor = true;
            this.disconnect_button.Click += new System.EventHandler(this.disconnect_button_Click);
            // 
            // message_box
            // 
            this.message_box.Location = new System.Drawing.Point(790, 80);
            this.message_box.Name = "message_box";
            this.message_box.Size = new System.Drawing.Size(241, 20);
            this.message_box.TabIndex = 16;
            // 
            // Copy_button
            // 
            this.Copy_button.Enabled = false;
            this.Copy_button.Location = new System.Drawing.Point(790, 133);
            this.Copy_button.Name = "Copy_button";
            this.Copy_button.Size = new System.Drawing.Size(75, 23);
            this.Copy_button.TabIndex = 17;
            this.Copy_button.Text = "Copy";
            this.Copy_button.UseVisualStyleBackColor = true;
            this.Copy_button.Click += new System.EventHandler(this.Copy_button_Click);
            // 
            // delete_button
            // 
            this.delete_button.Enabled = false;
            this.delete_button.Location = new System.Drawing.Point(871, 133);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(75, 23);
            this.delete_button.TabIndex = 18;
            this.delete_button.Text = "Delete";
            this.delete_button.UseVisualStyleBackColor = true;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // Download_button
            // 
            this.Download_button.Enabled = false;
            this.Download_button.Location = new System.Drawing.Point(956, 133);
            this.Download_button.Name = "Download_button";
            this.Download_button.Size = new System.Drawing.Size(75, 23);
            this.Download_button.TabIndex = 19;
            this.Download_button.Text = "Download";
            this.Download_button.UseVisualStyleBackColor = true;
            this.Download_button.Click += new System.EventHandler(this.Download_button_Click);
            // 
            // browse_for_download
            // 
            this.browse_for_download.Enabled = false;
            this.browse_for_download.Location = new System.Drawing.Point(956, 172);
            this.browse_for_download.Name = "browse_for_download";
            this.browse_for_download.Size = new System.Drawing.Size(75, 41);
            this.browse_for_download.TabIndex = 20;
            this.browse_for_download.Text = "Browse for Download";
            this.browse_for_download.UseVisualStyleBackColor = true;
            this.browse_for_download.Click += new System.EventHandler(this.browse_for_download_Click);
            // 
            // Get_a_File
            // 
            this.Get_a_File.Enabled = false;
            this.Get_a_File.Location = new System.Drawing.Point(790, 172);
            this.Get_a_File.Margin = new System.Windows.Forms.Padding(2);
            this.Get_a_File.Name = "Get_a_File";
            this.Get_a_File.Size = new System.Drawing.Size(85, 41);
            this.Get_a_File.TabIndex = 21;
            this.Get_a_File.Text = "Get a File";
            this.Get_a_File.UseVisualStyleBackColor = true;
            this.Get_a_File.Click += new System.EventHandler(this.Get_a_File_Click);
            // 
            // change_to_public
            // 
            this.change_to_public.Enabled = false;
            this.change_to_public.Location = new System.Drawing.Point(790, 269);
            this.change_to_public.Name = "change_to_public";
            this.change_to_public.Size = new System.Drawing.Size(104, 26);
            this.change_to_public.TabIndex = 22;
            this.change_to_public.Text = "To Public";
            this.change_to_public.UseVisualStyleBackColor = true;
            this.change_to_public.Click += new System.EventHandler(this.change_to_public_Click);
            // 
            // username_label
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 345);
            this.Controls.Add(this.change_to_public);
            this.Controls.Add(this.Get_a_File);
            this.Controls.Add(this.browse_for_download);
            this.Controls.Add(this.Download_button);
            this.Controls.Add(this.delete_button);
            this.Controls.Add(this.Copy_button);
            this.Controls.Add(this.message_box);
            this.Controls.Add(this.disconnect_button);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.username_box);
            this.Controls.Add(this.Upload_button);
            this.Controls.Add(this.browse_button);
            this.Controls.Add(this.label_directory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "username_label";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_directory;
        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.Button Upload_button;
        private System.Windows.Forms.TextBox username_box;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button disconnect_button;
        private System.Windows.Forms.TextBox message_box;
        private System.Windows.Forms.Button Copy_button;
        private System.Windows.Forms.Button delete_button;
        private System.Windows.Forms.Button Download_button;
        private System.Windows.Forms.Button browse_for_download;
        private System.Windows.Forms.Button Get_a_File;
        private System.Windows.Forms.Button change_to_public;
    }
}

