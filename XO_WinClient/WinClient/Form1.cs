using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinClient
{
    public partial class TickTackToeForm : Form
    {
        int[] board = new int[9];
        int znak;
        NetServer server;

        public TickTackToeForm()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            for (int i = 0; i < 9; i++)
            {
                board[i] = 0;
            }
            server = new NetServer();
            server.Connect();
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            int num = Convert.ToInt32(bt.Tag);
            if (board[num] == 0)
            {
                board[num] = znak;
            }
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            string s = "{\"nickname\":\"" + textBox1.Text + ",  \"action\":\"connect\"}";
            server.Send(s);

            PlayerList.DataSource = Converter.GetPlayerList(server.Receive());
        }
    }
}
