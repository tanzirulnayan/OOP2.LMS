using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP2.LMS.Data;

namespace OOP2.LMS.BL
{
    public class UserInfoBL
    {
        private librarydbEntities _context = new librarydbEntities();
        public List<UserInfo> GetAll(string key="")
        {
            IEnumerable<UserInfo> query = _context.UserInfoes;

            if (!string.IsNullOrEmpty(key))
            {
                int id;
                if (Int32.TryParse(key, out id))
                {
                    query = query.Where(q => q.ID==id);
                }
                else
                {
                    query = query.Where(q => q.FirstName.Contains(key) ||
                                             q.LastName.Contains(key) ||
                                             q.Email.Contains(key));
                }
                
            }

            return query.ToList();
        }

        public UserInfo Save(UserInfo value, out string error)
        {
            error = string.Empty;
            try
            {
                var userInfo = _context.UserInfoes.FirstOrDefault(u => u.ID == value.ID);

                if (userInfo == null)
                {
                    userInfo = new UserInfo();
                    _context.UserInfoes.Add(userInfo);
                }

                userInfo.FirstName = value.FirstName;
                userInfo.LastName = value.LastName;
                userInfo.Password = value.Password;
                userInfo.Email = value.Email;
                userInfo.Phone = value.Phone;
                userInfo.OrgID = value.OrgID;
                userInfo.Gender = value.Gender;
                userInfo.TypeID = value.TypeID;
                userInfo.StatusID = value.StatusID;
                userInfo.DepartmentID = value.DepartmentID;
                userInfo.DOB = value.DOB;

                if (userInfo.OrgID == string.Empty)
                {
                    error = "Invalid Organization ID";
                    return value;
                }

                _context.SaveChanges();
                return userInfo;
            }
            catch (Exception e)
            {
                error = e.Message;
                return value;
            }
        }
        public bool Delete(int id,out string error)
        {
            error = string.Empty;

            var userInfo = _context.UserInfoes.FirstOrDefault(u => u.ID == id);

            if (userInfo == null)
            {
                error = "Invalid User ID";
                return false;
            }

            try
            {
                _context.UserInfoes.Remove(userInfo);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }

            return true;
        }
    }
}
