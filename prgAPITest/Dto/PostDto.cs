namespace prgAPITest.Dto
{
    public class PostDto
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Body { get; set; }
    }
}