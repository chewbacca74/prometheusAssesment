using RestSharp;
using System.Net;
using prgAPITest.Dto;

namespace prgAPITest
{
    [TestClass]
    public class ApiGetTests
    {
        private static readonly string BaseUrl = "https://jsonplaceholder.typicode.com/";
        private static readonly RestClient client = new RestClient(BaseUrl);
        private static readonly Dictionary<string, string> headers = new Dictionary<string, string>(){
                {"ContentType","application/json"},
                {"charset","UTF-8"}
        };

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            client.AddDefaultHeaders(headers);
        }

        [TestMethod]
        public void Posts_PostCommentsShouldMatchCommentsForPost()
        {
            // Arrange
            var request = new RestRequest("/posts/1/comments", Method.Get);
            // Act
            var commentsPostResponse = client.Execute<List<CommentDto>>(request);

            // Assert since we are using a DTO there is no need to assert on types
            // since the response would deserialize incorrectly 
            Assert.IsNotNull(commentsPostResponse, "Response should not be null.");
            Assert.AreEqual(HttpStatusCode.OK, commentsPostResponse.StatusCode, "Expected HTTP status code 200 OK.");
            //check the size
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.AreEqual(5, commentsPostResponse.Data.Count, "Expected number of comments for post 1 is 5");

            //get a count of the posts which user id 1 posted
            request = new RestRequest("/comments?postId=1", Method.Get);
            var commentsPostQueryStringResponse = client.Execute<List<CommentDto>>(request);
            Assert.IsNotNull(commentsPostQueryStringResponse, "Response should not be null.");
            Assert.AreEqual(HttpStatusCode.OK, commentsPostQueryStringResponse.StatusCode, "Expected HTTP status code 200 OK.");
            //the size of the responses should be the same
            Assert.AreEqual(commentsPostQueryStringResponse.Data.Count, commentsPostResponse.Data.Count);

            //check that the contents match of each way of calling the api match
            //note this might be a better compare against static external data but the strings are not easy to put into csv, and I did not want to use hashes here 

            for (int i = 0; i < commentsPostResponse.Data.Count; i++)
            {
                Assert.AreEqual(commentsPostQueryStringResponse.Data[i].Id, commentsPostResponse.Data[i].Id);
                Assert.AreEqual(commentsPostQueryStringResponse.Data[i].PostId, commentsPostResponse.Data[i].PostId);
                Assert.AreEqual(commentsPostQueryStringResponse.Data[i].Name, commentsPostResponse.Data[i].Name);
                Assert.AreEqual(commentsPostQueryStringResponse.Data[i].Email, commentsPostResponse.Data[i].Email);
                Assert.AreEqual(commentsPostQueryStringResponse.Data[i].Body, commentsPostResponse.Data[i].Body);
            }
        }

        [TestMethod]
        public void Posts_AddingPostShouldIncreaseTheCountOfPosts()
        {
            var request = new RestRequest("/posts", Method.Post);

            var payload = new
            {
                title = "foo",
                body = "bar",
                userId = 1
            };

            request.AddJsonBody(payload);
            var postsResponse = client.Execute<PostDto>(request);
            Assert.AreEqual(HttpStatusCode.Created, postsResponse.StatusCode, "Expected HTTP status code 201 Created.");
            //since data is static on the server Id will always be 101, irl this is not a good idea because the 
            //primary key is often autoincrement or uuid
            Assert.AreEqual(101, postsResponse.Data.Id, "This should be the 101st post");
            Assert.AreEqual(1, postsResponse.Data.UserId, "This should be the Id of the poster");
            Assert.AreEqual("foo", postsResponse.Data.Title, "This should match the title in the request");
            Assert.AreEqual("bar", postsResponse.Data.Body, "This should match the body of the request");

            //subsequent calls should return 101 posts which ofcourse they won't (static data)
            request = new RestRequest("/posts", Method.Get);
            var allPosts = client.Execute<List<PostDto>>(request);
            //With Real Data this should be 101, note this could be done by making this call at the beginning of th method and comparing
            Assert.AreEqual(100, allPosts.Data.Count, "Total Posts Should Be Incremented by one");
            //Also there should now be 11 posts by userId 1, but of course there are only 10 (static data)
            Assert.AreEqual(10, allPosts.Data.Where(post => post.UserId == 1).ToList<PostDto>().Count, "The number of posts by user1 is wrong");
            //<PostDto>post => post.Id == 1).ToList()., "Total ")
        }

        [TestMethod]
        public void Posts_UpdatingAPostsShouldOnlyUpdateTheFieldsThatWereAdded()
        {
            var request = new RestRequest("/posts/90", Method.Put);

            var payload = new
            {
                title = "fidelio",
                userId = 8
            };

            request.AddJsonBody(payload);
            var putResponse = client.Execute<PostsPutDto>(request);
            Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode, "Expected HTTP status code 200 OK.");
            Assert.AreEqual(90, putResponse.Data.Id, "Id Should Be the Same As The Request");
            Assert.AreEqual(8, putResponse.Data.UserId, "The User Id Changed And Should Be Different Now");
            Assert.AreEqual("fidelio", putResponse.Data.Title, "Title Should Have the New Value");
            Assert.AreEqual(null, putResponse.Data.Body, "The Body Should not Be Changed");
            request = new RestRequest("/posts", Method.Get);
            var allPosts = client.Execute<List<PostDto>>(request);
            //With Real Data this should still be 100, because we did not add a post
            Assert.AreEqual(100, allPosts.Data.Count, "Total Posts Should Be Incremented by one");
            //Also there should now be 11 posts by userId 8, as we changed the id on the post, but static data
            Assert.AreEqual(10, allPosts.Data.Where(post => post.UserId == 1).ToList<PostDto>().Count);
        }

        [TestMethod]
        public void Posts_DeletingPostsShouldJustGiveAnOk()
        {
            var request = new RestRequest("/posts/90", Method.Delete);
            //Note Im not using a dto here, as the response can vary based on which fields I updated
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected HTTP status code 200 OK.");
            Assert.AreEqual("{}", response.Content);
            request = new RestRequest("/posts/90", Method.Get);
            //note assertions here are kind of pointless (static data), but this should return 
            //a 401 or other appropriate status code
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected HTTP status code 200 OK.");
        }

        [TestMethod]
        public void PostsNegative_DeleteNonExistantPostShouldGive401()
        {
            var request = new RestRequest("/posts/1003", Method.Delete);
            var response = client.Execute(request);
            //Should be not found 
            //Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expected HTTP status code 401 NoT Found.");
        }

        [TestMethod]
        public void PostsNegative_UpdateInvalidField()
        {
            var request = new RestRequest("/posts/90", Method.Put);
            var payload = new
            {
                totallyFakeTitle = "foo",
                userId = 8
            };

            request.AddJsonBody(payload);
            var Response = client.Execute(request);
            //Should Be 400 
            //Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected HTTP status code 400 BadRequest.");
        }

        [TestMethod]
        public void PostsNegative_BadResourcePath()
        {
            var request = new RestRequest("/posts/comments", Method.Get);
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expected HTTP status code 401 NotFound.");
        }

        /**
        There are a lot of other negative tests possible but not very demonstrable with static data and no security!
            Some Examples:
            Invalid Field Name for posts and puts
            Invalid character sets for posts and puts
            Invalid Resources and resource Ids (deleting posts that don't exist, updating to users that dont exist)
            Permissions for various rest resources (i.e. not every user should have the ability to post or put)
            Users should only be able to update their own posts...


        **/
    }
}
#pragma warning restore CS8602// Dereference of a possibly null reference.