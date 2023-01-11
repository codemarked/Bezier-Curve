namespace Grafica1
{
    public partial class Form1 : Form
    {
        public static Main i;
        public Form1()
        {
            InitializeComponent();
            i = new Main(pictureBox1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs args = (MouseEventArgs) e;
            System.Drawing.Point location = args.Location;
            Point p = new Point(i.DX(location.X), i.DY(location.Y), Color.White, 14);
            i.points.Add(p);
            listBox1.Items.Add(p.getName() + "(" + p.X + "," + p.Y + ")");
            i.DrawPoint(p);
            i.DrawCoords(p);
            i.RefreshGraph();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            i.Run();
            trackBar1.Value = (int)(trackBar1.Maximum * Main.time);
            textBox1.Text = $"t = {Main.time}";
            i.RefreshGraph();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Main.time = (float) trackBar1.Value / trackBar1.Maximum;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            i.Apply();
            trackBar1.Value = (int)(trackBar1.Maximum * Main.time);
            textBox1.Text = $"t = {Main.time}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            i.ClearAll();
            trackBar1.Value = (int)(trackBar1.Maximum * Main.time);
            textBox1.Text = $"t = {Main.time}";
            listBox1.Items.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Main.isDrawingDebugLines = checkBox1.Checked;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Main.time = 0.0f;
            i.Apply();
            trackBar1.Value = (int)(trackBar1.Maximum * Main.time);
            textBox1.Text = $"t = {Main.time}";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Main.isDrawingLowerOrderLines = checkBox2.Checked;
        }
    }
}