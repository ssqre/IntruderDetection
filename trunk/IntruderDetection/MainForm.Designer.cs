namespace IntruderDetection
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.pictureVideo = new System.Windows.Forms.PictureBox();
            this.timerVideo = new System.Windows.Forms.Timer(this.components);
            this.checkVideo = new System.Windows.Forms.CheckBox();
            this.checkSound = new System.Windows.Forms.CheckBox();
            this.timerVideoAlarm = new System.Windows.Forms.Timer(this.components);
            this.timerSound = new System.Windows.Forms.Timer(this.components);
            this.progressSound = new System.Windows.Forms.ProgressBar();
            this.timerSoundAlarm = new System.Windows.Forms.Timer(this.components);
            this.checkVideoAlarm = new System.Windows.Forms.CheckBox();
            this.checkOpenEcho = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelVideoAlarm = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelSoundAlarm = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkSoundAlarm = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureVideo)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panelVideoAlarm.SuspendLayout();
            this.panelSoundAlarm.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureVideo
            // 
            this.pictureVideo.Location = new System.Drawing.Point(12, 12);
            this.pictureVideo.Name = "pictureVideo";
            this.pictureVideo.Size = new System.Drawing.Size(320, 240);
            this.pictureVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureVideo.TabIndex = 0;
            this.pictureVideo.TabStop = false;
            // 
            // timerVideo
            // 
            this.timerVideo.Tick += new System.EventHandler(this.timerVideo_Tick);
            // 
            // checkVideo
            // 
            this.checkVideo.AutoSize = true;
            this.checkVideo.Checked = true;
            this.checkVideo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkVideo.Location = new System.Drawing.Point(11, 27);
            this.checkVideo.Name = "checkVideo";
            this.checkVideo.Size = new System.Drawing.Size(114, 16);
            this.checkVideo.TabIndex = 1;
            this.checkVideo.Text = "Video Detection";
            this.checkVideo.UseVisualStyleBackColor = true;
            // 
            // checkSound
            // 
            this.checkSound.AutoSize = true;
            this.checkSound.Location = new System.Drawing.Point(10, 20);
            this.checkSound.Name = "checkSound";
            this.checkSound.Size = new System.Drawing.Size(114, 16);
            this.checkSound.TabIndex = 2;
            this.checkSound.Text = "Sound Detection";
            this.checkSound.UseVisualStyleBackColor = true;
            // 
            // timerVideoAlarm
            // 
            this.timerVideoAlarm.Tick += new System.EventHandler(this.timerVideoAlarm_Tick);
            // 
            // timerSound
            // 
            this.timerSound.Tick += new System.EventHandler(this.timerSound_Tick);
            // 
            // progressSound
            // 
            this.progressSound.Location = new System.Drawing.Point(12, 272);
            this.progressSound.Name = "progressSound";
            this.progressSound.Size = new System.Drawing.Size(320, 26);
            this.progressSound.TabIndex = 5;
            // 
            // timerSoundAlarm
            // 
            this.timerSoundAlarm.Tick += new System.EventHandler(this.timerSoundAlarm_Tick);
            // 
            // checkVideoAlarm
            // 
            this.checkVideoAlarm.AutoSize = true;
            this.checkVideoAlarm.Checked = true;
            this.checkVideoAlarm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkVideoAlarm.Location = new System.Drawing.Point(11, 57);
            this.checkVideoAlarm.Name = "checkVideoAlarm";
            this.checkVideoAlarm.Size = new System.Drawing.Size(90, 16);
            this.checkVideoAlarm.TabIndex = 6;
            this.checkVideoAlarm.Text = "Video Alarm";
            this.checkVideoAlarm.UseVisualStyleBackColor = true;
            // 
            // checkOpenEcho
            // 
            this.checkOpenEcho.AutoSize = true;
            this.checkOpenEcho.Location = new System.Drawing.Point(10, 67);
            this.checkOpenEcho.Name = "checkOpenEcho";
            this.checkOpenEcho.Size = new System.Drawing.Size(84, 16);
            this.checkOpenEcho.TabIndex = 7;
            this.checkOpenEcho.Text = "Sound Echo";
            this.checkOpenEcho.UseVisualStyleBackColor = true;
            this.checkOpenEcho.CheckedChanged += new System.EventHandler(this.checkOpenEcho_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkVideo);
            this.groupBox1.Controls.Add(this.checkVideoAlarm);
            this.groupBox1.Location = new System.Drawing.Point(357, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(133, 90);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Video";
            // 
            // panelVideoAlarm
            // 
            this.panelVideoAlarm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelVideoAlarm.Controls.Add(this.label1);
            this.panelVideoAlarm.Location = new System.Drawing.Point(357, 120);
            this.panelVideoAlarm.Name = "panelVideoAlarm";
            this.panelVideoAlarm.Size = new System.Drawing.Size(279, 82);
            this.panelVideoAlarm.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Abnormal Video";
            // 
            // panelSoundAlarm
            // 
            this.panelSoundAlarm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSoundAlarm.Controls.Add(this.label2);
            this.panelSoundAlarm.Location = new System.Drawing.Point(357, 216);
            this.panelSoundAlarm.Name = "panelSoundAlarm";
            this.panelSoundAlarm.Size = new System.Drawing.Size(279, 82);
            this.panelSoundAlarm.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Abnormal Sound";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkSound);
            this.groupBox2.Controls.Add(this.checkSoundAlarm);
            this.groupBox2.Controls.Add(this.checkOpenEcho);
            this.groupBox2.Location = new System.Drawing.Point(503, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(133, 90);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sound";
            // 
            // checkSoundAlarm
            // 
            this.checkSoundAlarm.AutoSize = true;
            this.checkSoundAlarm.Location = new System.Drawing.Point(10, 43);
            this.checkSoundAlarm.Name = "checkSoundAlarm";
            this.checkSoundAlarm.Size = new System.Drawing.Size(90, 16);
            this.checkSoundAlarm.TabIndex = 6;
            this.checkSoundAlarm.Text = "Sound Alarm";
            this.checkSoundAlarm.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 315);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panelSoundAlarm);
            this.Controls.Add(this.panelVideoAlarm);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressSound);
            this.Controls.Add(this.pictureVideo);
            this.Name = "MainForm";
            this.Text = "IntruderDetection";
            ((System.ComponentModel.ISupportInitialize)(this.pictureVideo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelVideoAlarm.ResumeLayout(false);
            this.panelVideoAlarm.PerformLayout();
            this.panelSoundAlarm.ResumeLayout(false);
            this.panelSoundAlarm.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureVideo;
        private System.Windows.Forms.Timer timerVideo;
        private System.Windows.Forms.CheckBox checkVideo;
        private System.Windows.Forms.CheckBox checkSound;
        private System.Windows.Forms.Timer timerVideoAlarm;
        private System.Windows.Forms.Timer timerSound;
        private System.Windows.Forms.ProgressBar progressSound;
        private System.Windows.Forms.Timer timerSoundAlarm;
        private System.Windows.Forms.CheckBox checkVideoAlarm;
        private System.Windows.Forms.CheckBox checkOpenEcho;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panelVideoAlarm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelSoundAlarm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkSoundAlarm;
    }
}

