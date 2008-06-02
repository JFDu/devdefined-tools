using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DevDefined.Common.Extensions.Annotations;


namespace DevDefined.Common.Tests.Extensions.Annotations
{
    [TestFixture]
    public class AnnotationTests
    {
        [SetUp]
        public void ForceCollection()
        {
            GC.Collect();
        }

        [Test]
        public void AnnotateClass()
        {
            ClassA target = new ClassA();
            target.Annotate(Description => "instance we are testing");
            Assert.AreEqual("instance we are testing", target.Annotation<string>("Description"));
        }

        [Test]
        public void AnnotateProperty()
        {
            ClassA target = new ClassA();
            target.Annotate(() => target.FirstName, Suffix => "Mr");
            Assert.AreEqual("Mr", target.Annotation<string>(() => target.FirstName, "Suffix"));
        }

        [Test]
        [ExpectedException(ExpectedMessage="The selected member does not belong to the declaring type \"DevDefined.Common.Tests.ClassB\"")]
        public void AnnotateNonOwnedProperty()
        {
            ClassA targetA = new ClassA();
            ClassB targetB = new ClassB();
            targetA.Annotate(() => targetB.FirstName, Suffix => "Mr");
            Assert.AreEqual("Mr", targetA.Annotation<string>(() => targetB.FirstName, "Suffix"));
        }

        [Test]
        public void AnnotateDifferentInstances()
        {
            ClassA target1 = new ClassA();
            ClassA target2 = new ClassA();

            target1.Annotate(Description => "class number 1");
            target2.Annotate(Description => "class number 2");

            Assert.AreEqual("class number 1", target1.Annotation<string>("Description"));
            Assert.AreEqual("class number 2", target2.Annotation<string>("Description"));
        }

        [Test]
        public void QueryStoreForClassAnnotationsWithCertainKey()
        {
            ClassA target1 = new ClassA();
            ClassA target2 = new ClassA();
            ClassA target3 = new ClassA();

            target1.Annotate(Description => "class number 1");
            target2.Annotate(Description => "class number 2");
            target3.Annotate(Parsed => true);

            var results = AnnotationStore.Classes
                .Where(a => a.HasKey("Description"))
                .ToList();

            Assert.AreEqual(2, results.Count);
        }

        [Test]
        public void QueryStoreForMemberAnnotations()
        {
            ClassA target1 = new ClassA();
            ClassA target2 = new ClassA();
            ClassA target3 = new ClassA();

            target1.Annotate(() => target1.FirstName, CamelCase => true); // annotating a property
            target1.Annotate(() => target1.Field, Ignored => true); // annotating a field
            target2.Annotate(() => target2.Execute(), Parsed => true); // annotating a method

            target3.Annotate(Parsed => true);

            var results = AnnotationStore.Members
                .Where(p => p.HasKey("CamelCase"))
                .ToList();

            Assert.AreEqual(1, results.Count);
        }

        [Test]
        public void AccessAnnotationDirectly()
        {
            ClassA target = new ClassA();

            target.Annotation()["Tags"] = new[] {"C#", "Tests"};

            CollectionAssert.AreEqual(target.Annotation<string[]>("Tags"), new[] { "C#", "Tests" });
        }
    }
}