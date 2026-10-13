
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai4_8
{
    public partial class FormStudent : Form
    {
        public event Action<string> StatusMessageChanged;

        public FormStudent()
        {
            InitializeComponent();

            Text = "Tải dữ liệu sinh viên";

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;

            lblPercent.Text = "0%";
            lblMessage.Text = "Sẵn sàng";

            dgvStudents.ReadOnly = true;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.MultiSelect = false;
            dgvStudents.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.AutoGenerateColumns = true;

            btnLoadData.Click += btnLoadData_Click;
        }

        private async void btnLoadData_Click(object sender, EventArgs e)
        {
            btnLoadData.Enabled = false;
            progressBar1.Value = 0;
            lblPercent.Text = "0%";
            lblMessage.Text = "Đang tải dữ liệu...";

            SendStatus("Bắt đầu tải dữ liệu...");

            try
            {
                IProgress<int> progress = new Progress<int>(value =>
                {
                    int percent = Math.Max(0, Math.Min(100, value));

                    progressBar1.Value = percent;
                    lblPercent.Text = percent + "%";
                    lblMessage.Text = "Đang tải dữ liệu: " + percent + "%";

                    SendStatus("Đang tải dữ liệu: " + percent + "%");
                });

                List<StudentInfo> students =
                    await LoadStudentsAsync(progress);

                dgvStudents.DataSource = students;

                SetHeader("StudentId", "Mã sinh viên");
                SetHeader("FullName", "Họ và tên");
                SetHeader("Age", "Tuổi");
                SetHeader("Gender", "Giới tính");

                progressBar1.Value = 100;
                lblPercent.Text = "100%";
                lblMessage.Text = "Tải dữ liệu thành công!";

                SendStatus("Tải thành công " + students.Count + " sinh viên.");
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Tải dữ liệu thất bại!";
                SendStatus("Lỗi tải dữ liệu.");

                MessageBox.Show(
                    "Không thể tải dữ liệu:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnLoadData.Enabled = true;
            }
        }

        private async Task<List<StudentInfo>> LoadStudentsAsync(
            IProgress<int> progress)
        {
            return await Task.Run(async () =>
            {
                for (int percent = 10; percent <= 100; percent += 10)
                {
                    await Task.Delay(250);
                    progress.Report(percent);
                }

                return new List<StudentInfo>
                {
                    new StudentInfo
                    {
                        StudentId = "SV001",
                        FullName = "Nguyễn Văn An",
                        Age = 20,
                        Gender = "Nam"
                    },
                    new StudentInfo
                    {
                        StudentId = "SV002",
                        FullName = "Trần Thị Bình",
                        Age = 21,
                        Gender = "Nữ"
                    },
                    new StudentInfo
                    {
                        StudentId = "SV003",
                        FullName = "Lê Văn Cường",
                        Age = 19,
                        Gender = "Nam"
                    },
                    new StudentInfo
                    {
                        StudentId = "SV004",
                        FullName = "Phạm Minh Đức",
                        Age = 22,
                        Gender = "Nam"
                    },
                    new StudentInfo
                    {
                        StudentId = "SV005",
                        FullName = "Hoàng Thu Hà",
                        Age = 20,
                        Gender = "Nữ"
                    }
                };
            });
        }

        private void SetHeader(string columnName, string header)
        {
            if (dgvStudents.Columns[columnName] != null)
            {
                dgvStudents.Columns[columnName].HeaderText = header;
            }
        }

        private void SendStatus(string message)
        {
            StatusMessageChanged?.Invoke(message);
        }

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
