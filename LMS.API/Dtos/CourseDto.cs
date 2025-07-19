namespace LMS.API.Dtos
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; }         // Course title
        public string Description { get; set; }   // Description
        public bool IsActive { get; set; }        // Whether it's active
        public DateTime CreatedAt { get; set; }   // When it was created
    }
}
