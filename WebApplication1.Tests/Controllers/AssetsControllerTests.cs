using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApplication1.Controllers.Tests
{
    [TestClass()]
    public class AssetsControllerTests
    {
        [TestMethod()]
        public void IndexTest_下拉選單不可為空值()
        {
            // Arrange
            AssetsController controller = new AssetsController();
            int notExpected = 0;

            // Act
            ViewResult vResult = controller.Index() as ViewResult;
            var riskLvl = vResult.ViewData["RiskLvl"];
            var qryType = vResult.ViewData["QryType"];
            int riskLvlCount = 0;
            int qryTypeCount = 0;
            if (riskLvl != null)
            {
                riskLvlCount = (riskLvl as SelectList).Count();
            }
            
            if (qryType != null)
            {
                qryTypeCount = (qryType as SelectList).Count();
            }

            // Assert
            Assert.IsNotNull(riskLvl);
            Assert.IsNotNull(qryType);
            Assert.AreNotEqual(notExpected, riskLvlCount);
            Assert.AreNotEqual(notExpected, qryTypeCount);
        }

    }
}