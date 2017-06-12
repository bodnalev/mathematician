using System;
using System.Collections.Generic;
using System.IO;

namespace Mathematician.MMReader
{
    class MMReader
    {
        private string path;

        public MMReader(string path)
        {
            this.path = path;
            StreamReader sr = new StreamReader(path);

            


        }

        private MMSent ReadSent(StreamReader sr)
        {
            MMItem item;
            bool end = false;
            do
            {
                item = ReadItem(sr);
                if (item.type == MMItem.MMItemType.Syntax)
                {
                    if (item.content == "$.")
                    {
                        end = true;
                    }
                }


            }
            while (item != null && !end);
        }

        private MMItem ReadItem(StreamReader sr)
        {
            char ch = (char)sr.Read();
            string s = "";
            do
            {
                s += ch;
                ch = (char)sr.Read();
            }
            while (ch != ' ' && !sr.EndOfStream);
            if (sr.EndOfStream)
            {
                return null;
            }
            return new MMItem(s);
        }


    }
}
