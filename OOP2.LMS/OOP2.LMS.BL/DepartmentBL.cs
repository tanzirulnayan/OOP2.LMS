using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP2.LMS.Data;

namespace OOP2.LMS.BL
{
    public class DepartmentBL
    {
        private librarydbEntities _context = new librarydbEntities();
        public List<Department> GetAll(string key = "")
        {
            IEnumerable<Department> query = _context.Departments;

            if (!string.IsNullOrEmpty(key))
            {
                int id; 
                if (Int32.TryParse(key, out id))
                {
                    query = query.Where(q => q.ID == id);
                }
                else
                {
                    query = query.Where(q => q.Name.Contains(key));
                }

            }

            return query.ToList();
        }

        public Department Save(Department value, out string error)
        {
            error = string.Empty;
            try
            {
                var department = _context.Departments.FirstOrDefault(u => u.ID == value.ID);

                if (department == null)
                {
                    department = new Department();
                    _context.Departments.Add(department);
                }

                department.Name = value.Name;

                if (department.Name == string.Empty)
                {
                    error = "Invalid Department Name";
                    return value;
                }

                _context.SaveChanges();
                return department;
            }
            catch (Exception e)
            {
                error = e.Message;
                return value;
            }
        }
        public bool Delete(int id, out string error)
        {
            error = string.Empty;

            var department = _context.Departments.FirstOrDefault(u => u.ID == id);

            if (department == null)
            {
                error = "Invalid Department ID";
                return false;
            }

            try
            {
                _context.Departments.Remove(department);
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
