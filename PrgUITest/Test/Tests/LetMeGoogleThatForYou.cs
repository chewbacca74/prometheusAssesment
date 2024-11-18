using PrgUITest.Test.Common;
using PrgUITest.Test.Tests.Tasks.Fill;
using PrgUITest.Test.Tests.Tasks.Navigation;
using PrgUITest.Test.Tests.Tasks.Validate;
using Xunit;

namespace PrgUITest.Tests
{

    public class LetMeGoogleThatForYou : IDisposable
    {
        [Fact]
        public void VarifyThatContactUsFormCannotBeSubmittedWithoutAllRequiredFields()
        {
            // Navigate to Google
            Navigate.To().Google();
            FillGoogleSearch.With("Prometheus Group").AndSubmit();
            ValidateGoogleSearchResults.For().SearchResult();
            NavigateFromGoogleSearchResults.ToPrometheusGroup("Contact Us").ViaContactLink();
            FillPmgConactUs.With("Bob", "Smith").FillFirstLastAndSubmit();
            ValidateContactUsPage.For().RequiredFields();
        }

        public void Dispose()
        {
            Driver.QuitDriver();
        }
    }
}