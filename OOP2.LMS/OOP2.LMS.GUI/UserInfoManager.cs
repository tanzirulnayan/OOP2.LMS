using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using OOP2.LMS.BL;
using OOP2.LMS.Data;
using OOP2.LMS.Framework;

namespace OOP2.LMS.GUI
{
    public partial class UserInfoManager : MetroFramework.Forms.MetroForm
    {
        librarydbEntities _context = new librarydbEntities();
        UserInfoBL _userInfoBl = new UserInfoBL();
        List<UserInfo> _userInfos = new List<UserInfo>();
        private UserInfo _selectedUserInfo = null;
        private int _selectedIndex = 0;

        public UserInfoManager()
        {
            InitializeComponent();
        }

        private void UserInfoManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                ddlType.DataSource = EnumCollection.UserTypeList();
                ddlType.DisplayMember = "Name";
                ddlType.ValueMember = "ID";

                ddlStatus.DataSource = EnumCollection.UserStatusList();
                ddlStatus.DisplayMember = "Name";
                ddlStatus.ValueMember = "ID";

                ddlDepartment.DataSource = _context.Departments.ToList();
                ddlDepartment.DisplayMember = "Name";
                ddlDepartment.ValueMember = "ID";

                txtSearch.Text = "";

                this.LoadUserManagers();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void LoadUserManagers()
        {
            _userInfos = _userInfoBl.GetAll(txtSearch.Text);

            if (_userInfos.Count > 0)
            {
                _selectedUserInfo = _userInfos[0];
                _selectedIndex = 0;
            }
            else
            {
                _selectedUserInfo = new UserInfo()
                {
                    DOB = DateTime.Now
                };
            }

            this.Populate();
            this.RefreshDgv();
        }

        private void RefreshDgv()
        {
            dgvUserInfoList.AutoGenerateColumns = false;
            dgvUserInfoList.DataSource = _userInfos.ToList();
            dgvUserInfoList.Refresh();

            dgvUserInfoList.ClearSelection();

            for (int i = 0; i < dgvUserInfoList.Rows.Count; i++)
            {
                if (dgvUserInfoList.Rows[i].Cells[0].Value.ToString() == _selectedUserInfo.ID.ToString())
                {
                    dgvUserInfoList.Rows[i].Selected = true;
                    break;
                }
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedUserInfo.ID.ToString();
            txtFn.Text = _selectedUserInfo.FirstName;
            txtLn.Text = _selectedUserInfo.LastName;
            txtPassword.Text = _selectedUserInfo.Password;
            txtEmail.Text = _selectedUserInfo.Email;
            txtPhone.Text = _selectedUserInfo.Phone;
            txtUserID.Text = _selectedUserInfo.OrgID;
            ddlGender.Text = _selectedUserInfo.Gender;
            ddlType.SelectedValue = _selectedUserInfo.TypeID;
            ddlStatus.SelectedValue = _selectedUserInfo.StatusID;
            ddlDepartment.SelectedValue = _selectedUserInfo.DepartmentID;
            dtpDOB.Text = _selectedUserInfo.DOB.ToString();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadUserManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedUserInfo = _userInfos[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedUserInfo = new UserInfo()
            {
                DOB = DateTime.Now
            };
            this.Populate();
            dgvUserInfoList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedUserInfo.ID == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Please Select A Row First");
                return;
            }

            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure?", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
            {
                return;
            }

            string error;
            if (_userInfoBl.Delete(_selectedUserInfo.ID, out error) == false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _userInfos.Remove(_selectedUserInfo);
            this.RefreshDgv();
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed.");
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            try
            {
                _selectedUserInfo.FirstName= txtFn.Text;
                _selectedUserInfo.LastName= txtLn.Text;
                _selectedUserInfo.Password= txtPassword.Text;
                _selectedUserInfo.Email= txtEmail.Text;
                _selectedUserInfo.Phone= txtPhone.Text;
                _selectedUserInfo.OrgID= txtUserID.Text;
                _selectedUserInfo.Gender= ddlGender.Text;
                _selectedUserInfo.TypeID=Int32.Parse(ddlType.SelectedValue.ToString());
                _selectedUserInfo.StatusID = Int32.Parse(ddlStatus.SelectedValue.ToString());
                _selectedUserInfo.DepartmentID = Int32.Parse(ddlDepartment.SelectedValue.ToString());
                _selectedUserInfo.DOB= Convert.ToDateTime(dtpDOB.Text);

                bool isNew = _selectedUserInfo.ID == 0;

                string error;
                _selectedUserInfo = _userInfoBl.Save(_selectedUserInfo, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                    _userInfos.Add(_selectedUserInfo);
                else
                {
                    _userInfos[_selectedIndex] = _selectedUserInfo;
                }

                MetroFramework.MetroMessageBox.Show(this, "Operation Completed");
                RefreshDgv();
            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "Input is not correct");
            }
        }
    }
}
