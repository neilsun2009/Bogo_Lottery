using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Number
{
    public partial class Form1 : Form
    {
        // state: 0 - pending, 1 - rolling
        private int state = 0;
        private int rollingNumber = 0;
        System.Threading.Timer timerThr;
        private delegate void SetTBMethodInvoke(object state);

        int min, max, exLength;
        String[] ex;
        int[] exInt;

        public Form1()
        {
            InitializeComponent();
            timerThr = new System.Threading.Timer(new TimerCallback(SetTB), null, Timeout.Infinite, 500);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (state == 0)
            {
                min = (int)numericUpDown1.Value;
                max = (int)numericUpDown2.Value;
                if (min > max)
                {
                    MessageBox.Show("最小号码大于最大号吗！\n请重新填写！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ex = textBox1.Text.Split(',');
                exLength = ex.Length;
                exInt = new int[exLength];
                for (int i = 0; i < exLength; ++i)
                {
                    bool success = Int32.TryParse(ex[i], out exInt[i]);
                    if (!success)
                    {
                        if (ex[i].Trim().Equals("")) {
                            continue;
                        }
                        MessageBox.Show("去除号码出现非法字符“" + ex[i] + "”，请输入数字！\n本次摇号将忽略之！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        exInt[i] = -1;
                    }
                }
                timerThr.Change(0, 10);
                button1.Text = "摇号中";
                state = 1;
                panel2.Enabled = false;
            }
            else {
                state = 0;
                button1.Text = "开始摇号";
                timerThr.Change(Timeout.Infinite, 10);
                // Thread.Sleep(1000);
                int x = getRandom();
                Console.WriteLine("chose " + x);
                label4.Text = getString(x);
                panel2.Enabled = true;
            }
            
            
        }

        private int getRealRandom(int min, int max) {
            // Console.WriteLine(min + "-" + max);
            return (int)Math.Floor(new Random().NextDouble() * (max - min + 1)) + min;
        }

        private int getRandom() {
            int re;
            while (true) {
                re = getRealRandom(min, max);
                bool cont = false;
                for (int i = 0; i < exLength; ++i) {
                    if (re == exInt[i])
                    {
                        cont = true;
                        break;
                    }
                }
                if (!cont) {
                    break;
                }
            }
            return re;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.bogobogo.cn");
        }

        private string getString(int x) {
            string re = x.ToString();
            if (x < 10)
            {
                re = "00" + re;
            }
            else if (x < 100) {
                re = "0" + re;
            }
            return re;
        }

        public void SetTB(object value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetTBMethodInvoke(SetTB), value);
            }
            else
            {
                rollingNumber = (int)Math.Floor(new Random().NextDouble() * 1000.0);
                if (state == 1)
                {
                    label4.Text = getString(rollingNumber);
                }
            }
        }
    }
}
