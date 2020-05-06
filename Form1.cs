using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Графический_редактор
{

    public partial class Form1 : Form
    {
        bool isMouseDown;

        Point oldLocation;

        List<GraphicsPath> paths = new List<GraphicsPath>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.BackColor = Color.Black;
        }

        

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            oldLocation = e.Location;
            isMouseDown = true;
        }

        public void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            

            if (!isMouseDown)
            {
                return;
            }

            paths.Add(new GraphicsPath());

            paths[paths.Count - 1].AddLine(oldLocation, e.Location);
            
            oldLocation = e.Location;

            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (GraphicsPath path in paths)
            {
                Graphics.FromImage(pictureBox1.Image).DrawPath(Pens.White, path);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        public void toolStripButton2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "BMP картинка(*.bmp)|*.bmp;";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                switch (saveFileDialog1.FilterIndex)
                {
                    case 0:
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
                        break;
                    case 1:
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Png);
                        break;
                    case 2:
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Icon);
                        break;
                    case 3:
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Gif);
                        break;
                    case 4:
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
                        break;
                    case 5:
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Tiff);
                        break;
                }
                Bitmap myImg = (Bitmap)Bitmap.FromFile(saveFileDialog1.FileName);

                StringBuilder sb = new StringBuilder();
                sb.Append("{" + Environment.NewLine);
                StringBuilder sbLine = new StringBuilder();
                for (int ii = 0; ii < myImg.Height; ii++)
                {
                    
                    for (int jj = 0; jj < myImg.Width; jj++)
                    {
                        
                        Color pixelColor = myImg.GetPixel(jj, ii);
                        sbLine.Append(HexConverter(pixelColor));
                    }
                   
                    byte[] buffer = GetBytes(sbLine.ToString());
                    
                    sb.Append("0x");
                    
                    sb.Append(BitConverter.ToString(buffer).Replace("-", ",0x"));
                    
                    sbLine.Clear();
                    buffer = null;
                    
                    sb.Append(",");
                   
                    sb.Append(Environment.NewLine);
                }
                

                sb.Append("};" + Environment.NewLine);
                textBox1.Text = sb.ToString();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        public byte[] GetBytes(string bitString)
        {
            return Enumerable.Range(0, bitString.Length / 8).
                Select(pos => Convert.ToByte(
                    bitString.Substring(pos * 8, 8),
                    2)
                ).ToArray();
        }

        private String HexConverter(System.Drawing.Color c)
        {
            if ((c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2")).Equals("000000"))
            {
               
                return "0";
            }
            else
            {
                
                return "1";
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

    }
}
