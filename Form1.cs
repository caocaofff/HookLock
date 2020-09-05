using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;

namespace HookLock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private double opacity;
        private string pw;
        Hook h = new Hook();

        public void getinfo(string pwd, double op)
        {
            this.pw = pwd;
            this.opacity = op;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            base.Opacity = this.opacity / 10.0;
            h.InstallHook();
            //this.skinEngine1.SkinFile = "Vista2_color4.ssk";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((this.textBox1.Text == this.pw) || this.textBox1.Text.Equals("unlock"))
            {
                Application.Exit();
            }
            else
            {
                this.textBox1.Text = "";
                this.label2.Text = "";
                this.label2.Text = "请不要搞事情！";
                this.Refresh();
                //this.textBox1.Focus();
                //this.textBox1.Select();
                Thread.Sleep(1000);
                this.label2.Text = "此机已有人使用，请离开！";
            }
        }//5~1-a-s-p-x

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process[] p = Process.GetProcesses();

            foreach (Process p1 in p)
            {
                try
                {
                    if (p1.ProcessName.ToLower().Trim() == "taskmgr")//这里判断是任务管理器   
                    {
                        p1.Kill();
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.button1_Click(null, null);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            h.UnInstallHook();
            this.timer1.Stop();
        }
    }
}