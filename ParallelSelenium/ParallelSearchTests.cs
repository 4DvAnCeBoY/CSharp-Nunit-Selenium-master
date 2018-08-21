using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using NUnit.Framework;
using ParallelSelenium.Utils;
using System.Threading;
using System.Collections.Generic;

namespace ParallelSelenium
{
    [TestFixture("chrome", "60", "Windows 10")]
    [TestFixture("internet explorer", "11", "Windows 7")]
    [TestFixture("firefox", "41", "Windows 7")]
    [TestFixture("chrome", "30", "Windows 7")]
    [TestFixture("internet explorer", "9", "Windows 7")]
    [TestFixture("firefox", "30", "Windows 7")]
    [TestFixture("chrome", "38", "Windows 7")]
    [TestFixture("internet explorer", "10", "Windows 7")]
    [TestFixture("firefox", "35", "Windows 7")]
    [Parallelizable(ParallelScope.Children)]
    public class ParallelSearchTests
    {

        ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
        private String browser;
        private String version;
        private String os;

        public ParallelSearchTests(String browser, String version, String os)
        {
            this.browser = browser;
            this.version = version;
            this.os = os;
        }

        [SetUp]
        public void Init()
        {
            /* Web proxy setup to be used with the underlying Rest requests.
            **/
            /*
            WebProxy iProxy = new WebProxy("192.168.1.159:808", true);
            iProxy.UseDefaultCredentials = true;
            iProxy.Credentials = new NetworkCredential("test", "hello123");
            WebRequest.DefaultWebProxy = iProxy;
            */
            String seleniumUri = "http://{0}:{1}/wd/hub";
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.BrowserName, browser);
            capabilities.SetCapability(CapabilityType.Version, version);
            capabilities.SetCapability(CapabilityType.Platform, os);
            //Sauce Connect setup.
            //Requires a named tunnel.
            if (Constants.tunnelId != null)
            {
                capabilities.SetCapability("tunnel-identifier", Constants.tunnelId);
            }
            if (Constants.buildTag != null)
            {
                capabilities.SetCapability("build", Constants.buildTag);
            }
            if (Constants.seleniumRelayPort != null && Constants.seleniumRelayHost != null)
            {
                seleniumUri = String.Format(seleniumUri, Constants.seleniumRelayHost, Constants.seleniumRelayPort);
            }
            else {
                seleniumUri = "https://ondemand.saucelabs.com:443/wd/hub";
            }
            capabilities.SetCapability("username", Constants.sauceUser);
            capabilities.SetCapability("accessKey", Constants.sauceKey);
            capabilities.SetCapability("name",
            String.Format("{0}:{1}: [{2}]",
            TestContext.CurrentContext.Test.ClassName,
            TestContext.CurrentContext.Test.MethodName,
            TestContext.CurrentContext.Test.Properties.Get("Description")));
            driver.Value = new CustomRemoteWebDriver(new Uri(seleniumUri), capabilities, TimeSpan.FromSeconds(600));
            driver.Value.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Todotest()
        {
            {
                Console.WriteLine("Navigating to todos app.");
                driver.Value.Navigate().GoToUrl("https://4dvanceboy.github.io/lambdatest/todo.html");

                // Check the title
                Console.WriteLine("Clicking Checkbox");
                //[FindsBy(How = How.Id, Using = "todo-4")]

                driver.Value.FindElement(By.Name("todo-4")).Click();
                Console.WriteLine("Clicking Checkbox");
                driver.Value.FindElement(By.Name("todo-5")).Click();


                // If both clicks worked, then te following List should have length 2
                IList<IWebElement> elems = driver.Value.FindElements(By.ClassName("done-true"));
                // so we'll assert that this is correct.
                Assert.AreEqual(2, elems.Count);

                Console.WriteLine("Entering Text");
                driver.Value.FindElement(By.Id("todotext")).SendKeys("Get Taste of Lambda and Stick to It");
                driver.Value.FindElement(By.Id("addbutton")).Click();


                // lets also assert that the new todo we added is in the list
                string spanText = driver.Value.FindElement(By.XPath("/html/body/div/div/div/ul/li[6]/span")).Text;
                Assert.AreEqual("Get Taste of Lambda and Stick to It", spanText);


                Console.WriteLine("Taking Snapshot");

                driver.Value.Quit();
            }
        }

        [TearDown]
        public void Cleanup()
        {
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed;
            try
            {
                // Logs the result to Sauce Labs
                ((IJavaScriptExecutor)driver.Value).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            }
            finally
            {
                Console.WriteLine(String.Format("SauceOnDemandSessionID={0} job-name={1}", ((CustomRemoteWebDriver)driver.Value).getSessionId(), TestContext.CurrentContext.Test.MethodName));
                // Terminates the remote webdriver session
                driver.Value.Quit();
            }
        }
    }
}
