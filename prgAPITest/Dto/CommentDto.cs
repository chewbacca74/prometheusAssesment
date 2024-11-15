namespace prgAPITest.Dto
{
    public class CommentDto
    {
        public int PostId { get; set; }
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Body { get; set; }
    }
}