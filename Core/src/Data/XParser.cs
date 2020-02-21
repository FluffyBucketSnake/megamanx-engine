using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace MegamanX.Data
{
    public static class XParser
    {
        public static void AssertExists(XElement xElement, string attributeName)
        {
            var attribute = xElement.Attribute(attributeName);
            if (attribute == null)
            {
                var li = (IXmlLineInfo)xElement;
                throw new XmlException($"'{xElement.Name}' doesn't has '{attributeName}' attribute.\nAt line {li.LineNumber}, position {li.LinePosition}.");
            }
        }

        public static int ParseInt(XElement xElement, string attributeName)
        {
            AssertExists(xElement,attributeName);

            int result;
            if (!Int32.TryParse(xElement.Attribute(attributeName).Value, out result))
            {
                var li = (IXmlLineInfo)xElement;
                throw new XmlException($"'{attributeName}' has a invalid value.\nAt line {li.LineNumber}, position {li.LinePosition}.");
            }
            return result;
        }

        public static string GetString(XElement xElement, string attributeName)
        {
            AssertExists(xElement,attributeName);

            return xElement.Attribute(attributeName).Value;
        }

        public static float ParseFloat(XElement xElement, string attributeName)
        {
            
            AssertExists(xElement,attributeName);

            float result;
            if (!Single.TryParse(xElement.Attribute(attributeName).Value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                var li = (IXmlLineInfo)xElement;
                throw new XmlException($"'{attributeName}' has a invalid value.\nAt line {li.LineNumber}, position {li.LinePosition}.");
            }
            return result;
        }
    }
}