﻿using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using API.Models;
using API.Models.Courses.Students;
using API.Services;
using System;
using System.Web.Http.Description;
using API.Services.Exceptions;

namespace SimpleWebAPI3.Controllers
{
    /// <summary>
    /// This is the Courses Controller
    /// </summary>
    [RoutePrefix("api/courses")]
    public class CoursesController : ApiController
    {
        //This is a reference to the businesslogic service that handles all the logic
        private readonly CoursesServiceProvider _service;

        /// <summary>
        /// This is the main course method
        /// </summary>
        public CoursesController()
        {
            _service = new CoursesServiceProvider();
        }

        /// <summary>
        /// Gets courses currently available
        /// </summary>
        /// <returns>A list of course objects</returns>
        [HttpGet]
        [Route("")]
        public List<CourseDTO> GetCourses(string semester = null)
        {
            return _service.GetCoursesBySemester(semester);
        }

        /// <summary>
        /// Returns a single course with the given id, containing a student list
        /// If the course doesn't exist exception is thrown and 404 is returned
        /// </summary>
        /// <param name="id"> The id of the course </param>
        /// <returns> The course object </returns>
        [HttpGet]
        [Route("{id:int}", Name = "GetCourse")]
        public CourseDetailsDTO GetCourse(int id)
        {
            try
            {
                return _service.GetSingleCourse(id);
            }
            catch (AppObjectNotFoundException e)
            {
                //return 404
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Gets a list of students in a particular course
        /// If the course doesn't exist an exception if thrown and 404 is returned
        /// </summary>
        /// <param name="id">ID of the course</param>
        /// <returns>A List of Students in the given course</returns>
        [HttpGet]
        [Route("{id:int}/students")]
        public List<StudentDTO> GetStudentsInCourse(int id)
        {
            try
            {
                return _service.GetStudentsInCourse(id);
            }
            catch (AppObjectNotFoundException e)
            {
                //return 404
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Updates the start and end dates of a pre existing course and returns 201
        /// If the course doesn't exist an exception is thrown and 404 is returned
        /// </summary>
        /// <param name="id">ID of the course to be updated<param>
        /// <param name="model">UpdateCourseViewModel object to be updated with</param>
        /// <returns>A 201 status code if successful, but 404 if not</returns>
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(CourseDTO))]
        public IHttpActionResult UpdateCourse(int id, UpdateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _service.UpdateCourse(id, model);
                    return Content(HttpStatusCode.Created, result);
                }
                catch (AppObjectNotFoundException e)
                {
                    //return 404
                    return NotFound();
                }
                catch (AppServerErrorException e)
                {
                    //return 500
                    return InternalServerError();
                }
            }
            else
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }
            
        }

        /// <summary>
        /// Removes the course with the given ID from the database
        /// </summary>
        /// <param name="id">ID of Course to remove</param>
        /// <returns>If succeeded: HTTP status code 204, else HTTP status code 404</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteCourse(int id)
        {
            try
            {
                _service.DeleteCourse(id);
            }
            catch (AppObjectNotFoundException e)
            {
                //return 404
                return NotFound();
            }
            //return 204 if deletion was successful
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a course to the database and returns 201 if successful
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(CourseDTO))]
        public IHttpActionResult AddCourse(AddCourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _service.AddCourse(course);
                    var location = Url.Link("GetCourse", new { id = result.ID });
                    return Created(location, result);

                }
                catch (AppObjectNotFoundException)
                {
                    return NotFound();
                }
                catch (AppBadRequestException)
                {
                    return BadRequest();
                }
            }
            else
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }
            
        }


        /// <summary>
        /// Adds the given Student to the Course with the given ID and returns 201 status code
        /// If the course or student do not previously exist in the database, 404 is returned
        /// If the student is already enrolled in the course, status code 409 is returned
        /// </summary>
        /// <param name="id">ID of Course to add the Student to</param>
        /// <param name="student">AddStudentViewModel object</param>
        /// <returns>If successful 201 statuscode, else HTTP status code 404</returns>
        [HttpPost]
        [Route("{id:int}/students")]
        [ResponseType(typeof(StudentDTO))]
        public IHttpActionResult AddStudentToCourse(int id, AddStudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _service.AddStudentToCourse(id, student);
                    return Content(HttpStatusCode.Created, result);
                }
                catch (AppObjectNotFoundException e)
                {
                    //return 404
                    return NotFound();
                }
                catch (AppConflictException e)
                {
                    //return 409
                    return Conflict();
                }
                
            }
            else
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }
        }
    }
}
