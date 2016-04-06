using System;
using System.Windows.Forms;

namespace QzoneAutoLike
{
    public partial class UpdateForm : Form
    {
        public UpdateForm()
        {
            InitializeComponent();
        }

        private string fileUrl=null;

        private void button1_Click(object sender, EventArgs e)
        {
            string dotNet = "35";
            if (radioButton1.Checked) dotNet = "45";
            fileUrl = Program.githubUrl+ "blob/master/Binary/QzoneAutoLike" + dotNet + ".zip?raw=true";


            
        }





        private void UpdateForm_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = Program.dotNetVersion != "35";
            radioButton2.Checked = !radioButton1.Checked ;
        }
    }
}
