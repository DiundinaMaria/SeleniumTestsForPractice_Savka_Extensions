using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Selenium_tests_Savka_Extensions;

public class SeleniumTestsForPractice
{
    [Test]
    public void Authorization()
    {
        var options = new ChromeOptions ();
        options.AddArguments ("--no-sandbox", "--start-maximized", "--disable-extensions");
        
         // - Зайти в хром (с помощью вебдрайвера)
         var driver = new ChromeDriver(options);
         
         // - Перейти по урлу https://staff-testing.testkontur.ru
         driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
         Thread.Sleep(3000);
         
         // - Ввести логин и пароль
         var login = driver.FindElement(By.Id("Username"));
         login.SendKeys("mary.diundina@gmail.com");
         var password = driver.FindElement(By.Name("Password"));
         password.SendKeys("Lutido1113!");
         Thread.Sleep(3000);
         
         // - Нажать на кнопку "Войти"
         var enter = driver.FindElement(By.Name("button"));
         enter.Click();
         Thread.Sleep(3000);
         
         // - Проверяем, что мы находимся на нужной странице https://staff-testing.testkontur.ru/news
         var curUrl = driver.Url;
         Assert.That(curUrl == "https://staff-testing.testkontur.ru/news");
         
         // - Закрываем браузер и убиваем процесс драйвера
         driver.Quit();
    }
}