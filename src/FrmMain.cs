using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

internal sealed partial class FrmMain : Form
{
    private readonly struct InjectStatus
    {
        internal static readonly string PREPARED = "[PREPARED]";
        internal static readonly string FAILED = "[FAILED]";
        internal static readonly string LOADED = "[LOADED]";
    }

    private readonly List<StoredData> st_data = new List<StoredData>();
    private const int COLOR_CHANGE_SPEED = 6;
    private const int COLOR_LESS = 0;
    private const int COLOR_FULL = 255;
    private const int COLOR_RED_INC = 0;
    private const int COLOR_GREEN_INC = 1;
    private const int COLOR_BLUE_INC = 2;
    private const int COLOR_RED_DEC = 3;
    private const int COLOR_GREEN_DEC = 4;
    private const int COLOR_BLUE_DEC = 5;

    private int red = 0;
    private int green = 0;
    private int blue = 0;
    private int colorize_mode = 0;
    private string lib_n = "";
    private string lib_c = "";
    private string lib_m = "";
    private string title;

    internal FrmMain() {
        InitializeComponent();
    }

    private void Browse() {
        using (OpenFileDialog dlg = new OpenFileDialog()) {
            byte[] bytes = null;
            string path;

            dlg.Filter = "dll files (*.dll)|*.dll";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK) {
                bytes = File.ReadAllBytes(dlg.FileName);
                if (bytes == null) {
                    MessageBox.Show("This library is empty.", "Main", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                path = dlg.FileName;

                foreach (object item in this.LstLibs.Items) {
                    if (item.ToString().Contains(path)) {
                        MessageBox.Show("This library already added.", "Main", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        return;
                    }
                }

                this.LstLibs.Items.Add(InjectStatus.PREPARED + " " + path);
            }
        }
    }

    private void ProcExited(object sender, EventArgs e) {
        if (this.CmbProcs.InvokeRequired) {
            this.CmbProcs.Invoke((MethodInvoker) delegate {
                ProcPullSafe((Process) sender);
            });
        }
    }

    private void ProcPullSafe(Process proc) {
        // remove all loaded libs of the exited process
        for (int i = this.LstLibs.Items.Count - 1; i >= 0; i--) {
            string item = this.LstLibs.Items[i].ToString();

            if (item.Contains(InjectStatus.LOADED))
                this.LstLibs.Items.Remove(item);
        }

        foreach (object item in this.CmbProcs.Items) {
            if (item.ToString().Split(null)[0] == proc.Id.ToString()) {
                this.CmbProcs.Items.Remove(item);

                break;
            }
        }

        DataRemoveByProc(proc);

        if (this.CmbProcs.Items.Count > 0)
            this.CmbProcs.SelectedIndex = 0;
    }

    private void GetMonoProcesses() {
        int count_old = this.CmbProcs.Items.Count;

        foreach (Process proc in Process.GetProcesses()) {
            try {
                IntPtr mod_addr;
                IntPtr h = proc.Handle;
                int pid = proc.Id;
                bool proc_capacity = Utils.GetProcExt(h);

                mod_addr = Utils.GetMonoModule(h, proc_capacity);
                if (mod_addr != IntPtr.Zero && !CheckDataIdent(proc.Id)) {
                    this.st_data.Add(new StoredData() {
                        proc = proc,
                        proc_capacity = proc_capacity,
                        mod_addr = mod_addr,
                        asmp = IntPtr.Zero,
                        libs = new List<string>()
                    });

                    this.CmbProcs.Items.Add(pid + " (" + proc.ProcessName + ")");

                    proc.EnableRaisingEvents = true;
                    proc.Exited += new EventHandler(ProcExited);
                }
            } catch (Exception e) when (e is Win32Exception || e is InvalidOperationException) { }
        }

        if (this.CmbProcs.Items.Count > count_old && count_old == 0)
            this.CmbProcs.SelectedIndex = 0;
    }

    private void DataRemoveByProc(Process proc) {
        StoredData data;

        for (int i = 0; i < this.st_data.Count; i++) {
            data = this.st_data[i];
            if (data.proc == proc) {
                this.st_data.RemoveAt(i);

                break;
            }
        }
    }

    private bool CheckDataIdent(int pid) {
        foreach (StoredData data in this.st_data) {
            if (data.proc.Id == pid)
                return true;
        }

        return false;
    }

    private int GetDataIndex(StoredData data) {
        return this.st_data.FindIndex(item => item.proc == data.proc);
    }

    private StoredData GetDataByPid(int pid) {
        foreach (StoredData data in this.st_data) {
            if (data.proc.Id == pid)
                return data;
        }

        return new StoredData() {
            proc = null
        };
    }

    private StoredData GetCurrData() {
        int pid;

        pid = GetCurrProcPid();
        if (pid == -1) {
            return new StoredData() {
                proc = null
            };
        }

        return GetDataByPid(pid);
    }

    private int GetCurrProcPid() {
        int pid;

        try {
            pid = int.Parse(this.CmbProcs.SelectedItem.ToString().Split(null)[0]);
        } catch (NullReferenceException) {
            return -1;
        }

        return pid;
    }

    private void SetLibStatus(int index, string status, string path) {
        this.LstLibs.Items.RemoveAt(index);
        this.LstLibs.Items.Add(status + " " + path);
    }

    private void DataUpdateBy(StoredData data) {
        this.st_data.RemoveAt(GetDataIndex(data));
        this.st_data.Add(data);
    }

    private void BtnInsert_Click(object sender, EventArgs e) {
        Browse();
    }

    private void BtnRemove_Click(object sender, EventArgs e) {
        int index;
        string item;

        index = this.LstLibs.SelectedIndex;
        if (index == -1) {
            MessageBox.Show("No selected library.", "Main", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        item = this.LstLibs.Items[index].ToString();
        if (item.Contains(InjectStatus.LOADED)) {
            MessageBox.Show("Can't remove this library: eject it, firstly.", "Main",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            return;
        }

        this.LstLibs.Items.RemoveAt(index);
    }

    private void BtnInject_Click(object sender, EventArgs e) {
        StoredData st_data;
        InjectData inj_data;
        int index;
        string lib_path;
        uint status;
        string item_loaded;

        index = this.LstLibs.SelectedIndex;
        if (index == -1) {
            MessageBox.Show("No selected library.", "Main", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        lib_path = this.LstLibs.Items[index].ToString().Split(null)[1];

        if (!File.Exists(lib_path)) {
            MessageBox.Show("The library not found.", "Main", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        item_loaded = InjectStatus.LOADED + " " + lib_path;

        if (this.LstLibs.Items.Contains(item_loaded)) {
            MessageBox.Show("This library already injected.", "Main", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        st_data = GetCurrData();
        if (st_data.proc == null) {
            MessageBox.Show("No selected process.", "Main", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        if (string.IsNullOrEmpty(this.lib_n = this.TxtNamespace.Text)
                || string.IsNullOrEmpty(this.lib_c = this.TxtClass.Text)
                || string.IsNullOrEmpty(this.lib_m = this.TxtMethod.Text)) {
            this.TmrAnimation.Enabled = false;
            this.BtnInject.ForeColor = Color.Red;

            MessageBox.Show("One or more fields is empty.", "Main", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

            this.TmrAnimation.Enabled = true;

            return;
        }

        this.Text = this.title;

        if (st_data.proc_capacity == Utils.WIN64)
            this.Text += " (x64)"; /* Hello, GH Mono-Injector! */

        inj_data.proc = st_data.proc;
        inj_data.proc_extended = st_data.proc_capacity;
        inj_data.mod_addr = st_data.mod_addr;
        inj_data.asmp = st_data.asmp;
        inj_data.lib_path = lib_path;
        inj_data.lib_n = this.lib_n;
        inj_data.lib_c = this.lib_c;
        inj_data.lib_m = this.lib_m;

        status = Injector.Inject(inj_data);
        if (status != 0) {
            switch (status) {
            case Injector.ERROR_MONO_FAILED:
                MessageBox.Show("Failed to get Mono addresses.", "Injection", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                break;
            case Injector.ERROR_CLASS_NOT_FOUND:
                MessageBox.Show("Couldn't get a pointer to the " + inj_data.lib_c + " class.",
                    "Injection", MessageBoxButtons.OK, MessageBoxIcon.Error);

                break;
            case Injector.ERROR_METHOD_NOT_FOUND:
                MessageBox.Show("Couldn't get a pointer to the " + inj_data.lib_m + " method.",
                    "Injection", MessageBoxButtons.OK, MessageBoxIcon.Error);

                break;
            case Injector.ERROR_INVOKE_FAILED:
                MessageBox.Show("Failed to invoke the " + inj_data.lib_m + " method, of the " +
                    inj_data.lib_c + " class.", "Injection", MessageBoxButtons.OK, MessageBoxIcon.Error);

                break;
            }

            SetLibStatus(index, InjectStatus.FAILED, lib_path);
        } else {
            SetLibStatus(index, InjectStatus.LOADED, lib_path);

            st_data.libs.Add(item_loaded);
        }

        if (st_data.proc.HasExited) {
            MessageBox.Show("Possibly, the library is corrupted.", "Main", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        st_data.asmp = Injector.asmp;

        DataUpdateBy(st_data);
    }

    private void BtnEject_Click(object sender, EventArgs e) {
        StoredData data = GetCurrData();
        int index;
        string item;
        string lib_path;

        index = this.LstLibs.SelectedIndex;
        if (index == -1) {
            MessageBox.Show("No selected library.", "Main", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        item = this.LstLibs.Items[index].ToString();
        lib_path = item.Split(null)[1];

        if (!item.Contains(InjectStatus.LOADED)) {
            MessageBox.Show("This library isn't injected.", "Main", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        if (string.IsNullOrEmpty(this.lib_n = this.TxtNamespace.Text)
                || string.IsNullOrEmpty(this.lib_c = this.TxtClass.Text)
                || string.IsNullOrEmpty(this.lib_m = this.TxtMethod.Text)) {
            MessageBox.Show("One or more fields is empty.", "Main", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

            return;
        }

        if (MessageBox.Show("Be accurate, because the game may crash due to an incorrect input. " +
                "And, a further behaviour after injecting the same library, in the same opened process, " +
                "is undefined.", "Main", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information) == DialogResult.Cancel)
            return;

        Injector.Eject(data, this.lib_n, this.lib_c, this.lib_m);

        if (data.proc.HasExited)
            return;

        SetLibStatus(index, InjectStatus.PREPARED, lib_path);

        data.asmp = Injector.asmp;
        data.libs.Remove(item);

        DataUpdateBy(data);
    }

    private void FrmMain_Load(object sender, EventArgs e) {
        this.title = this.Text;
        this.LnkRefresh.TabStop = false;
        this.TxtNamespace.ContextMenuStrip = new ContextMenuStrip();
        this.TxtClass.ContextMenuStrip = new ContextMenuStrip();
        this.TxtMethod.ContextMenuStrip = new ContextMenuStrip();

        GetMonoProcesses();
    }

    private void FrmMain_Click(object sender, EventArgs e) {
        this.LstLibs.ClearSelected();
        this.LblProc.Focus();
    }

    private void CmbProcs_SelectedIndexChanged(object sender, EventArgs e) {
        // remove loaded libs of the previous selected process
        for (int i = 0; i < this.LstLibs.Items.Count; i++) {
            string item = this.LstLibs.Items[i].ToString();

            if (item.Contains(InjectStatus.LOADED))
                this.LstLibs.Items.Remove(item);
        }

        // add loaded libs of the current selected process
        foreach (string lib in GetCurrData().libs)
            this.LstLibs.Items.Add(lib);
    }

    private void LnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
        GetMonoProcesses();
    }

    private void TmrAnimation_Tick(object sender, EventArgs e) {
        switch (this.colorize_mode) {
        case 0:
            this.red += COLOR_CHANGE_SPEED;

            if (this.red > COLOR_FULL) {
                this.red = COLOR_FULL;

                this.colorize_mode = COLOR_GREEN_INC;
            }

            break;
        case 1:
            this.green += COLOR_CHANGE_SPEED;

            if (this.green > COLOR_FULL) {
                this.green = COLOR_FULL;

                this.colorize_mode = COLOR_BLUE_INC;
            }

            break;
        case 2:
            if (this.blue == COLOR_FULL) {
                this.colorize_mode = COLOR_BLUE_DEC;

                break;
            }

            this.blue += COLOR_CHANGE_SPEED;

            if (this.blue > COLOR_FULL) {
                this.blue = COLOR_FULL;

                this.colorize_mode = COLOR_RED_DEC;
            }

            break;
        case 3:
            this.red -= COLOR_CHANGE_SPEED;

            if (this.red < COLOR_LESS) {
                this.red = COLOR_LESS;

                this.colorize_mode = COLOR_GREEN_DEC;
            }

            break;
        case 4:
            this.green -= COLOR_CHANGE_SPEED;

            if (this.green < COLOR_LESS) {
                this.green = COLOR_LESS;

                this.colorize_mode = COLOR_RED_INC;
            }

            break;
        case 5:
            this.blue -= COLOR_CHANGE_SPEED;

            if (this.blue < COLOR_LESS) {
                this.blue = COLOR_LESS;

                this.colorize_mode = COLOR_RED_DEC;
            }

            break;
        }

        this.BtnInject.ForeColor = Color.FromArgb(0, this.red, this.green, this.blue);
    }
}