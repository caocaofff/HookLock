using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HookLock
{
    public partial class Login : Form
    {
        public Login()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            InitializeComponent();

        }

        private Point loc = Point.Empty;
        private Form1 f1 = new Form1();

        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            dllName = dllName.Replace(".", "_");

            if (dllName.EndsWith("_resources")) return null;

            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            byte[] bytes = (byte[])rm.GetObject(dllName);

            return System.Reflection.Assembly.Load(bytes);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.passwork.Text.Equals("") && this.passwork2.Text.Equals(""))
            {
                this.toolTip1.Show("密码不能为空！", this.passwork, 600);
            }
            else if (this.passwork.Text != this.passwork2.Text)
            {
                this.toolTip1.Show("两次设置的密码不一样。", this.passwork, 600);
                
            }
            else
            {
                base.Visible = false;
                if (this.cbbox.SelectedItem.ToString().Equals("100%"))
                {
                    this.f1.getinfo(this.passwork.Text, 10.0);
                }
                else
                {
                    this.f1.getinfo(this.passwork.Text, double.Parse(this.cbbox.SelectedItem.ToString().Remove(1)));
                }
                this.f1.Show();
                this.Hide();
            }
        }//5^1^a^s^p^x

        private void passwork2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.btnOK_Click(null, null);
            }
        }
        

        private void Login_Load(object sender, EventArgs e)
        {
            this.cbbox.SelectedItem = "100%";
            //this.skinEngine1.SkinFile = "PageColor2.ssk";
        }

        private void Login_Activated(object sender, EventArgs e)
        {
            this.passwork.Focus();
        }
    }
}