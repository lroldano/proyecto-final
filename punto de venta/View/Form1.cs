using punto_de_venta.View;

namespace punto_de_venta
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 nuevoFormulario = new Form2();
            nuevoFormulario.Show();
            this.Hide(); // Oculta el formulario actual
        }
    }
}
