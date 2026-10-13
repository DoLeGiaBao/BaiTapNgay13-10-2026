
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Bai4_7
{
    public partial class FormStudent : Form
    {
        private BindingList<StudentInfo> students =
            new BindingList<StudentInfo>();

        private BindingSource bindingSource =
            new BindingSource();

        // Gửi thông báo trạng thái về Form1
        public event Action<string> StatusMessageChanged;

        public FormStudent()
        {
            InitializeComponent();

            ConfigureGrid();
            ConfigureContextMenu();
            LoadSampleData();
        }

        private void ConfigureGrid()
        {
            Text = "Quản lý sinh viên";

            dgvStudents.Dock = DockStyle.Fill;
            dgvStudents.ReadOnly = true;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.MultiSelect = false;
            dgvStudents.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.AutoGenerateColumns = true;

            bindingSource.DataSource = students;
            dgvStudents.DataSource = bindingSource;

            dgvStudents.DataBindingComplete +=
                dgvStudents_DataBindingComplete;

            dgvStudents.SelectionChanged += delegate
            {
                StudentInfo student = GetSelectedStudent();

                if (student != null)
                {
                    SendStatus("Đang chọn: " + student.FullName);
                }
            };
        }

        private void dgvStudents_DataBindingComplete(
            object sender,
            DataGridViewBindingCompleteEventArgs e)
        {
            SetHeader("StudentId", "Mã sinh viên");
            SetHeader("FullName", "Họ và tên");
            SetHeader("Age", "Tuổi");
            SetHeader("Gender", "Giới tính");
        }

        private void SetHeader(string columnName, string header)
        {
            if (dgvStudents.Columns[columnName] != null)
            {
                dgvStudents.Columns[columnName].HeaderText = header;
            }
        }

        private void ConfigureContextMenu()
        {
            // Tạo menu chuột phải bằng code
            contextMenuStrip1.Items.Clear();

            contextMenuStrip1.Items.Add(
                "Xem thông tin", null, mnuViewInfo_Click);

            contextMenuStrip1.Items.Add(
                "Làm mới", null, mnuRefresh_Click);

            contextMenuStrip1.Items.Add(
                "Xóa sinh viên", null, mnuDelete_Click);

            dgvStudents.ContextMenuStrip = contextMenuStrip1;
        }

        private void LoadSampleData()
        {
            students.Add(new StudentInfo
            {
                StudentId = "SV001",
                FullName = "Nguyễn Văn An",
                Age = 20,
                Gender = "Nam"
            });

            students.Add(new StudentInfo
            {
                StudentId = "SV002",
                FullName = "Trần Thị Bình",
                Age = 21,
                Gender = "Nữ"
            });

            students.Add(new StudentInfo
            {
                StudentId = "SV003",
                FullName = "Lê Văn Cường",
                Age = 19,
                Gender = "Nam"
            });

            SendStatus("Đã tải danh sách sinh viên");
        }

        private StudentInfo GetSelectedStudent()
        {
            return bindingSource.Current as StudentInfo;
        }

        private void mnuViewInfo_Click(object sender, EventArgs e)
        {
            StudentInfo student = GetSelectedStudent();

            if (student == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên.");
                return;
            }

            MessageBox.Show(
                "Mã sinh viên: " + student.StudentId +
                "\nHọ và tên: " + student.FullName +
                "\nTuổi: " + student.Age +
                "\nGiới tính: " + student.Gender,
                "Thông tin sinh viên");

            SendStatus("Đã xem thông tin: " + student.FullName);
        }

        private void mnuRefresh_Click(object sender, EventArgs e)
        {
            bindingSource.ResetBindings(false);
            dgvStudents.Refresh();

            SendStatus("Đã làm mới danh sách sinh viên");
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            StudentInfo student = GetSelectedStudent();

            if (student == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa " + student.FullName + "?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string name = student.FullName;
                bindingSource.RemoveCurrent();

                SendStatus("Đã xóa sinh viên: " + name);
            }
        }

        private void SendStatus(string message)
        {
            StatusMessageChanged?.Invoke(message);
        }

        // Giữ hàm này nếu Designer đang đăng ký sự kiện FormStudent_Load
        private void FormStudent_Load(object sender, EventArgs e)
        {
        }
    }

    public class StudentInfo
    {
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}
