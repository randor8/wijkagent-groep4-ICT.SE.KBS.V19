using System;
using System.Configuration;
using NUnit.Framework;
using WijkagentModels;
using WijkagentWPF;
using WijkagentWPF.database;

namespace WijkagentTests
{
    [TestFixture]
    public class OffenceControllerTest
    {
        /// <summary>
        /// used in the test cases for seting the offence
        /// </summary>
        public static DateTime dateTime = new DateTime().ToLocalTime();
        public static Location location = new Location(0, 1.1, 2.1);

        /*
        [Test]
        public void SetOffence_WithLocationAsID_ReturnsID()
        {
            OffenceController offenceController = new OffenceController();
            
            int returnedID = offenceController.SetOffence(dateTime, "unit test with id description", 1, OffenceCategories.Cybercrime);
            
            Assert.IsNotNull(offenceController.GetOffence(returnedID));
        }

        [Test]
        public void SetOffence_WithLocationAsObj_ReturnsID()
        {
            OffenceController offenceController = new OffenceController();

            int returnedID = offenceController.SetOffence(dateTime, "unit test with location description", location, OffenceCategories.Geweld);

            Assert.IsNotNull(offenceController.GetOffence(returnedID));
        }

        [Test]
        public void GetOffence_WithLocationAsObj_ReturnsOffence()
        {
            OffenceController offenceController = new OffenceController();

            int returnedID = offenceController.SetOffence(dateTime, "unit test for getOffence", location, OffenceCategories.Geweld);

            Assert.IsNotNull(offenceController.GetOffence(returnedID));
        }

        [Test]
        public void GetOffences_WithOutDBRows_NotNull()
        {
            OffenceController offenceController = new OffenceController();
            DBContext dBContext = new DBContext();

            //clear table
            dBContext.ExecuteQuery(new System.Data.SqlClient.SqlCommand("DELETE FROM offence"));
            
            Assert.IsNotNull(offenceController.GetOffences());
        }
        
        [Test]
        public void GetOffences_WithDBRows_ReturnsAllDBItems()
        {
            OffenceController offenceController = new OffenceController();
            DBContext dBContext = new DBContext();
            Random random = new Random();

            //clear table
            dBContext.ExecuteQuery(new System.Data.SqlClient.SqlCommand("DELETE FROM offence"));

            //add new rows
            int rowCount = random.Next(1, 13);
            for (int i = 0; i < rowCount; i++)
            {
                offenceController.SetOffence(dateTime, "unit for returning rows", 1, (OffenceCategories)random.Next(0, 8));
            }

            Assert.AreEqual(rowCount, offenceController.GetOffences().Count);
        }*/
    }
}
