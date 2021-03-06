using NUnit.Framework;
using SisoDb.Resources;
using SisoDb.Sql2005;

namespace SisoDb.UnitTests.Providers.Connections
{
    [TestFixture]
    public class Sql2005ConnectionInfoTests : UnitTestBase
    {
        [Test]
        public void WhenPassingNormalCnString_ItRocks()
        {
            var cnInfo = new Sql2005ConnectionInfo(@"data source=.;initial catalog=SisoDbTests.Temp;integrated security=SSPI;");

            Assert.AreEqual(StorageProviders.Sql2005, cnInfo.ProviderType);
            Assert.AreEqual(@"Data Source=.;Initial Catalog=;Integrated Security=True;MultipleActiveResultSets=True", cnInfo.ServerConnectionString);
            Assert.AreEqual(@"Data Source=.;Initial Catalog=SisoDbTests.Temp;Integrated Security=True;MultipleActiveResultSets=True", cnInfo.ClientConnectionString);
            Assert.AreEqual("SisoDbTests.Temp", cnInfo.DbName);
        }

        [Test]
        public void WhenExplicitlyPassingMarsFalse_ItBecomesTrue()
        {
            var cnInfo = new Sql2005ConnectionInfo(@"data source=.;initial catalog=SisoDbTests.Temp;integrated security=SSPI;MultipleActiveResultSets=False");

            Assert.AreEqual(StorageProviders.Sql2005, cnInfo.ProviderType);
            Assert.AreEqual(@"Data Source=.;Initial Catalog=;Integrated Security=True;MultipleActiveResultSets=True", cnInfo.ServerConnectionString);
            Assert.AreEqual(@"Data Source=.;Initial Catalog=SisoDbTests.Temp;Integrated Security=True;MultipleActiveResultSets=True", cnInfo.ClientConnectionString);
            Assert.AreEqual("SisoDbTests.Temp", cnInfo.DbName);
        }

        [Test]
        public void WhenMissingDbName_ThrowsSisoDbException()
        {
            var ex = Assert.Throws<SisoDbException>(() => new Sql2005ConnectionInfo(@"data source=.;integrated security=SSPI;"));

            Assert.AreEqual(ExceptionMessages.ConnectionInfo_MissingName, ex.Message);
        }
    }
}