
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bai4_6
{
    public partial class Form1 : Form
    {
        private BindingList<Student> students =
            new BindingList<Student>();

        private BindingSource bindingSource =
            new BindingSource();

        public Form1()
        {
            InitializeComponent();

            ConfigureForm();
            ConfigureBinding();

            // Không đăng ký lại sự kiện Button ở đây,
            // vì các sự kiện được tạo trong Designer.
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void ConfigureForm()
        {
            this.Text = "Quản lý sinh viên - Bài 4.6";

            cboGender.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cboGender.Items.Clear();
            cboGender.Items.AddRange(new object[]
            {
                "Nam", "Nữ", "Khác"
            });

            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatar.BorderStyle = BorderStyle.FixedSingle;

            dgvStudents.ReadOnly = true;
            dgvStudents.MultiSelect = false;
            dgvStudents.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.AllowUserToAddRows = false;

            // Sử dụng cột tự sinh từ thuộc tính Student
            dgvStudents.AutoGenerateColumns = true;
            dgvStudents.Columns.Clear();
        }

        private void ConfigureBinding()
        {
            bindingSource.DataSource = students;
            dgvStudents.DataSource = bindingSource;

            // Không thêm DataBindings vào TextBox.
            // Khi bấm Lưu, ta lấy trực tiếp nội dung từ các ô
            // rồi cập nhật đối tượng Student.
            dgvStudents.DataBindingComplete +=
                dgvStudents_DataBindingComplete;
        }

        private void dgvStudents_DataBindingComplete(
            object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetHeader("StudentId", "Mã sinh viên");
            SetHeader("FullName", "Họ và tên");
            SetHeader("Age", "Tuổi");
            SetHeader("Gender", "Giới tính");

            if (dgvStudents.Columns["AvatarPath"] != null)
                dgvStudents.Columns["AvatarPath"].Visible = false;
        }

        private void SetHeader(string columnName, string header)
        {
            if (dgvStudents.Columns[columnName] != null)
                dgvStudents.Columns[columnName].HeaderText = header;
        }

        private Student CurrentStudent
        {
            get { return bindingSource.Current as Student; }
        }

        // BUTTON THÊM MỚI
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Student student = new Student();

            students.Add(student);
            bindingSource.Position = students.Count - 1;

            txtStudentId.Text = "";
            txtFullName.Text = "";
            txtAge.Text = "";
            cboGender.SelectedIndex = -1;

            DisposeAvatar();
            txtStudentId.Focus();
        }

        // BUTTON LƯU / CẬP NHẬT
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Student student = CurrentStudent;

            if (student == null)
            {
                MessageBox.Show("Vui lòng nhấn Thêm mới trước.");
                return;
            }

            string id = txtStudentId.Text.Trim();
            string name = txtFullName.Text.Trim();
            int age;

            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên.");
                txtStudentId.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập họ và tên.");
                txtFullName.Focus();
                return;
            }

            if (!int.TryParse(txtAge.Text.Trim(), out age) ||
                age < 1 || age > 120)
            {
                MessageBox.Show("Tuổi phải từ 1 đến 120.");
                txtAge.Focus();
                return;
            }

            if (cboGender.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn giới tính.");
                cboGender.Focus();
                return;
            }

            bool duplicateId = students.Any(s =>
                !object.ReferenceEquals(s, student) &&
                string.Equals(
                    (s.StudentId ?? "").Trim(),
                    id,
                    StringComparison.OrdinalIgnoreCase));

            if (duplicateId)
            {
                MessageBox.Show("Mã sinh viên đã tồn tại.");
                txtStudentId.Focus();
                return;
            }

            student.StudentId = id;
            student.FullName = name;
            student.Age = age;
            student.Gender = cboGender.SelectedItem.ToString();

            bindingSource.ResetBindings(false);
            dgvStudents.Refresh();

            MessageBox.Show("Lưu thông tin sinh viên thành công!");
        }

        // BUTTON XÓA
        private void btnDelete_Click(object sender, EventArgs e)
        {
            Student student = CurrentStudent;

            if (student == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa sinh viên " +
                student.FullName + "?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            bindingSource.RemoveCurrent();

            txtStudentId.Clear();
            txtFullName.Clear();
            txtAge.Clear();
            cboGender.SelectedIndex = -1;

            DisposeAvatar();
        }

        // BUTTON CHỌN ẢNH
        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            Student student = CurrentStudent;

            if (student == null)
            {
                MessageBox.Show("Vui lòng nhấn Thêm mới trước.");
                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Chọn ảnh đại diện";
                dialog.Filter =
                    "Tệp ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (Image source = Image.FromFile(dialog.FileName))
                    {
                        Image newImage = new Bitmap(source);
                        DisposeAvatar();
                        picAvatar.Image = newImage;
                    }

                    student.AvatarPath = dialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Không thể mở ảnh: " + ex.Message);
                }
            }
        }

        private void DisposeAvatar()
        {
            if (picAvatar.Image != null)
            {
                Image oldImage = picAvatar.Image;
                picAvatar.Image = null;
                oldImage.Dispose();
            }
        }

        // BUTTON XUẤT CSV
        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            var validStudents = students
                .Where(s =>
                    !string.IsNullOrWhiteSpace(s.StudentId) &&
                    !string.IsNullOrWhiteSpace(s.FullName))
                .ToList();

            if (validStudents.Count == 0)
            {
                MessageBox.Show(
                    "Chưa có dữ liệu sinh viên để xuất.");
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Xuất danh sách sinh viên";
                dialog.Filter = "CSV (*.csv)|*.csv";
                dialog.FileName = "DanhSachSinhVien.csv";
                dialog.DefaultExt = "csv";
                dialog.AddExtension = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (StreamWriter writer = new StreamWriter(
                        dialog.FileName, false, new UTF8Encoding(true)))
                    {
                        writer.WriteLine(
                            "MaSinhVien,HoTen,Tuoi,GioiTinh,DuongDanAnh");

                        foreach (Student s in validStudents)
                        {
                            string[] values =
                            {
                                s.StudentId,
                                s.FullName,
                                s.Age.ToString(),
                                s.Gender,
                                s.AvatarPath
                            };

                            writer.WriteLine(
                                string.Join(",", values.Select(EscapeCsv)));
                        }
                    }

                    MessageBox.Show(
                        "Xuất CSV thành công!\n" + dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Lỗi xuất CSV: " + ex.Message);
                }
            }
        }

        private static string EscapeCsv(string value)
        {
            if (value == null)
                value = "";

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
