using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChensMinionRetarget.Tests.MinionRetarget
{
    [TestClass]
    public class Constants
    {
        [TestMethod]
        public void ModVer_Length_ReturnsCorrectFormat()
        {
            string result = Chen.MinionRetarget.MinionRetargetPlugin.ModVer;
            const int ModVersionCount = 3;

            int count = result.Split('.').Length;

            Assert.AreEqual(ModVersionCount, count);
        }

        [TestMethod]
        public void ModName_Value_ReturnsCorrectName()
        {
            string result = Chen.MinionRetarget.MinionRetargetPlugin.ModName;
            const string ModName = "ChensMinionRetarget";

            Assert.AreEqual(ModName, result);
        }

        [TestMethod]
        public void ModGuid_Value_ReturnsCorrectGuid()
        {
            string result = Chen.MinionRetarget.MinionRetargetPlugin.ModGuid;
            const string ModGuid = "com.Chen.ChensMinionRetarget";

            Assert.AreEqual(ModGuid, result);
        }
    }
}