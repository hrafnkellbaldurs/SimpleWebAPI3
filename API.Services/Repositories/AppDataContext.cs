using API.Services.Entities;
using System.Data.Entity;


namespace API.Services.Repositories
{
    /// <summary>
    /// This class represents the connection to the database
    /// </summary>
    class AppDataContext : DbContext
    {
        /// <summary>
        /// This represents the Courses table
        /// </summary>
        public DbSet<Course> Courses { get; set; }

        /// <summary>
        /// This represents the CourseTemplates table
        /// </summary>
        public DbSet<CourseTemplate> CourseTemplates { get; set; }

        /// <summary>
        /// This represents the CourseRegistrations table
        /// </summary>
        public DbSet<CourseRegistration> CourseRegistrations { get; set; }

        /// <summary>
        /// This represents the Students table
        /// </summary>
        public DbSet<Student> Students { get; set; }

        /// <summary>
        /// This represents the waiting list for every course
        /// </summary>
        public DbSet<WaitingListEntry> WaitingListentries { get; set; }
    }
}
