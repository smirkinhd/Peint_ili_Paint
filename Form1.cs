using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace graficredactr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            this.Text = "Графический редактор";
            this.Width = 900;
            this.Height = 700;
            bm = new Bitmap(pic.Width, pic.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            pic.Image = bm;
            pic_clr.BackColor = Color.Black;
        }
        Bitmap bm;
        Graphics g;
        bool paint = false;
        Point px, py;
        Pen erase = new Pen(Color.White, 100);
        Pen pencil = new Pen(Color.Black, 1);
        int index;
        int x, y, sX, sY, cX, cY;

        ColorDialog cd = new ColorDialog();
        Color new_clr;


        private void button4_Click(object sender, EventArgs e)
        {
            index = 4;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            index = 5;
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (paint)
            {
                if (index == 3)
                {
                    g.DrawEllipse(pencil, cX, cY, sX, sY);
                }

                if (index == 4)
                {
                    g.DrawRectangle(pencil, cX, cY, sX, sY);
                }

                if (index == 5)
                {
                    g.DrawLine(pencil, cX, cY, x, y);
                }
            }

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void новоеОкноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f2 = new Form1();
            f2.Show();
        }

        private void cоlоr_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();
            new_clr = cd.Color;
            pic_clr.BackColor = cd.Color;
            pencil.Color = cd.Color;
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);

            pic.Image = bm;
            index = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            index = 3;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            index = 1;
        }

        static Point set_point(PictureBox pb, Point pt)
        {
            float py = 1f * pb.Image.Height / pb.Height;
            float pX = 1f * pb.Image.Width / pb.Width;

            return new Point((int)(pt.X * pX), (int)(pt.Y * py));
        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (index ==7)
            {
                Point point = set_point(pic, e.Location);
                Fill(bm, point.X, point.Y, new_clr);
            }
        }

        private void fill_Click(object sender, EventArgs e)
        {
            index = 7;
        }

        public void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Жпег(*.jpg)|* .jpg|Бамп(*.bmp)|* .bmp";
            if (sfd.ShowDialog()==DialogResult.OK)
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, pic.Width, pic.Height), bm.PixelFormat);
                btm.Save(sfd.FileName);
                MessageBox.Show("Рисунок успешно сохранён!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                pencil.Width += 1;
            }
            catch (System.Exception Ситуация)
            {
                MessageBox.Show(Ситуация.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                pencil.Width -= 1;
            }
            catch (System.Exception Ситуация)
            {
                MessageBox.Show(Ситуация.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            pencil.Width = 1;
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult MBox = MessageBox.Show("Имеется незавершенная работа.\nСохранить изменения?",
            "Простой редактор", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            if (MBox == DialogResult.No) return;
            if (MBox == DialogResult.Cancel) e.Cancel = true;
            if (MBox == DialogResult.Yes)
            {
                сохранитьКакToolStripMenuItem_Click(sender, e);
            }
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                if (index == 1)
                {
                    px = e.Location;
                    g.DrawLine(pencil, px, py);
                    py = px;
                }
                if (index == 2)
                {
                    px = e.Location;
                    g.DrawLine(erase, px, py);
                    py = px;
                }
            }
            pic.Refresh();
            x = e.X;
            y = e.Y;
            sX = e.X - cX;
            sY = e.Y - cY;

        }


        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;

            sX = x - cX;
            sY = y - cY;

            if (index ==3)
            {
                g.DrawEllipse(pencil, cX, cY, sX, sY);
            }

            if (index == 4)
            {
                g.DrawRectangle(pencil, cX, cY, sX, sY);
            }

            if (index == 5)
            {
                g.DrawLine(pencil, cX, cY, x, y);
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            index = 2;
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            py = e.Location;
            cX = e.X;
            cY = e.Y;

        }


        private void validate(Bitmap bm, Stack<Point>sp, int x, int y, Color оld_cоlоr, Color new_cоlоr)
        {
            Color cx = new Color();
            cx = bm.GetPixel(x, y);
            if (cx == оld_cоlоr)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_cоlоr);
            }
        }

        public void Fill (Bitmap bm, int x, int y, Color new_clr)
        {
            Color оld_cоlоr = bm.GetPixel(x, y);
            Stack<Point>pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_clr);

            if (оld_cоlоr == new_clr) return;

            while(pixel.Count >0)
            {
                Point pt = (Point)pixel.Pop();
                if(pt.X>=0&&pt.Y>0 &&pt.X<bm.Width-1 && pt.Y<bm.Height-1)
                {
                    validate(bm, pixel, pt.X - 1, pt.Y, оld_cоlоr, new_clr);
                    validate(bm, pixel, pt.X, pt.Y-1, оld_cоlоr, new_clr);
                    validate(bm, pixel, pt.X+1, pt.Y, оld_cоlоr, new_clr);
                    validate(bm, pixel, pt.X, pt.Y + 1, оld_cоlоr, new_clr);
                }
            }
        }
    }
}
