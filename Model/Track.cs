using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{ 
    public class Track
    {
        public string Name { get; }
        public LinkedList<Section> Sections { get; set; }

        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = ConvertArrayToLinkedList(sections);
        }

        public LinkedList<Section> ConvertArrayToLinkedList(SectionTypes[] sectionTypes)
        {
            var linkedList = new LinkedList<Section>();

            foreach (var sectionType in sectionTypes)
            {
                linkedList.AddLast(new Section(sectionType));
            }
            return linkedList;
        }
    }
}
