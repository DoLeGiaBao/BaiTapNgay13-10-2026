
using System;
using System.Windows.Forms;

namespace Bai4_7
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

            // Mở cửa sổ quản lý sinh viên
            menuOpenStudents.Click += menuOpenStudents_Click;
            btnOpenStudents.Click += menuOpenStudents_Click;

            // Thoát chương trình
            menuExit.Click += menuExit_Click;

            // Sắp xếp cửa sổ con
            menuCascade.Click += delegate
            {
                LayoutMdi(MdiLayout.Cascade);
            };

            menuTileHorizontal.Click += delegate
            {
                LayoutMdi(MdiLayout.TileHorizontal);
            };

            menuTileVertical.Click += delegate
            {
                LayoutMdi(MdiLayout.TileVertical);
            };
        }

        private void menuOpenStudents_Click(object sender, EventArgs e)
        {
            // Nếu FormStudent đã mở thì kích hoạt lại
            if (studentForm != null && !studentForm.IsDisposed)
            {
                if (studentForm.WindowState == FormWindowState.Minimized)
                {
                    studentForm.WindowState = FormWindowState.Normal;
                }

                studentForm.Activate();
                return;
            }

            // Tạo cửa sổ con
            studentForm = new FormStudent();
            studentForm.MdiParent = this;

            // Nhận thông báo từ FormStudent
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
            lblStatus.Text = "Đã mở cửa sổ quản lý sinh viên";
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Giữ hàm này nếu Designer đang đăng ký sự kiện Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
