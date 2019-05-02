using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Music_Player
{
	public partial class Form1 : Form
	{
        int mySongLength = 2 * 60 + 42;
        int myCurrentSongTime = 0;

		public Form1()
		{
			InitializeComponent();
		}

        private void Form1_Load(object sender, EventArgs e)
        {
            myCurrentSongTime = 1 * 60 + 32;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            lblFocus.Focus();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            lblFocus.Focus();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            lblFocus.Focus();
        }

        private void barTimeline_Scroll(object sender, EventArgs e)
        {

        }
    }
}
