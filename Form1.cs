using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection.Metadata;

namespace AAE2023_P22083_M1
{
    public partial class Form1 : Form
    {
        private Pen p, eraser;
        private Graphics g, CursorGraphics, g2;
        private Bitmap bitmap, CursorBmp, bitmap2;
        private bool erase, draw, drawn;
        private int choice, circleSize, radius, centerX, centerY, length, x, y, historyIndex;
        private Point start, end;
        private PointF[] shape;
        private Point[] star;
        private string userInput, appInfo;
        private double angle, xd, yd, wavelength, amplitude, frequency, phase;
        private List<Bitmap> drawingHistory;

        public Form1()
        {
            InitializeComponent();
            p = new Pen(Color.Black, 3);
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bitmap);
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Parent = pictureBox1;
            bitmap2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            g2 = Graphics.FromImage(bitmap2);
            eraser = new Pen(pictureBox1.BackColor, 3);
            pictureBox1.Image = bitmap;
            pictureBox2.Image = bitmap2;
            drawingHistory = new List<Bitmap>();
            historyIndex = -1;
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            start = e.Location;
            end = e.Location;
            draw = true;
            drawn = false;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
            drawn = true;
            end = e.Location;
            if (choice == 11)
            {
                g2.Clear(Color.Transparent);
                pictureBox2.Refresh();
                Font textFont = new Font("Arial", 16);
                Brush textBrush = new SolidBrush(p.Color);
                g.DrawString(userInput, textFont, textBrush, e.X, e.Y);
                pictureBox1.Refresh();
            }
            else if (choice != 0)
            {
                freeze_controls();
            }

        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (erase)
            {
                if (draw)
                {
                    end = e.Location;
                    g.DrawLine(eraser, start, end);
                    start = end;
                    pictureBox1.Refresh();
                }
            }
            if (draw)
            {
                g2.Clear(Color.Transparent);
                end = e.Location;
                redraw_shape(choice, start, end);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // rectangle
            choice = 1;
            erase = false;
            drawn = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // circle
            choice = 2;
            erase = false;
            drawn = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Triangle
            choice = 3;
            erase = false;
            drawn = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Diamond
            choice = 4;
            erase = false;
            drawn = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // flowerpot (happy accident)
            choice = 5;
            erase = false;
            drawn = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Semicircle
            choice = 6;
            erase = false;
            drawn = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Decagon
            choice = 7;
            erase = false;
            drawn = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Cross
            choice = 8;
            erase = false;
            drawn = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // star
            choice = 9;
            erase = false;
            drawn = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // Wave
            choice = 10;
            erase = false;
            drawn = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // eraser
            erase = true;
            choice = 0;
            drawn = false;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            // canvas color
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = colorDialog1.Color;
                pictureBox2.BackColor = Color.Transparent;
                eraser.Color = colorDialog1.Color;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // pen color
            if (colorDialog4.ShowDialog() == DialogResult.OK)
            {
                p.Color = colorDialog4.Color;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            p.Width = (float)numericUpDown1.Value;
            eraser.Width = (float)numericUpDown1.Value;
        }
        private void create_polygon(Graphics gf, Point start, Point stop, int sides)
        {
            float x0 = (start.X + stop.X) / 2;
            float y0 = (start.Y + stop.Y) / 2;
            shape = new PointF[sides];
            float angle = 360 / sides;
            float r = (float)Math.Sqrt(Math.Pow(start.X - stop.X, 2) + Math.Pow(start.Y - stop.Y, 2)) / 2;
            for (int a = 0; a < sides; a++)
            {
                shape[a] = new PointF(
                    x0 + r * (float)Math.Cos((angle * a) * Math.PI / 180f),
                    y0 + r * (float)Math.Sin((angle * a) * Math.PI / 180f));
            }
            gf.DrawPolygon(p, shape);

        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            circleSize = (int)numericUpDown1.Value;
            CursorBmp = new Bitmap(circleSize, circleSize);
            CursorGraphics = Graphics.FromImage(CursorBmp);
            {
                CursorGraphics.Clear(Color.Transparent);
                CursorGraphics.DrawEllipse(new Pen(Color.Black, 1), 0, 0, circleSize - 1, circleSize - 1);
                Cursor CustomCursor = new Cursor(CursorBmp.GetHicon());

                panel2.Cursor = CustomCursor;
            }
        }
        private void create_rectangle(Graphics gf, Point start, Point stop)
        {

            gf.DrawRectangle(p, start.X, start.Y, stop.X - start.X, stop.Y - start.Y);
        }
        private void create_triangle(Graphics gf, Point start, Point end)
        {

            gf.DrawLine(p, start.X, start.Y, end.X, end.Y);
            gf.DrawLine(p, start.X, start.Y, start.X - (end.X - start.X), end.Y);
            gf.DrawLine(p, end.X, end.Y, start.X - (end.X - start.X), end.Y);

        }

        private void button14_Click(object sender, EventArgs e)
        {
            choice = 11;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Bitmap Image|*.bmp|JPEG Image|*.jpg|PNG Image|*.png|";
            ImageFormat imageFormat = ImageFormat.Bmp;
            string fileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(saveFileDialog1.FileName);
                fileName = saveFileDialog1.FileName;
                switch (ext)
                {
                    case ".bmp":
                        imageFormat = ImageFormat.Bmp;
                        break;
                    case ".jpg":
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    case ".png":
                        imageFormat = ImageFormat.Png;
                        break;
                }
                try
                {
                    using (Bitmap pictureBoxContent = new Bitmap(pictureBox1.Width, pictureBox1.Height))
                    {
                        pictureBox1.DrawToBitmap(pictureBoxContent, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
                        pictureBoxContent.Save(fileName, imageFormat);
                    }

                    MessageBox.Show("File saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while saving the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;

                try
                {
                    bitmap = Bitmap.FromFile(fileName) as Bitmap;
                    g = Graphics.FromImage(bitmap);
                    pictureBox1.Image = bitmap;
                    pictureBox1.Refresh();
                    MessageBox.Show("File opened successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while opening the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void create_circle(Graphics gf, Point start, Point stop)
        {

            gf.DrawEllipse(p, start.X, start.Y, stop.X - start.X, stop.Y - start.Y);

        }
        private void create_diamond(Graphics gf, Point start, Point stop)
        {

            PointF[] points = new PointF[4];
            points[0] = new PointF((start.X + stop.X) / 2, start.Y);
            points[1] = new PointF(stop.X, (start.Y + stop.Y) / 2);
            points[2] = new PointF((start.X + stop.X) / 2, stop.Y);
            points[3] = new PointF(start.X, (start.Y + stop.Y) / 2);
            gf.DrawPolygon(p, points);

        }
        private void create_flowerpot(Graphics gf, Point start, Point stop)
        {


            gf.DrawBezier(p, start.X + (stop.X - start.X) / 2, start.Y + (stop.Y - start.Y) / 4, start.X + (stop.X - start.X) / 4, start.Y + (stop.Y - start.Y) / 2, start.X + (stop.X - start.X) / 2, start.Y + (stop.Y - start.Y) * 3 / 4, start.X + (stop.X - start.X) * 3 / 4, start.Y + (stop.Y - start.Y) / 2);
            gf.DrawBezier(p, start.X + (stop.X - start.X) / 2, start.Y + (stop.Y - start.Y) / 4, start.X + (stop.X - start.X) * 3 / 4, start.Y + (stop.Y - start.Y) / 2, start.X + (stop.X - start.X) / 2, start.Y + (stop.Y - start.Y) * 3 / 4, start.X + (stop.X - start.X) / 4, start.Y + (stop.Y - start.Y) / 2);
            gf.DrawLine(p, start.X + (stop.X - start.X) / 4, start.Y + (stop.Y - start.Y) / 2, start.X + (stop.X - start.X) / 2, start.Y + (stop.Y - start.Y) * 3 / 4);
            gf.DrawLine(p, start.X + (stop.X - start.X) / 2, start.Y + (stop.Y - start.Y) * 3 / 4, start.X + (stop.X - start.X) * 3 / 4, start.Y + (stop.Y - start.Y) / 2);

        }

        private void button15_Click(object sender, EventArgs e)
        {
            //clear button
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Refresh();
            choice = 0;
        }
        private void create_semicircle(Graphics gf, Point start, Point stop)
        {
            centerX = Math.Abs(start.X + stop.X) / 2;
            centerY = Math.Abs(start.Y + stop.Y) / 2;
            radius = Math.Abs(start.X - stop.X) / 2;


            int startAngle = 180;
            int sweepAngle = 180;
            if (radius > 0)
            {
                gf.DrawArc(p, centerX - radius, centerY - radius, 2 * radius, 2 * radius, startAngle, sweepAngle);
                gf.DrawLine(p, centerX - radius, centerY, centerX + radius, centerY);
            }
        }
        private void create_cross(Graphics gf, Point start, Point stop)
        {

            gf.DrawLine(p, start.X, (start.Y + stop.Y) / 2, stop.X, (start.Y + stop.Y) / 2);
            gf.DrawLine(p, (start.X + stop.X) / 2, start.Y, (start.X + stop.X) / 2, stop.Y);

        }
        private void create_star(Graphics gf, Point start, Point stop)
        {
            centerX = (start.X + stop.X) / 2;
            centerY = (start.Y + stop.Y) / 2;
            length = Math.Min(Math.Abs(stop.X - start.X), Math.Abs(stop.Y - start.Y));
            star = new Point[10];
            for (int i = 0; i < 10; i++)
            {
                angle = Math.PI / 5 * i;
                radius = i % 2 == 0 ? length / 2 : length / 4;
                x = (int)(centerX + radius * Math.Sin(angle));
                y = (int)(centerY - radius * Math.Cos(angle));
                star[i] = new Point(x, y);
            }
            gf.DrawPolygon(p, star);

        }
        private void create_wave(Graphics gf, Point start, Point stop)
        {
            wavelength = Math.Abs(stop.X - start.X) / 4.0;
            amplitude = Math.Abs(stop.Y - start.Y) / 2.0;
            frequency = 2 * Math.PI / wavelength;
            phase = 0.0;
            yd = 0.0;
            xd = start.X;
            while (xd < stop.X)
            {
                yd = amplitude * Math.Sin(frequency * (xd - start.X) + phase) + (start.Y + stop.Y) / 2.0;
                gf.DrawLine(p, (int)xd, (int)yd, (int)(xd + 1), (int)yd);
                xd += 1.0;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            userInput = textBox1.Text;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (drawn)
            {
                switch (e.KeyCode)
                {
                    case Keys.W:
                        g2.Clear(Color.Transparent);
                        start.Y -= 10;
                        end.Y -= 10;
                        redraw_shape(choice, start, end);
                        break;
                    case Keys.S:
                        g2.Clear(Color.Transparent);
                        start.Y += 10;
                        end.Y += 10;
                        redraw_shape(choice, start, end);
                        break;
                    case Keys.A:
                        g2.Clear(Color.Transparent);
                        start.X -= 10;
                        end.X -= 10;
                        redraw_shape(choice, start, end);
                        break;
                    case Keys.D:
                        g2.Clear(Color.Transparent);
                        start.X += 10;
                        end.X += 10;
                        redraw_shape(choice, start, end);
                        break;
                }
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            drawn = false;
            g.DrawImage(bitmap2, 0, 0);
            reset_controls();
        }
        private void redraw_shape(int c, Point start, Point stop)
        {
            switch (c)
            {
                case 1:
                    create_rectangle(g2, start, stop);
                    break;
                case 2:
                    create_circle(g2, start, stop);
                    break;
                case 3:
                    create_triangle(g2, start, stop);
                    break;
                case 4:
                    create_diamond(g2, start, stop);
                    break;
                case 5:
                    create_flowerpot(g2, start, stop);
                    break;
                case 6:
                    create_semicircle(g2, start, stop);
                    break;
                case 7:
                    create_polygon(g2, start, stop, 10);
                    break;
                case 8:
                    create_cross(g2, start, stop);
                    break;
                case 9:
                    create_star(g2, start, stop);
                    break;
                case 10:
                    create_wave(g2, start, stop);
                    break;
            }
            pictureBox2.Refresh();
            save_drawing();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            // bigger Y
            if (drawn)
            {
                g2.Clear(Color.Transparent);
                start.Y -= 5;
                end.Y += 5;
                redraw_shape(choice, start, end);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            // smaller X
            if (drawn)
            {
                g2.Clear(Color.Transparent);
                start.X += 5;
                end.X -= 5;
                redraw_shape(choice, start, end);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            // smaller Y
            if (drawn)
            {
                g2.Clear(Color.Transparent);
                start.Y += 5;
                end.Y -= 5;
                redraw_shape(choice, start, end);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            // bigger X
            if (drawn)
            {
                g2.Clear(Color.Transparent);
                start.X -= 5;
                end.X += 5;
                redraw_shape(choice, start, end);
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appInfo = "This is a drawing app created as the first exercise of year 2023, in Univeristy of Piraeus,\"and the subject Object Oriented Application Development by the student Christos Lazaridis with school Id P22083.";
            appInfo += "\n\nThe app has the following features:";
            appInfo += "\n\n1. Draw shapes such as rectangle, circle, triangle, diamond, flowerpot, semicircle, decagon, cross, star and wave.";
            appInfo += "\n\n2. Change the color of the pen and the canvas.";
            appInfo += "\n\n3. Change the size of the pen.";
            appInfo += "\n\n4. Erase parts of the drawing.";
            appInfo += "\n\n5. Write text on the canvas.";
            appInfo += "\n\n6. Save the drawing as a .bmp, .jpg or .png file.";
            appInfo += "\n\n7. Open a .bmp, .jpg or .png file.";
            appInfo += "\n\n8. Move the drawing on the canvas.";
            appInfo += "\n\n9. Resize the drawing on the canvas.";
            appInfo += "\n\n10. Clear the canvas.";
            appInfo += "\n\n11. Undo or cancel the last actions";
            MessageBox.Show(appInfo, "App Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void save_drawing()
        {
            Bitmap copyBitmap = new Bitmap(bitmap2);
            drawingHistory.Add(copyBitmap);
            historyIndex = drawingHistory.Count - 1;
        }
        private void Undo()
        {
            if (historyIndex > 0)
            {
                historyIndex--;
                Bitmap bmp = new Bitmap(drawingHistory[historyIndex]);
                g2.Clear(Color.Transparent);
                g2.DrawImage(bmp, 0, 0);
                pictureBox2.Refresh();
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            drawn = false;
            reset_controls();
        }
        private void DisableButtons(params Button[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Enabled = false;
                button.Visible = false;
            }
        }
        private void EnableButtons(params Button[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Enabled = true;
                button.Visible = true;
            }
        }
        private void reset_controls()
        {
            DisableButtons(button16, button17, button18, button19, button20, button21, button22);
            EnableButtons(button1, button2, button3, button4, button5, button6, button7, button8, button9, button10, button11, 
                          button12, button13, button14, button15);

            textBox1.Enabled = true;
            textBox1.Visible = true;
            numericUpDown1.Enabled = true;
            numericUpDown1.Visible = true;

            textBox2.Visible = false;
            choice = 0;

            g2.Clear(Color.Transparent);
            pictureBox2.Refresh();
            pictureBox1.Refresh();

            drawingHistory.Clear();
            historyIndex = -1;
        }
        private void freeze_controls()
        {
            EnableButtons(button16, button17, button18, button19, button20, button21, button22);
            DisableButtons(button1, button2, button3, button4, button5, button6, button7, button8, button9, button10, button11,
                           button12, button13, button14, button15);
            textBox1.Enabled = false;
            textBox1.Visible = false;
            numericUpDown1.Enabled = false;
            numericUpDown1.Visible = false;

            textBox2.Visible = true;
        }
    }

}