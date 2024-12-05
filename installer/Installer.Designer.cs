using System.ComponentModel;

namespace installer;

partial class Installer {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        ViewController = new System.Windows.Forms.TabControl();
        View1 = new System.Windows.Forms.TabPage();
        CancelButton = new System.Windows.Forms.Button();
        InstallButton = new System.Windows.Forms.Button();
        EnvVarsCheckbox = new System.Windows.Forms.CheckBox();
        textBox1 = new System.Windows.Forms.TextBox();
        label2 = new System.Windows.Forms.Label();
        label1 = new System.Windows.Forms.Label();
        View2 = new System.Windows.Forms.TabPage();
        label3 = new System.Windows.Forms.Label();
        InstallProgressBar = new System.Windows.Forms.ProgressBar();
        InstallLogs = new System.Windows.Forms.TextBox();
        View3 = new System.Windows.Forms.TabPage();
        label4 = new System.Windows.Forms.Label();
        CloseButton = new System.Windows.Forms.Button();
        ViewController.SuspendLayout();
        View1.SuspendLayout();
        View2.SuspendLayout();
        View3.SuspendLayout();
        SuspendLayout();
        // 
        // ViewController
        // 
        ViewController.Controls.Add(View1);
        ViewController.Controls.Add(View2);
        ViewController.Controls.Add(View3);
        ViewController.Location = new System.Drawing.Point(-4, 0);
        ViewController.Name = "ViewController";
        ViewController.SelectedIndex = 0;
        ViewController.Size = new System.Drawing.Size(455, 203);
        ViewController.TabIndex = 0;
        // 
        // View1
        // 
        View1.Controls.Add(CancelButton);
        View1.Controls.Add(InstallButton);
        View1.Controls.Add(EnvVarsCheckbox);
        View1.Controls.Add(textBox1);
        View1.Controls.Add(label2);
        View1.Controls.Add(label1);
        View1.Location = new System.Drawing.Point(4, 24);
        View1.Name = "View1";
        View1.Padding = new System.Windows.Forms.Padding(3);
        View1.Size = new System.Drawing.Size(447, 175);
        View1.TabIndex = 0;
        View1.Text = "View1";
        View1.UseVisualStyleBackColor = true;
        // 
        // CancelButton
        // 
        CancelButton.Location = new System.Drawing.Point(220, 139);
        CancelButton.Name = "CancelButton";
        CancelButton.Size = new System.Drawing.Size(103, 23);
        CancelButton.TabIndex = 5;
        CancelButton.Text = "Cancel";
        CancelButton.UseVisualStyleBackColor = true;
        CancelButton.Click += CancelButton_Click;
        // 
        // InstallButton
        // 
        InstallButton.Location = new System.Drawing.Point(329, 139);
        InstallButton.Name = "InstallButton";
        InstallButton.Size = new System.Drawing.Size(103, 23);
        InstallButton.TabIndex = 4;
        InstallButton.Text = "Install";
        InstallButton.UseVisualStyleBackColor = true;
        InstallButton.Click += InstallButton_Click;
        // 
        // EnvVarsCheckbox
        // 
        EnvVarsCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
        EnvVarsCheckbox.Checked = true;
        EnvVarsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
        EnvVarsCheckbox.Location = new System.Drawing.Point(13, 103);
        EnvVarsCheckbox.Name = "EnvVarsCheckbox";
        EnvVarsCheckbox.Size = new System.Drawing.Size(420, 22);
        EnvVarsCheckbox.TabIndex = 3;
        EnvVarsCheckbox.Text = "Automatically add BBPackager to the PATH environment variable\r\n";
        EnvVarsCheckbox.UseVisualStyleBackColor = true;
        // 
        // textBox1
        // 
        textBox1.Location = new System.Drawing.Point(13, 68);
        textBox1.Name = "textBox1";
        textBox1.ReadOnly = true;
        textBox1.Size = new System.Drawing.Size(420, 23);
        textBox1.TabIndex = 2;
        textBox1.Text = "C:\\Program Files (x86)\\bbpackager\\";
        // 
        // label2
        // 
        label2.Location = new System.Drawing.Point(10, 48);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(424, 17);
        label2.TabIndex = 1;
        label2.Text = "BBPackager will be installed for all users in the following directory:";
        // 
        // label1
        // 
        label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        label1.Location = new System.Drawing.Point(10, 9);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(425, 25);
        label1.TabIndex = 0;
        label1.Text = "Welcome to the BBPackager install wizard";
        // 
        // View2
        // 
        View2.Controls.Add(InstallLogs);
        View2.Controls.Add(InstallProgressBar);
        View2.Controls.Add(label3);
        View2.Location = new System.Drawing.Point(4, 24);
        View2.Name = "View2";
        View2.Padding = new System.Windows.Forms.Padding(3);
        View2.Size = new System.Drawing.Size(447, 175);
        View2.TabIndex = 1;
        View2.Text = "View2";
        View2.UseVisualStyleBackColor = true;
        // 
        // label3
        // 
        label3.Location = new System.Drawing.Point(14, 14);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(419, 20);
        label3.TabIndex = 0;
        label3.Text = "Installing...";
        // 
        // InstallProgressBar
        // 
        InstallProgressBar.Location = new System.Drawing.Point(14, 37);
        InstallProgressBar.Name = "InstallProgressBar";
        InstallProgressBar.Size = new System.Drawing.Size(419, 27);
        InstallProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
        InstallProgressBar.TabIndex = 1;
        // 
        // InstallLogs
        // 
        InstallLogs.Location = new System.Drawing.Point(14, 77);
        InstallLogs.Multiline = true;
        InstallLogs.Name = "InstallLogs";
        InstallLogs.ReadOnly = true;
        InstallLogs.Size = new System.Drawing.Size(419, 83);
        InstallLogs.TabIndex = 2;
        // 
        // View3
        // 
        View3.Controls.Add(CloseButton);
        View3.Controls.Add(label4);
        View3.Location = new System.Drawing.Point(4, 24);
        View3.Name = "View3";
        View3.Padding = new System.Windows.Forms.Padding(3);
        View3.Size = new System.Drawing.Size(447, 175);
        View3.TabIndex = 2;
        View3.Text = "View3";
        View3.UseVisualStyleBackColor = true;
        // 
        // label4
        // 
        label4.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        label4.Location = new System.Drawing.Point(12, 13);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(418, 22);
        label4.TabIndex = 0;
        label4.Text = "The installation has completed.";
        // 
        // CloseButton
        // 
        CloseButton.Location = new System.Drawing.Point(305, 130);
        CloseButton.Name = "CloseButton";
        CloseButton.Size = new System.Drawing.Size(125, 27);
        CloseButton.TabIndex = 1;
        CloseButton.Text = "Close";
        CloseButton.UseVisualStyleBackColor = true;
        // 
        // Installer
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(447, 199);
        Controls.Add(ViewController);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "BBPackager Install Wizard";
        ViewController.ResumeLayout(false);
        View1.ResumeLayout(false);
        View1.PerformLayout();
        View2.ResumeLayout(false);
        View2.PerformLayout();
        View3.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button CloseButton;

    private System.Windows.Forms.TabPage View3;
    private System.Windows.Forms.Label label4;

    private System.Windows.Forms.TextBox InstallLogs;

    private System.Windows.Forms.ProgressBar InstallProgressBar;

    private System.Windows.Forms.Label label3;

    private System.Windows.Forms.Button InstallButton;
    private System.Windows.Forms.Button CancelButton;

    private System.Windows.Forms.CheckBox EnvVarsCheckbox;

    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox textBox1;

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.TabControl ViewController;
    private System.Windows.Forms.TabPage View1;
    private System.Windows.Forms.TabPage View2;

    #endregion
}