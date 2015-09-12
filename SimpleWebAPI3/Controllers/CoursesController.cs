using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using API.Services;
using System.Web.Http.Description;
using API.Models.DTOs.Courses;
using API.Models.DTOs.Students;
using API.Models.ViewModels.Courses;
using API.Models.ViewModels.Students;
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
        [ResponseType(typeof(List<CourseDTO>))]
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
        [ResponseType(typeof(CourseDetailsDTO))]
        public CourseDetailsDTO GetCourse(int id)
        {
            try
            {
                return _service.GetSingleCourse(id);
            }
            catch (AppObjectNotFoundException)
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
        [ResponseType(typeof(List<StudentDTO>))]
        public List<StudentDTO> GetStudentsInCourse(int id)
        {
            try
            {
                return _service.GetStudentsInCourse(id);
            }
            catch (AppObjectNotFoundException)
            {
                //return 404
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Updates the start and end dates of a pre existing course and returns 201
        /// If the course doesn't exist an exception is thrown and 404 is returned
        /// </summary>
        /// <param name="id">ID of the course to be updated</param>
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
                catch (AppObjectNotFoundException)
                {
                    //return 404
                    return NotFound();
                }
                catch (AppServerErrorException)
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
        [ResponseType(typeof(HttpStatusCode))]
        public IHttpActionResult DeleteCourse(int id)
        {
            try
            {
                _service.DeleteCourse(id);
            }
            catch (AppObjectNotFoundException)
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
                catch (AppObjectNotFoundException)
                {
                    //return 404
                    return NotFound();
                }
                catch (AppPreconditionFailedException)
                {
                    //return 409
                    return StatusCode(HttpStatusCode.PreconditionFailed);
                }
                
            }
            else
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }
        }

        /// <summary>
        /// Gets the waitinglist for the course with the given ID,
        /// returns 404 if there is no course with the given ID.
        /// </summary>
        /// <param name="id">ID if the course to get the waitinglist from</param>
        /// <returns>A list of all students on the waitinglist</returns>
        [HttpGet]
        [Route("{id}/waitinglist")]
        [ResponseType(typeof(List<WaitingListDTO>))]
        public List<WaitingListDTO> GetWaitingListInACourse(int id)
        {
            try
            {
                return _service.GetWaitingListForCourse(id);
            }
            catch (AppObjectNotFoundException)
            {
                //return 404
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Adds student to waiting list in a course with the given id, returns 200 if successful.
        /// If student is already enrolled and active in course or already on waitinglist, 412 is returned to client.
        /// If course or student don't exist, a 404 is returned to client .
        /// </summary>
        /// <param name="id">ID if the course</param>
        /// <param name="student">AddStudentViewModel with students SSN</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/waitinglist")]
        [ResponseType(typeof(HttpStatusCode))]
        public IHttpActionResult AddStudentToWaitingList(int id, AddStudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.AddStudentToWaitingList(id, student);
                    return StatusCode(HttpStatusCode.OK);
                }
                catch (AppObjectNotFoundException)
                {
                    //return 404
                    return NotFound();
                }
                catch (AppConflictException)
                {
                    //return 409
                    return StatusCode(HttpStatusCode.PreconditionFailed);
                }

            }
            else
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }

        }

        /// <summary>
        /// Removes the student with the given SSN from the course with the given
        /// ID and returns 204 status code. If the student is not in the school
        /// or the course does not exist; it returns 404 status code
        /// </summary>
        /// <param name="id">The ID of the course</param>
        /// <param name="ssn">The SSN of the student</param>
        /// <returns>201 status code if successful, else 404</returns>
        [HttpDelete]
        [Route("{id:int}/students/{ssn}")]
        [ResponseType(typeof(HttpStatusCode))]
        public IHttpActionResult RemoveStudentFromCourse(int id, string ssn)
        {
            try
            {
                _service.RemoveStudentFromCourse(id, ssn);
                //return 204 if deletion was successful
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (AppObjectNotFoundException)
            {
                return NotFound();
            }
            catch (AppPreconditionFailedException)
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }
        }
    }
}
