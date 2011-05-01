using FubuMVC.Core.View;
using FubuMVC.Core.View.WebForms;

namespace FubuMVC.HelloWorld.Controllers.IntegrationTests
{
    public class IntegrationTestsController
    {
        public RunTestsViewModel Run()
        {
            return new RunTestsViewModel();
        }
    }

    public class RunTestsViewModel {}

    public class RunView : FubuPage<RunTestsViewModel>
    {
    }

}