using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGroup.Config.Channel
{
    public class ColorChannel :NamedItem
    {
        internal class Element
        {
            public int nChannel;
            public String sElement;

            public Element(int _channel, String _element)
            {
                nChannel = _channel;
                sElement = _element;
            }
        }

        private List<Element> vecElements;
        private uint defaultColor;

        public ColorChannel(String name, String alias, uint _default)
            :base(name,alias)
        {
            defaultColor = _default;
            vecElements = new List<Element>();
        }

        public void AddModelElement(int channel, String element)
        {
            Element e = new Element(channel, element);
            vecElements.Add(e);
        }

        public int GetModelElementChannel(int n)
        {
            return vecElements[n].nChannel;
        }

        public String GetModelElementTarget(int n)
        {
            return vecElements[n].sElement;
        }
    }
}
