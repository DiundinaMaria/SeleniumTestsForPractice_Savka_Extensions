using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Selenium_tests_Savka_Extensions;

public class SeleniumTestsForPractice
{
    public ChromeDriver driver;

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions ();
        options.AddArguments ("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        // Зайти в хром (с помощью вебдрайвера)
        driver = new ChromeDriver(options);
        
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        Authorization();
    }
    
    public void Authorization()
    {
        // Перейти по урлу "https://staff-testing.testkontur.ru"
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        
        // Ввести логин и пароль
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("mary.diundina@gmail.com");
        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("Lutido1113!");
        
        // Нажать на кнопку "Войти"
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        
        // Явное ожидание урла
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/news"));
    }
    
    [Test]
    public void NewsPageAfterLogin()
    {
         // Проверить, что мы находимся на странице новостей (с помощью заголовка "Новости")
         var news = driver.FindElement(By.CssSelector("[data-tid='Title']"));
         Assert.That(news.Displayed, "Заголовок страницы новостей не отображается.");
    }
    
    [Test]
    public void NavigationTest()
    {
        // Кликнуть на кнопку отображения бокового меню
        var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sideMenu.Click();
        
        // Кликнуть на кнопку "Сообщества"
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']"))
            .First(element => element.Displayed);
        community.Click();
        
        // Проверить, что заголовок "Сообщества" есть на странице
        var communityTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        communityTitle.Text.Should().Be("Сообщества");
    }

    [Test]
    public void DropdownMenuDisplayedTest()
    {
        // Кликнуть на кнопку отображения выпадающего меню
        var dropdownButton = driver.FindElement(By.CssSelector("[data-tid='DropdownButton']"));
        dropdownButton.Click();
        
        // Проверить, что выпадающее меню отображается на странице
        var dropdownMenu = driver.FindElement(By.CssSelector("[data-tid='ScrollContainer__inner']"));
        Assert.That(dropdownMenu.Displayed, "Выпадающее меню не отображается.");
    }

    [Test]
    public void CreateCommunityWithEmptyFieldsValidation()
    {
        // Перейти на страницу сообществ (по урлу)
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
        
        // Кликнуть на кнопку создания нового сообщества
        var addCommunity = driver.FindElement(By.CssSelector("[data-tid='PageHeader'] button")); 
        addCommunity.Click();
        
        // Кликнуть на кнопку "Создать"
        var createButton = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        createButton.Click();
        
        var validationMessage = driver.FindElement(By.CssSelector("[data-tid='validationMessage']"));
        
        // Проверить, что сообщение валидации отображается
        // Проверить текст валидации
        Assert.Multiple(() =>
        {
            Assert.That(validationMessage.Displayed, "Текст валидации не отображается на странице.");
            Assert.That(validationMessage.Text == "Поле обязательно для заполнения."
                , "Ожидаемый текст валидации: 'Поле обязательно для заполнения.', полученный текст валидации: " + validationMessage.Text);
        });
    }

    [Test]
    public void FileSearch()
    {
        const string namePart = "cat";
        
        // Перейти на страницу раздела "Файлы" (по урлу)
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/files");
        
        // Нажать на кнопку поиска
        var search = driver.FindElement(By.CssSelector("[data-tid='Search']"));
        search.Click();
        
        // Написать в поле поиска имя загруженного файла
        var searchBar = driver.FindElement(By.CssSelector("label[data-tid='Search'] input"));
        searchBar.SendKeys(namePart);
        
        // Подождать появления результата поиска
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='FilesTable'] [data-tid='Item']")));
        
        // Проверить, что количество найденных файлов больше нуля
        var  searchResultItems = driver.FindElements(
            By.CssSelector("[data-tid='FilesTable'] [data-tid='FileName']"));
        
        searchResultItems.Count.Should().BeGreaterThan(0);
    }
    
    [TearDown]
    // Закрываем браузер и убиваем процесс драйвера
    public void TearDown()
    {
        driver.Close();
        driver.Quit();
    }
}