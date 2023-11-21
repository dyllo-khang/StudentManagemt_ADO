
using BusinessObject;
using BusinessObject.Models;
using ManagementService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagementGUI
{
    public partial class AddStudent : Form
    {
        private IStudentService _studentService;
        public event EventHandler<Student> DataAdded;
        private ManagementStudent _parent;
        List<Class> _classes;
        public AddStudent(ManagementStudent parent)
        {
            InitializeComponent();
            _parent = parent;
            _studentService = new StudentService();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string studentID = txtID.Text;
            var isExistID = _studentService.checkExistData("StudentID", studentID);
            Student student = null;
            if (isExistID)
            {
                MessageBox.Show("Duplicat StudentID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                student = new Student();
                string resultStudentID = ValidationHelper.ValidateID(studentID);
                if (resultStudentID != null)
                {
                    MessageBox.Show(resultStudentID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                student.StudentId = studentID;
                string resultFullName = ValidationHelper.ValidateFullName(txtFullName.Text.Trim());
                if (resultFullName != null)
                {
                    MessageBox.Show(resultFullName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                student.FullName = txtFullName.Text.Trim();
                if (!rdMale.Checked && !rdFemale.Checked)
                {
                    MessageBox.Show("Please check your gender", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (rdMale.Checked) student.Gender = "male";
                if (rdFemale.Checked) student.Gender = "female";
                if (string.IsNullOrWhiteSpace(richAdress.Text))
                {
                    MessageBox.Show("Address cannot blank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                student.Address = richAdress.Text.Trim();
                student.DateOfBirth = dtpBirthday.Value;
                string email = txtEmail.Text.Trim();
                var isExistEmail = _studentService.checkExistData("Email", email);
                if (isExistEmail)
                {
                    MessageBox.Show("Email existed!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string resultEmail = ValidationHelper.ValidateEmail(email);
                if (resultEmail != null)
                {
                    MessageBox.Show(resultEmail, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                student.Email = email;
                student.ClassId = cbClass.SelectedValue.ToString();
                DataAdded?.Invoke(this, student);
                this.Close();
            }
        }

        private void AddStudent_Load(object sender, EventArgs e)
        {
            _classes = _parent._classes;
            cbClass.DataSource = _classes;
            cbClass.DisplayMember = "ClassName";
            cbClass.ValueMember = "ClassID";
        }
    }
}
