partial class FrmMain
{
    /// <saummary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.TmrAnimation = new System.Windows.Forms.Timer(this.components);
            this.LstLibs = new System.Windows.Forms.ListBox();
            this.BtnInject = new System.Windows.Forms.Button();
            this.TxtNamespace = new System.Windows.Forms.TextBox();
            this.TxtClass = new System.Windows.Forms.TextBox();
            this.TxtMethod = new System.Windows.Forms.TextBox();
            this.LblMethod = new System.Windows.Forms.Label();
            this.LblProc = new System.Windows.Forms.Label();
            this.LblClass = new System.Windows.Forms.Label();
            this.LblNamespace = new System.Windows.Forms.Label();
            this.GrpData = new System.Windows.Forms.GroupBox();
            this.LnkRefresh = new System.Windows.Forms.LinkLabel();
            this.CmbProcs = new System.Windows.Forms.ComboBox();
            this.GrpLibs = new System.Windows.Forms.GroupBox();
            this.BtnRemove = new System.Windows.Forms.Button();
            this.BtnInsert = new System.Windows.Forms.Button();
            this.BtnEject = new System.Windows.Forms.Button();
            this.GrpData.SuspendLayout();
            this.GrpLibs.SuspendLayout();
            this.SuspendLayout();
            // 
            // TmrAnimation
            // 
            this.TmrAnimation.Enabled = true;
            this.TmrAnimation.Interval = 1;
            this.TmrAnimation.Tick += new System.EventHandler(this.TmrAnimation_Tick);
            // 
            // LstLibs
            // 
            this.LstLibs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LstLibs.FormattingEnabled = true;
            this.LstLibs.HorizontalScrollbar = true;
            this.LstLibs.Location = new System.Drawing.Point(8, 22);
            this.LstLibs.Name = "LstLibs";
            this.LstLibs.Size = new System.Drawing.Size(314, 54);
            this.LstLibs.TabIndex = 0;
            this.LstLibs.TabStop = false;
            // 
            // BtnInject
            // 
            this.BtnInject.BackColor = System.Drawing.Color.Black;
            this.BtnInject.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.BtnInject.FlatAppearance.BorderSize = 2;
            this.BtnInject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.BtnInject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.BtnInject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnInject.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnInject.ForeColor = System.Drawing.Color.White;
            this.BtnInject.Location = new System.Drawing.Point(12, 237);
            this.BtnInject.Name = "BtnInject";
            this.BtnInject.Size = new System.Drawing.Size(68, 23);
            this.BtnInject.TabIndex = 0;
            this.BtnInject.TabStop = false;
            this.BtnInject.Text = "Inject";
            this.BtnInject.UseVisualStyleBackColor = false;
            this.BtnInject.Click += new System.EventHandler(this.BtnInject_Click);
            // 
            // TxtNamespace
            // 
            this.TxtNamespace.Location = new System.Drawing.Point(149, 45);
            this.TxtNamespace.Name = "TxtNamespace";
            this.TxtNamespace.Size = new System.Drawing.Size(208, 20);
            this.TxtNamespace.TabIndex = 0;
            this.TxtNamespace.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtClass
            // 
            this.TxtClass.Location = new System.Drawing.Point(149, 71);
            this.TxtClass.Name = "TxtClass";
            this.TxtClass.Size = new System.Drawing.Size(208, 20);
            this.TxtClass.TabIndex = 1;
            this.TxtClass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtMethod
            // 
            this.TxtMethod.Location = new System.Drawing.Point(149, 97);
            this.TxtMethod.Name = "TxtMethod";
            this.TxtMethod.Size = new System.Drawing.Size(208, 20);
            this.TxtMethod.TabIndex = 2;
            this.TxtMethod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LblMethod
            // 
            this.LblMethod.AutoSize = true;
            this.LblMethod.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LblMethod.ForeColor = System.Drawing.Color.Black;
            this.LblMethod.Location = new System.Drawing.Point(41, 102);
            this.LblMethod.Name = "LblMethod";
            this.LblMethod.Size = new System.Drawing.Size(82, 11);
            this.LblMethod.TabIndex = 22;
            this.LblMethod.Text = "method name";
            // 
            // LblProc
            // 
            this.LblProc.AutoSize = true;
            this.LblProc.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LblProc.ForeColor = System.Drawing.Color.Black;
            this.LblProc.Location = new System.Drawing.Point(41, 24);
            this.LblProc.Name = "LblProc";
            this.LblProc.Size = new System.Drawing.Size(54, 11);
            this.LblProc.TabIndex = 19;
            this.LblProc.Text = "process";
            // 
            // LblClass
            // 
            this.LblClass.AutoSize = true;
            this.LblClass.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LblClass.ForeColor = System.Drawing.Color.Black;
            this.LblClass.Location = new System.Drawing.Point(41, 76);
            this.LblClass.Name = "LblClass";
            this.LblClass.Size = new System.Drawing.Size(75, 11);
            this.LblClass.TabIndex = 21;
            this.LblClass.Text = "class name";
            // 
            // LblNamespace
            // 
            this.LblNamespace.AutoSize = true;
            this.LblNamespace.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LblNamespace.ForeColor = System.Drawing.Color.Black;
            this.LblNamespace.Location = new System.Drawing.Point(41, 50);
            this.LblNamespace.Name = "LblNamespace";
            this.LblNamespace.Size = new System.Drawing.Size(68, 11);
            this.LblNamespace.TabIndex = 20;
            this.LblNamespace.Text = "namespace";
            // 
            // GrpData
            // 
            this.GrpData.Controls.Add(this.LnkRefresh);
            this.GrpData.Controls.Add(this.CmbProcs);
            this.GrpData.Controls.Add(this.TxtNamespace);
            this.GrpData.Controls.Add(this.LblNamespace);
            this.GrpData.Controls.Add(this.LblClass);
            this.GrpData.Controls.Add(this.TxtClass);
            this.GrpData.Controls.Add(this.LblProc);
            this.GrpData.Controls.Add(this.LblMethod);
            this.GrpData.Controls.Add(this.TxtMethod);
            this.GrpData.Location = new System.Drawing.Point(12, 6);
            this.GrpData.Name = "GrpData";
            this.GrpData.Size = new System.Drawing.Size(402, 130);
            this.GrpData.TabIndex = 25;
            this.GrpData.TabStop = false;
            this.GrpData.Text = "Data";
            // 
            // LnkRefresh
            // 
            this.LnkRefresh.AutoSize = true;
            this.LnkRefresh.Location = new System.Drawing.Point(318, 22);
            this.LnkRefresh.Name = "LnkRefresh";
            this.LnkRefresh.Size = new System.Drawing.Size(39, 13);
            this.LnkRefresh.TabIndex = 0;
            this.LnkRefresh.TabStop = true;
            this.LnkRefresh.Text = "refresh";
            this.LnkRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkRefresh_LinkClicked);
            // 
            // CmbProcs
            // 
            this.CmbProcs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbProcs.FormattingEnabled = true;
            this.CmbProcs.Location = new System.Drawing.Point(149, 18);
            this.CmbProcs.Name = "CmbProcs";
            this.CmbProcs.Size = new System.Drawing.Size(163, 21);
            this.CmbProcs.TabIndex = 0;
            this.CmbProcs.TabStop = false;
            this.CmbProcs.SelectedIndexChanged += new System.EventHandler(this.CmbProcs_SelectedIndexChanged);
            // 
            // GrpLibs
            // 
            this.GrpLibs.Controls.Add(this.BtnRemove);
            this.GrpLibs.Controls.Add(this.BtnInsert);
            this.GrpLibs.Controls.Add(this.LstLibs);
            this.GrpLibs.Location = new System.Drawing.Point(12, 142);
            this.GrpLibs.Name = "GrpLibs";
            this.GrpLibs.Size = new System.Drawing.Size(402, 89);
            this.GrpLibs.TabIndex = 26;
            this.GrpLibs.TabStop = false;
            this.GrpLibs.Text = "Libs";
            // 
            // BtnRemove
            // 
            this.BtnRemove.BackColor = System.Drawing.Color.Black;
            this.BtnRemove.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.BtnRemove.FlatAppearance.BorderSize = 2;
            this.BtnRemove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.BtnRemove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.BtnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRemove.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnRemove.ForeColor = System.Drawing.Color.White;
            this.BtnRemove.Location = new System.Drawing.Point(328, 52);
            this.BtnRemove.Name = "BtnRemove";
            this.BtnRemove.Size = new System.Drawing.Size(68, 23);
            this.BtnRemove.TabIndex = 0;
            this.BtnRemove.TabStop = false;
            this.BtnRemove.Text = "Remove";
            this.BtnRemove.UseVisualStyleBackColor = false;
            this.BtnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // BtnInsert
            // 
            this.BtnInsert.BackColor = System.Drawing.Color.Black;
            this.BtnInsert.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.BtnInsert.FlatAppearance.BorderSize = 2;
            this.BtnInsert.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.BtnInsert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.BtnInsert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnInsert.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnInsert.ForeColor = System.Drawing.Color.White;
            this.BtnInsert.Location = new System.Drawing.Point(328, 22);
            this.BtnInsert.Name = "BtnInsert";
            this.BtnInsert.Size = new System.Drawing.Size(68, 23);
            this.BtnInsert.TabIndex = 0;
            this.BtnInsert.TabStop = false;
            this.BtnInsert.Text = "Insert";
            this.BtnInsert.UseVisualStyleBackColor = false;
            this.BtnInsert.Click += new System.EventHandler(this.BtnInsert_Click);
            // 
            // BtnEject
            // 
            this.BtnEject.BackColor = System.Drawing.Color.Black;
            this.BtnEject.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.BtnEject.FlatAppearance.BorderSize = 2;
            this.BtnEject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.BtnEject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.BtnEject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnEject.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnEject.ForeColor = System.Drawing.Color.White;
            this.BtnEject.Location = new System.Drawing.Point(86, 237);
            this.BtnEject.Name = "BtnEject";
            this.BtnEject.Size = new System.Drawing.Size(68, 23);
            this.BtnEject.TabIndex = 28;
            this.BtnEject.TabStop = false;
            this.BtnEject.Text = "Eject";
            this.BtnEject.UseVisualStyleBackColor = false;
            this.BtnEject.Click += new System.EventHandler(this.BtnEject_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(426, 268);
            this.Controls.Add(this.BtnEject);
            this.Controls.Add(this.GrpData);
            this.Controls.Add(this.GrpLibs);
            this.Controls.Add(this.BtnInject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmMain";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Click += new System.EventHandler(this.FrmMain_Click);
            this.GrpData.ResumeLayout(false);
            this.GrpData.PerformLayout();
            this.GrpLibs.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Timer TmrAnimation;
    private System.Windows.Forms.ListBox LstLibs;
    private System.Windows.Forms.Button BtnInject;
    private System.Windows.Forms.TextBox TxtNamespace;
    private System.Windows.Forms.TextBox TxtClass;
    private System.Windows.Forms.TextBox TxtMethod;
    private System.Windows.Forms.Label LblMethod;
    private System.Windows.Forms.Label LblProc;
    private System.Windows.Forms.Label LblClass;
    private System.Windows.Forms.Label LblNamespace;
    private System.Windows.Forms.GroupBox GrpData;
    private System.Windows.Forms.GroupBox GrpLibs;
    private System.Windows.Forms.Button BtnRemove;
    private System.Windows.Forms.Button BtnInsert;
    private System.Windows.Forms.ComboBox CmbProcs;
    private System.Windows.Forms.LinkLabel LnkRefresh;
    private System.Windows.Forms.Button BtnEject;
}