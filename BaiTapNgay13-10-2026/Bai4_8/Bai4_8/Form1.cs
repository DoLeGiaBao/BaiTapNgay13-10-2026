
using System;
using System.Windows.Forms;

namespace Bai4_8
{
    public partial class Form1 : Form
    {
        private FormStudent studentForm;

        public Form1()
        {
            InitializeComponent();

            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;
            MainMenuStrip = menuStrip1;

            lblStatus.Text = "Sẵn sàng";

            menuOpenStudent.Click += menuOpenStudent_Click;
            btnOpenStudent.Click += menuOpenStudent_Click;
            menuExit.Click += menuExit_Click;
        }

        private void menuOpenStudent_Click(object sender, EventArgs e)
        {
            if (studentForm != null && !studentForm.IsDisposed)
            {
                if (studentForm.WindowState == FormWindowState.Minimized)
                {
                    studentForm.WindowState = FormWindowState.Normal;
                }

                studentForm.Activate();
                return;
            }

            studentForm = new FormStudent();
            studentForm.MdiParent = this;

            studentForm.StatusMessageChanged += delegate (string message)
            {
                lblStatus.Text = message;
            };

            studentForm.FormClosed += delegate
            {
                studentForm = null;
                lblStatus.Text = "Đã đóng cửa sổ sinh viên";
            };

            studentForm.Show();
            lblStatus.Text = "Đã mở cửa sổ tải dữ liệu";
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
