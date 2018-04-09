using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class UpdateStudentInfoDTO
    {
        public int UserID { get; set; }

        public List<Student> UpdatedStudents { get; set; }

        public List<int> DeletedStudentIDs { get; set; }

        public List<Student> AddedStudents { get; set; }
    }
}
