using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core.Resources.Media;
using FubuMVC.Core.Urls;
using FubuTestingSupport;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.Resources.Media
{
    [TestFixture]
    public class ProjectionContextTester : InteractionContext<ProjectionContext<ProjectionContextTester.ProjectionModel>> 
    {
        [Test]
        public void subject_delegates_to_the_inner_values()
        {
            var model = new ProjectionModel();
            MockFor<IValues<ProjectionModel>>().Stub(x => x.Subject).Return(model);


            ClassUnderTest.Subject.ShouldBeTheSameAs(model);
        }

        [Test]
        public void formatted_value_of_extension_method_1()
        {
            var formatter = MockFor<IDisplayFormatter>();
            MockFor<IServiceLocator>().Stub(x => x.GetInstance<IDisplayFormatter>()).Return(formatter);

            var accessor = ReflectionHelper.GetAccessor<ProjectionModel>(x => x.Name);

            var theRawValue = "Jeremy";
            MockFor<IValues<ProjectionModel>>().Stub(x => x.ValueFor(accessor))
                .Return(theRawValue);

            var theFormattedValue = "*Jeremy*";
            formatter.Stub(x => x.GetDisplayForValue(accessor, theRawValue)).Return(theFormattedValue);

            ClassUnderTest.FormattedValueOf(accessor).ShouldEqual(theFormattedValue);
            ClassUnderTest.FormattedValueOf(x => x.Name).ShouldEqual(theFormattedValue);
        }

        [Test]
        public void value_for_delegates_to_the_inner_values()
        {
            var accessor = ReflectionHelper.GetAccessor<ProjectionModel>(x => x.Name);
            MockFor<IValues<ProjectionModel>>().Stub(x => x.ValueFor(accessor))
                .Return("Jeremy");

            ClassUnderTest.ValueFor(accessor).ShouldEqual("Jeremy");
        }

        [Test]
        public void getting_a_service_delegates_to_the_service_locator()
        {
            var stub = new StubUrlRegistry();
            MockFor<IServiceLocator>().Stub(x => x.GetInstance<IUrlRegistry>()).Return(stub);

            ClassUnderTest.Service<IUrlRegistry>().ShouldBeTheSameAs(stub);
        }

        [Test]
        public void urls_are_pulled_from_the_service_locator_but_only_once()
        {
            var stub = new StubUrlRegistry();
            MockFor<IServiceLocator>().Stub(x => x.GetInstance<IUrlRegistry>()).Return(stub);

            ClassUnderTest.Urls.ShouldBeTheSameAs(stub);
            ClassUnderTest.Urls.ShouldBeTheSameAs(stub);
            ClassUnderTest.Urls.ShouldBeTheSameAs(stub);
            ClassUnderTest.Urls.ShouldBeTheSameAs(stub);
            ClassUnderTest.Urls.ShouldBeTheSameAs(stub);
        
            MockFor<IServiceLocator>().AssertWasCalled(x => x.GetInstance<IUrlRegistry>(), x=> x.Repeat.Once());
        }

        [Test]
        public void display_formatter_is_pulled_from_the_service_locator_once()
        {
            var formatter = MockFor<IDisplayFormatter>();
            MockFor<IServiceLocator>().Stub(x => x.GetInstance<IDisplayFormatter>()).Return(formatter);

            ClassUnderTest.Formatter.ShouldBeTheSameAs(formatter);
            ClassUnderTest.Formatter.ShouldBeTheSameAs(formatter);
            ClassUnderTest.Formatter.ShouldBeTheSameAs(formatter);
            ClassUnderTest.Formatter.ShouldBeTheSameAs(formatter);
            ClassUnderTest.Formatter.ShouldBeTheSameAs(formatter);

            MockFor<IServiceLocator>().AssertWasCalled(x => x.GetInstance<IDisplayFormatter>(), x => x.Repeat.Once()); 
        }


        public class ProjectionModel
        {
            public string Name { get; set; }
        }
    }

    
}