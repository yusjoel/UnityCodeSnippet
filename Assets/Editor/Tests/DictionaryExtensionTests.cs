using Gempoll.Plugins.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class DictionaryExtensionTests
    {
        [Test]
        public void Tests()
        {
            var aa = new ClassA();
            var bb = new ClassB();

            var nameObjectMap = new Dictionary<string, object>
            {
                {"Number", 190},
                {"Text", "Hello World!"},
                {"ClassA", aa},
                {"ClassB", bb}
            };

            var nameIntMap = new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2
            };

            var nameStringMap = new Dictionary<string, string>
            {
                ["one"] = "1",
                ["two"] = "2"
            };

            var number = nameObjectMap.SafelyGet<int>("Number");
            Assert.AreEqual(190, number);

            var text = nameObjectMap.SafelyGet<string>("Text");
            Assert.AreEqual("Hello World!", text);

            var object1 = nameObjectMap.SafelyGet("ClassA");
            Assert.AreEqual(aa, object1);

            var object2 = nameObjectMap.SafelyGet<object>("ClassB");
            Assert.AreEqual(bb, object2);

            var notFoundClass = nameObjectMap.SafelyGet<string>("Not found");
            Assert.AreEqual(null, notFoundClass);

            var notFoundStruct = nameObjectMap.SafelyGet<int>("Not found");
            Assert.AreEqual(0, notFoundStruct);

            Assert.Catch<InvalidCastException>(() =>
            {
                nameObjectMap.SafelyGet<int>("Text");
            });

            Assert.Catch<InvalidCastException>(() =>
            {
                nameObjectMap.SafelyGet<string>("Number");
            });

            var number1 = nameIntMap.SafelyGet("one");
            Assert.AreEqual(1, number1);
            var number2 = nameIntMap.SafelyGet("two");
            Assert.AreEqual(2, number2);
            var notFoundNumber = nameIntMap.SafelyGet("Not found");
            Assert.AreEqual(0, notFoundNumber);

            var text1 = nameStringMap.SafelyGet("one");
            Assert.AreEqual("1", text1);
            var text2 = nameStringMap.SafelyGet("two");
            Assert.AreEqual("2", text2);
            var notFoundText = nameStringMap.SafelyGet("Not found");
            Assert.AreEqual(null, notFoundText);
        }

        private class ClassA
        {
        }

        private class ClassB
        {
        }
    }
}
