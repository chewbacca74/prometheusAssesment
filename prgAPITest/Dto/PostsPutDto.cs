// Post Dto gives me free type checking on fields I mark required, in this case I 
// Do a remove the required because they may not all be updated on a put
// But still keep me from having to use hamcrest or jsonpath matchers
namespace prgAPITest.Dto
{

    public class PostsPutDto
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}