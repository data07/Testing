using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace InterValue
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var options = new ChromeOptions();
            options.LeaveBrowserRunning = false;
            IWebDriver driver;
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            options.AddArguments("headless");
            using (driver = new ChromeDriver(service, options))
            {
                driver.Url = "https://github.com/login";
                IWebElement username = driver.FindElement(By.Id("login_field"));
                //Enter your user name
                username.SendKeys(txtUser.Text);
                //Locate
                IWebElement password = driver.FindElement(By.Id("password"));
                //enter password
                password.SendKeys(txtPass.Text);
                driver.FindElement(By.Name("commit")).Click();

                //audit-log-search
                driver.Url = "https://github.com/settings/security-log";
                IWebElement auditlog = driver.FindElement(By.Id("audit-log-search"));
                IWebElement getBox = auditlog.FindElement(By.ClassName("Box"));
 
                var getUser = getBox.FindElements(By.ClassName("member-username"));
      
                var AccountID = getUser.First().Text;
                string strJSON = "{" + Convert.ToChar(34) + "Account" + Convert.ToChar(34) + ":" + Convert.ToChar(34) + AccountID + Convert.ToChar(34) + "," + Convert.ToChar(34) + "Logs"  +Convert.ToChar(34) + ":[";
                var getAllEvents = getBox.FindElements(By.XPath("//span[contains(@class, 'audit-type')]/*[@href]"));
                var getAllTime = getBox.FindElements(By.XPath("//div/*[@datetime]"));

                string joinedString = "";
                for (int i =0; i<=getAllEvents.Count-1;i++)
                {
                    if (joinedString!="") { joinedString += ","; }
                    joinedString = joinedString + "{" + Convert.ToChar(34) + "event" + Convert.ToChar(34) + ":" + Convert.ToChar(34) + getAllEvents[i].Text + Convert.ToChar(34) + "," + Convert.ToChar(34) + "time" + Convert.ToChar(34) + ":" + Convert.ToChar(34) + getAllTime[i].Text + Convert.ToChar(34) + "}";


                }
  
                txtJSON.Text = strJSON + joinedString + "]}";
            }
        }
    }
}
