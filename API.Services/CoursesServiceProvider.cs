﻿using API.Models;
using API.Services.Repositories;
using System.Collections.Generic;
using System.Linq;
using API.Services.Entities;
using API.Models.Courses.Students;
using API.Services.Exceptions;


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
                              Semester = c.Semester,
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
                              Semester = c.Semester,
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
                // If the course is not found:
                throw new AppObjectNotFoundException();
            }

            return result;
        }

        /// <summary>
        /// Replaces the course with the given id's StartDate and EndDate with the given information
        /// </summary>
        /// <param name="id">The ID of the course to update</param>
        /// <param name="model">The course object to update with</param>
        public CourseDTO UpdateCourse(int id, UpdateCourseViewModel model)
        {
            //Finds the course asked for
            var course = _db.Courses.SingleOrDefault(x => x.ID == id);

            if (course == null)
            {
                // If the course is not found:
                throw new AppObjectNotFoundException();
            }
            course.StartDate = model.StartDate;
            course.EndDate = model.EndDate;

            _db.SaveChanges();

            var templateId = _db.CourseTemplates.SingleOrDefault(x => x.TemplateID == course.TemplateID);

            if (templateId == null)
            {
                throw new AppServerErrorException();
            }

            var studentCount = (from cr in _db.CourseRegistrations
                                where course.ID == cr.ID
                                select cr.StudentID).ToList().Count();

            var updatedCourse = new CourseDTO
            {
                ID = course.ID,
                EndDate = course.EndDate,
                StartDate = course.StartDate,
                Name = templateId.Name,
                Semester = course.Semester,
                StudentCount = studentCount
            };

            return updatedCourse;
        }


        /// <summary>
        /// Deletes the course with the given ID, if the ID doesn't exist; an exception is thrown
        /// </summary>
        /// <param name="id">The ID of the course to delete</param>
        public void DeleteCourse(int id)
        {
            //Finds the course that is to be deleted
            var courseToDelete = _db.Courses.SingleOrDefault(x => x.ID == id);

            if (courseToDelete == null)
            {
                // If course is not found:
                throw new AppObjectNotFoundException();
            }

            // Remove the course from the database
            _db.Courses.Remove(courseToDelete);

            // Get a list of all the course registrations registered with the course id that we just deleted
            var courseRegistrationsToDelete = _db.CourseRegistrations.Where(x => x.CourseID == id).ToList();

            // Delete each row containing the deleted course id from the CourseRegistrations 
            foreach (var cr in courseRegistrationsToDelete)
            {
                if (cr == null)
                {
                    // If a course registation is not found:
                    throw new AppObjectNotFoundException();
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
            // Finds the appropriate course
            var courseExistance = _db.Courses.SingleOrDefault(x => x.ID == id);

            if (courseExistance == null)
            {
                throw new AppObjectNotFoundException();
            }

            // Finds each student enrolled in that course
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
        /// <param name="model">A AddStudentViewModel containing the student that is to be added</param>
        public StudentDTO AddStudentToCourse(int id, AddStudentViewModel model)
        {
            // Checking if student exists in the database
            var student = _db.Students.SingleOrDefault(x => x.SSN == model.SSN);

            if (student == null)
            {
                // If the student cannot be found:
                throw new AppObjectNotFoundException();
            }

            // Checking if the course exists in the database
            var course = _db.Courses.SingleOrDefault(x => x.ID == id);

            if (course == null)
            {
                // If the course cannot be found:
                throw new AppObjectNotFoundException();
            }

            // Checking if the student is already enrolled in the course
            var studentAlreadyInCourse = (from cr in _db.CourseRegistrations
                                          where cr.CourseID == id
                                          where student.ID == cr.StudentID
                                          select cr).SingleOrDefault();

            // If he is not enrolled, we enroll him in the course
            if (studentAlreadyInCourse == null)
            {
                var newRegistration = new CourseRegistration
                {
                    CourseID = id,
                    StudentID = student.ID
                };

                _db.CourseRegistrations.Add(newRegistration);
                _db.SaveChanges();

                var studentDto = new StudentDTO
                {
                    Name = student.Name,
                    SSN = student.SSN
                };

                return studentDto;
            }
            // If he is already enrolled, there is a conflict
            else
            {
                throw new AppConflictException();
            }
        }
    }
}
