using Microsoft.VisualStudio.TestTools.UnitTesting;
using epcalipers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epcalipers.Tests
{
    [TestClass()]
    public class EPCalculatorTests
    {
        [TestMethod()]
        public void MsecToBpmTest()
        {
            Assert.IsTrue(EPCalculator.MsecToBpm(1000) == 60);
            Assert.IsTrue(EPCalculator.MsecToBpm(500) == 120);
        }

        [TestMethod()]
        public void BpmToMsecTest()
        {
            Assert.IsTrue(EPCalculator.BpmToMsec(200) == 300);
            Assert.IsTrue(EPCalculator.BpmToMsec(100) == 600);
        }

        [TestMethod()]
        public void MsecToSecTest()
        {
           Assert.IsTrue(EPCalculator.MsecToSec(1200) == 1.2);
           Assert.IsTrue(EPCalculator.MsecToSec(1000) == 1);
        }

        [TestMethod()]
        public void MeanIntervalTest()
        {
            Assert.IsTrue(EPCalculator.MeanInterval(300, 3) == 100);
        }

        [TestMethod()]
        public void QtcTest()
        {
            Assert.IsTrue(Math.Round(EPCalculator.QtcBazettMsec(345, 879)) == 368);
            Assert.IsTrue(Math.Round(EPCalculator.SecToMsec(EPCalculator.QtcBazettSec(0.356f, 1.33f))) == 309);
            Assert.IsTrue(Math.Round(EPCalculator.QtcBazettMsec(499, 999)) == 499);
            Assert.IsTrue(Math.Round(EPCalculator.QtcBazettMsec(134, 765)) == 153);
            Assert.IsTrue(Math.Round(EPCalculator.QtcBazettMsec(500, 1000)) == 500);
            Assert.IsTrue(Math.Round(EPCalculator.QtcBazettMsec(371, 1000)) == 371);
            Assert.IsTrue(Math.Round(EPCalculator.QtcBazettMsec(459, 777)) == 521);
        }

        [TestMethod()]
        public void QtcFormulaTest()
        {
            QtcCalculator qtcCalculator = new QtcCalculator(QtcFormula.qtcBzt);
            string result = qtcCalculator.Calculate(0.4, 1.0, false, "sec");
            Assert.AreEqual(result, "Mean RR = 1 sec\nQT = 0.4 sec\nQTc = 0.4 sec (Bazett formula)");
            qtcCalculator = new QtcCalculator(QtcFormula.qtcHdg);
            result = qtcCalculator.Calculate(0.4, 1.0, false, "sec");
            Assert.AreEqual(result, "Mean RR = 1 sec\nQT = 0.4 sec\nQTc = 0.4 sec (Hodges formula)");

        }
    }
}