using Core;
using Core.Interfaces.Engines;
using Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelnetProject.Tests.Engines.MockedAccessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines
{
    [TestClass]
    public class TestStudentEngine
    {
        MockStudentAccessor studentAccessor;
        IStudentEngine studentEngine;

        public List<Student> StudentsDB = new List<Student>()
        {
            new Student ()
            {
                StudentID = 1,
                UserID = 1,
                FirstName = "Joe",
                LastName = "Sheepman",
                Grade = 8
            },
            new Student ()
            {
                StudentID = 2,
                UserID = 1,
                FirstName = "Bill",
                LastName = "Billman",
                Grade = 11
            },
            new Student ()
            {
                StudentID = 3,
                UserID = 2,
                FirstName = "Jeff",
                LastName = "Snaikes",
                Grade = 2
            }
        };

        [TestInitialize]
        public void InitializeStudentEngineTests()
        {
            studentAccessor = new MockStudentAccessor(StudentsDB);
            studentEngine = new StudentEngine(studentAccessor);
        }

        [TestMethod]
        public void TestDeleteStudentInfo()
        {
            var toDelete = new List<int>();
            toDelete.AddRange(StudentsDB.Select(x => x.StudentID));
            studentEngine.DeleteStudentInfo(toDelete);
            Assert.AreEqual(0, studentAccessor.MockDb.Count);
        }

        [TestMethod]
        public void TestDeleteNoStudentInfo()
        {
            var expected = studentAccessor.MockDb.Count;
            studentEngine.DeleteStudentInfo(new List<int>());
            Assert.AreEqual(expected, studentAccessor.MockDb.Count);
        }

        [TestMethod]
        public void TestGetStudentInfoById()
        {
            var inserted = new Student()
            {
                UserID = 1,
                FirstName = "Test",
                LastName = "Test",
                Grade = 3
            };

            studentAccessor.InsertStudentInfo(1, inserted);

            Assert.AreEqual(inserted, studentEngine.GetStudentInfoByID(inserted.StudentID));
        }

        [TestMethod]
        public void TestGetNullStudentInfoById()
        {
            var id = studentAccessor.MockDb.Count > 0 ? studentAccessor.MockDb.Select(x => x.StudentID).Max() + 1 : 1;
            Assert.IsNull(studentEngine.GetStudentInfoByID(id));
        }

        [TestMethod]
        public void TestGetStudentInfoByNonexistantUserId()
        {
            var id = -1;
            Assert.AreEqual(0, studentEngine.GetStudentInfoByUserID(id).ToList().Count);
        }

        [TestMethod]
        public void TestGetStudentInfoByUserId()
        {
            var userId = 1;
            CollectionAssert.AreEqual(StudentsDB.Where(x => x.UserID == userId).ToList(), studentEngine.GetStudentInfoByUserID(userId).ToList());
        }

        [TestMethod]
        public void TestInsertStudentInfo()
        {
            var expected = new List<Student>()
            {
                new Student()
                {
                    UserID = 4,
                    FirstName = "Sean",
                    LastName = "fitz",
                    Grade = 2
                },
                new Student()
                {
                    UserID = 4,
                    FirstName = "mark",
                    LastName = "nail",
                    Grade = 12
                }
            };

            studentEngine.InsertStudentInfo(4, expected);

            CollectionAssert.IsSubsetOf(expected, studentAccessor.MockDb);
        }

        [TestMethod]
        public void TestInsertNoStudentInfo()
        {
            var expected = studentAccessor.MockDb;

            studentEngine.InsertStudentInfo(0, new List<Student>());

            CollectionAssert.AreEqual(expected, studentAccessor.MockDb);
        }

        [TestMethod]
        public void TestUpdateStudentInfo()
        {
            var students = new List<Student>()
            {
                new Student()
                {
                    UserID = 8,
                    FirstName = "hi",
                    LastName = "hello",
                    Grade = 3
                }
            };

            studentAccessor.InsertStudentInfo(8, students[0]);

            students[0].Grade = 8;

            studentEngine.UpdateStudentInfo(students);

            Assert.AreEqual(students[0], studentAccessor.MockDb.FirstOrDefault(x => x.StudentID == students[0].StudentID));
        }
    }
}
