using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace teeze_bot.Sites.SeleniumItems
{
    public class SeleniumDropDownList : SeleniumBaseControl
    {
        public SeleniumDropDownList(IWebElement element, IWebDriver driver)
            : base(element, driver) { }

        public string SelectedValue
        {
            get
            {
                SelectElement selectElement = new SelectElement(this.baseElement);
                return selectElement.SelectedOption.GetAttribute("value");
            }
            set
            {
                try
                {
                    SelectElement selectElement = new SelectElement(this.baseElement);
                    selectElement.SelectByValue(value);
                }
                catch (Exception ex)
                {
                    // To see available options on the screenshots
                    try
                    {
                        this.baseElement.Click();
                    }
                    catch
                    {
                    }
                    throw ex;
                }
            }
        }

        public string SelectedText
        {
            get
            {
                SelectElement selectElement = new SelectElement(this.baseElement);
                return selectElement.SelectedOption.Text;
            }
            set
            {
                try
                {
                    SelectElement selectElement = new SelectElement(this.baseElement);
                    selectElement.SelectByText(value);
                }
                catch (Exception ex)
                {
                    // To see available options on the screenshots
                    try
                    {
                        this.baseElement.Click();
                    }
                    catch
                    {
                    }
                    throw ex;
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                SelectElement selectElement = new SelectElement(this.baseElement);
                return selectElement.Options.IndexOf(selectElement.SelectedOption);
            }
            set
            {
                SelectElement selectElement = new SelectElement(this.baseElement);
                selectElement.SelectByIndex(value);
            }
        }

        public bool HasItemWithText(string text, bool useAsLike = false)
        {
            SelectElement selectBox = new SelectElement(this.baseElement);

            if (useAsLike)
                return selectBox.Options.Any(itm => itm.Text.IndexOf(text) >= 0);
            else
                return selectBox.Options.Any(itm => itm.Text == text);
        }

        public bool HasItemWithValue(string value)
        {
            SelectElement selectBox = new SelectElement(this.baseElement);
            return selectBox.Options.Any(itm => itm.GetAttribute("value") == value);
        }
    }
}