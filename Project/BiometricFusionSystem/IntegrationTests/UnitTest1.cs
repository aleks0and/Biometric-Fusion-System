using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;

namespace IntegrationTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Person p1 = new Person()
            {
                FirstName = "ABC",
                LastName = "XYZ",
                Id = 1,
                FaceFeatureVector = new System.Collections.Generic.List<double>(){ 1.1, 2.2, 3.3, 4.4, 5.5 },
                VoiceFeatureVector = new System.Collections.Generic.List<double>{ 6.6, 7.7, 8.8, 9.9, 9.9 }
            };

            DbConnection dc = new DbConnection();
            
            PersonRepository pr = new PersonRepository(dc);

            pr.AddPerson(p1);            

            
        }
    }
}
