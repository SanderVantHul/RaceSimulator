using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class Section
    {
        public enum SectionTypes
        {
            Straight,
            LeftCorner,
            RightCorner,
            StartGrid,
            Finish
        }

        public SectionTypes SectionType { get; set; }

        public Section(SectionTypes sectionType)
        {
            SectionType = sectionType;
        }
    }
}
