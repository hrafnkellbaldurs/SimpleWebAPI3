using API.Models;
using API.Services.Repositories;
using System.Collections.Generic;
using System.Linq;
using API.Services.Entities;
using System;


namespace API.Services
{
    /// <summary>
    /// This class represent the businesslogic
    /// </summary>
    public class CoursesServiceProvider
    {
        //This is a reference to the database that stores the data
        private readonly AppDataContext _db;

        /// <summary>
        /// This method makes an instace of the database
        /// </summary>
        public CoursesServiceProvider()
        {
            _db = new AppDataContext();
        }

        /// <summary>
        /// Finds and returns a list of course objects, given a semester. 
        /// If no semester is provided, the "current" semester will be
        /// used.
        /// </summary>
        /// <param name="semester">The semester to get the courses from </param>
        /// <returns>A list of course objects being taught in a given semester</returns>
        public List<CourseDTO> GetCoursesBySemester(string semester = null)
        {
            if (string.IsNullOrEmpty(semester))
            {
                semester = "20153";
            }

            //A list of courses on the given semester
            var result = (from c in _db.Courses
                          join ct in _db.CourseTemplates
                            on c.TemplateID equals ct.TemplateID
                          where c.Semester == semester
                          select new CourseDTO
                          {
                              ID = c.ID,
                              StartDate = c.StartDate,
                              EndDate = c.EndDate,
                              Name = ct.Name,
                              StudentCount = (from cr in _db.CourseRegistrations
                                              where c.ID == cr.ID
                                              select cr.StudentID).ToList().Count
                          }).ToList();

            return result;
        }

        /// <summary>
        /// Finds the course with the given ID and returns it 
        /// with a list of all the students in that course
        /// </summary>
        /// <param name="id">The ID of the course to find</param>
        /// <returns>The course looked for with its student list, if the course doesn't exist, it returns 404</returns>
        public CourseDetailsDTO GetSingleCourse(int id)
        {
            //The course looked for with a list of all it's students
            var result = (from c in _db.Courses
                          join ct in _db.CourseTemplates
                          on c.TemplateID equals ct.TemplateID
                          where c.ID == id
                          select new CourseDetailsDTO
                          {
                              ID = c.ID,
                              StartDate = c.StartDate,
                              EndDate = c.EndDate,
                              Name = ct.Name,
                              Students = (from cr in _db.CourseRegistrations
                                          join s in _db.Students
                                          on cr.StudentID equals s.ID
                                          where cr.CourseID == id
                                          select new StudentDTO
                                          {
                                              Name = s.Name,
                                              SSN = s.SSN
                                          }).ToList()
                          }).Single();

            if (result == null)
            {
                throw new Exception();
            }

            return result;
        }

        /// <summary>
        /// Replaces the course with the given id's StartDate and EndDate with the given information
        /// </summary>
        /// <param name="id">The ID of the course to update</param>
        /// <param name="course">The course object to update with</param>
        public void UpdateCourse(int id, UpdateCourseViewModel course)
        {
            //Finds the course asked for
            var query = _db.Courses.First(x => x.ID == id);

            if (query == null)
            {
                throw new Exception();
            }
            query.StartDate = course.StartDate;
            query.EndDate = course.EndDate;

            _db.SaveChanges();
        }


        /// <summary>
        /// Deletes the course with the given ID, if the ID doesn't exist; an exception is thrown
        /// </summary>
        /// <param name="id">The ID of the course to delete</param>
        public void DeleteCourse(int id)
        {
            //Finds the course that is to be deleted
            var courseToDelete = _db.Courses.Where(x => x.ID == id).SingleOrDefault();

            if (courseToDelete == null)
            {
                throw new Exception();
            }

            // Remove the course from the database
            _db.Courses.Remove(courseToDelete);

            // Get a list of all the course registrations registered with the course id that we just deleted
            List<CourseRegistration> courseRegistrationsToDelete = _db.CourseRegistrations.Where(x => x.CourseID == id).ToList();

            // Delete each row containing the deleted course id from the CourseRegistrations 
            foreach (CourseRegistration cr in courseRegistrationsToDelete)
            {
                if (cr == null)
                {
                    throw new Exception();
                }
                _db.CourseRegistrations.Remove(cr);
            }

            _db.SaveChanges();
        }

        /// <summary>
        /// Returns a list of students in a given course, if course doesn't exist; throws exception
        /// </summary>
        /// <param name="id">The ID of the course to get students from</param>
        /// <returns>A list of students in the class with the given id</returns>
        public List<StudentDTO> GetStudentsInCourse(int id)
        {
            //Finds the appropriate course
            var courseExistance = _db.Courses.Where(x => x.ID == id).SingleOrDefault();

            if (courseExistance == null)
            {
                throw new Exception();
            }

            //Finds each student enrolled in that course
            var result = (from cr in _db.CourseRegistrations
                          join s in _db.Students
                          on cr.StudentID equals s.ID
                          where cr.CourseID == id
                          select new StudentDTO
                          {
                              Name = s.Name,
                              SSN = s.SSN
                          }).ToList();

            return result;
        }

        /// <summary>
        /// Adds a pre existing student to a pre existing course.
        /// </summary>
        /// <param name="id">The ID of the course to add the student to</param>
        /// <param name="student">A AddStudentViewModel containing the student that is to be added</param>
        public void AddStudentToCourse(int id, AddStudentViewModel student)
        {
            //Checking if student exists in the database
            var studentExistance = _db.Students.SingleOrDefault(x => x.SSN == student.SSN);

            if (studentExistance == null)
            {
                throw new ApplicationException();
            }

            //Checking if the course exists in the database
            var courseExistance = _db.Courses.SingleOrDefault(x => x.ID == id);

            if (courseExistance == null)
            {
                throw new ApplicationException();
            }

            //Checking if the student is already enrolled in the course
            var studentAlreadyInCourse = (from cr in _db.CourseRegistrations
                                          where cr.CourseID == id
                                          where studentExistance.ID == cr.StudentID
                                          select cr).SingleOrDefault();

            //If he is not enrolled, we enroll him in the course
            if (studentAlreadyInCourse == null)
            {
                CourseRegistration newRegistration = new CourseRegistration
                {
                    CourseID = id,
                    StudentID = studentExistance.ID
                };

                _db.CourseRegistrations.Add(newRegistration);
                _db.SaveChanges();
            }
            else
            {
                throw new AggregateException();
            }
        }
    }
}
