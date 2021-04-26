using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace teeze_bot.Sites.SeleniumItems
{
    public class SeleniumBaseControl : IWebElement
    {
        protected IWebElement baseElement;
        protected IWebDriver driver;
        protected IWait<IWebDriver> waitHandler;

        public SeleniumBaseControl(IWebElement element, IWebDriver driver)
        {
            this.baseElement = element;
            this.driver = driver;
            this.waitHandler = new WebDriverWait(this.driver, TimeSpan.FromSeconds(30.00));
        }

        #region IWebElement Properties

        public void Clear()
        {
            try
            {
                this.baseElement.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void Click()
        {
            try
            {
                this.baseElement.Click();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Displayed
        {
            get
            {
                try
                {
                    return this.baseElement.Displayed;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Prüft ob das Base Element existiert und sichtbar ist
        /// </summary>
        /// <remarks>Wirft im Gegensatz zu Displayed keine Exception wenn das Element nicht sichtbar ist.</remarks>
        /// <returns>Element ist vorhanden und sichtbar</returns>
        public bool Exists()
        {
            try
            {
                return this.baseElement.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool Enabled
        {
            get
            {
                try
                {
                    return this.baseElement.Enabled;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public string GetAttribute(string attributeName)
        {
            try
            {
                return this.baseElement.GetAttribute(attributeName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetCssValue(string propertyName)
        {
            try
            {
                return this.baseElement.GetCssValue(propertyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetProperty(string propertyName)
        {
            try
            {
                return this.baseElement.GetProperty(propertyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Drawing.Point Location
        {
            get
            {
                try { return this.baseElement.Location; }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool Selected
        {
            get
            {
                try
                {
                    return this.baseElement.Selected;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void SendKeys(string text)
        {
            try
            {
                this.baseElement.SendKeys(text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Size Size
        {
            get
            {
                try
                {
                    return this.baseElement.Size;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Submit()
        {
            try
            {
                this.baseElement.Submit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string TagName
        {
            get
            {
                try { return this.baseElement.TagName; }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public virtual string Text
        {
            get
            {
                try { return this.baseElement.Text; }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public IWebElement FindElement(By by)
        {
            try
            {
                return this.baseElement.FindElement(by);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            try
            {
                return this.baseElement.FindElements(by);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected bool HasElement(IWebElement searchBase, By by)
        {
            // Save Values from Implicit Wait
            TimeSpan timeouts = this.driver.Manage().Timeouts().ImplicitWait;

            // Set Implicit Timeout to 0
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            // Check if Any Item exists
            bool result = searchBase.FindElements(by).Any();

            // Reset Timeout to Default
            driver.Manage().Timeouts().ImplicitWait = timeouts;

            return result;
        }

        protected bool HasAllElementHidden(By by)
        {
            // Save Values from Implicit Wait
            TimeSpan timeouts = this.driver.Manage().Timeouts().ImplicitWait;

            // Set Implicit Timeout to 0
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            // Check if Any Item exists
            bool result = driver.FindElements(by).All(itm =>
            {
                try
                {
                    return itm.Displayed == false;
                }
                catch (StaleElementReferenceException)
                {
                    // Stale Elemente als Sichtbar zurückgeben. Führt im schlimmsten Fall zu einer erneuten Überprüfung.
                    return false;
                }
            });

            // Reset Timeout to Default
            driver.Manage().Timeouts().ImplicitWait = timeouts;

            return result;
        }

        #endregion IWebElement Properties

        /// <summary>
        /// Prüft ob ein Element existiert. Ist das Element nicht vorhanden, wird der vorgang wiederholt bis ein Timeout erreicht ist.
        /// </summary>
        /// <returns>Element exisitert</returns>
        public bool IsExisting()
        {
            // Warten bis Base Container für Toast Erzeugt ist. Kann einen Moment dauern, wenn erster Toast auf Seite
            WebDriverWait wait = new WebDriverWait(this.driver, new TimeSpan(0, 0, 30));
            try
            {
                // Falls das element nicht vorhanden ist, dann wird ein Exception geworfen und die Funcktion gibt falsch zurück nach dem Timeout
                return wait.Until((driver) =>
                {
                    try
                    {
                        // Check Element vorhanden und aktiv
                        return this.baseElement != null && this.baseElement.TagName != "";
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });
            }
            catch
            {
                return false;
            }
        }
    }
}