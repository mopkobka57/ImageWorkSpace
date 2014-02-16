using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageWorkSpace
{
    public partial class Form1 : Form
    {
        string LoadPath, SavePath;
        ImageFilter ImgFilter;
        ImageMatrix ImgMatrix;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.LoadPath = textBox1.Text;
            this.ImgFilter = new ImageFilter(this.LoadPath);
            this.ImgMatrix = new ImageMatrix(this.LoadPath);
            //try { pictureBox1.Image.Dispose(); }
            //catch { }
            pictureBox1.Image = new Bitmap(LoadPath);
            pictureBox2.Image = ImgFilter.Image;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.SavePath = textBox2.Text;
            this.ImgFilter.Save(this.SavePath);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //try { pictureBox2.Image.Dispose(); }
            //catch { }
            pictureBox2.Image = ImageFilter.Median(ImgFilter.Image);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ImgFilter.Blur();
            //try { pictureBox2.Image.Dispose(); }
            //catch { }
            textBox3.Text = ImgFilter.div.ToString();
            pictureBox2.Image = ImgFilter.Image;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ImgFilter.Negative();
            //try { pictureBox2.Image.Dispose(); }
            //catch { }
            textBox3.Text = ImgFilter.div.ToString();
            pictureBox2.Image = ImgFilter.Image;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ImgFilter.Clarity();
            textBox3.Text = ImgFilter.div.ToString();
            pictureBox2.Image = ImgFilter.Image;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = ImgMatrix.Gistogram();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = ImgFilter.Image = ImgMatrix.Shadows();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = ImgFilter.Image = ImgMatrix.Sobol();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = ImgMatrix.Previt();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Gauss.MakeGauss(ImgFilter.Image, Convert.ToInt32(Sigma.Text));
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = ImgFilter.Image = Gauss.SetScale(ImgFilter.Image, Convert.ToInt32(zoom.Text));
        }
    }
}
