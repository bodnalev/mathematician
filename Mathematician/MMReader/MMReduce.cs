using System;
using System.IO;

namespace Mathematician.MMReader
{
    class MMReduce
    {
        protected string pathOriginal;
        protected string pathDest;

        public MMReduce(string pathOriginal, string pathDest)
        {
            this.pathOriginal = pathOriginal;
            this.pathDest = pathDest;
        }

        public void ProcessSents(int charNumLimit)
        {
            try
            {
                int charNum = 0;
                int sentNum = 0;

                StreamReader sr = new StreamReader(pathOriginal);
                StreamWriter sw = new StreamWriter(pathDest + "\\sent-" + sentNum + ".txt");

                char ch;

                Console.WriteLine("processing started");

                do
                {
                    ch = (char)sr.Read();
                    charNum++;

                    if (sw != null)
                    {
                        sw.Write(ch);
                    }
                    
                    if (ch.Equals('$'))
                    {
                        ch = (char)sr.Peek();
                        if (ch.Equals('.'))
                        {
                            ch = (char)sr.Read();
                            sw.Write(ch);
                            if (sw != null)
                            {
                                sw.Close();
                            }
                            sw = new StreamWriter(pathDest + "\\sent-"+sentNum+".txt");
                            sentNum++;
                        }
                    }
                } while (!sr.EndOfStream && charNum < charNumLimit);
                sr.Close();
                if (sw != null)
                {
                    sw.Close();
                }
                Console.WriteLine("Processing of file finished");
            }
            catch (Exception e)
            {
                Console.Write("Exception: " + e.Message);
            }
        }

        public void ProcessBrackets()
        {
            try
            {
                StreamReader sr = new StreamReader(pathOriginal);
                StreamWriter sw = null;

                bool insideStatem = false;
                int indexStatem = 0;

                char ch;

                Console.WriteLine("processing started");

                do
                {
                    ch = (char)sr.Read();

                    //Console.Write(ch);

                    if (insideStatem)
                    {
                        if (sw != null)
                        {
                            //sw.Write(ch);
                            if (ch.Equals("$") && sr.Peek().Equals('a'))
                            {
                                Console.WriteLine("axiom: "+indexStatem);
                            }
                        }
                    }
                    if (ch.Equals('$'))
                    {
                        ch = (char)sr.Peek();
                        if (ch.Equals('{'))
                        {
                            insideStatem = true;
                            //sw = new StreamWriter(pathDest + "\\Statem-" + indexStatem+".txt");
                            indexStatem++;
                        }
                        else if (ch.Equals('}'))
                        {
                            insideStatem = false;
                            if (sw != null)
                            {
                                sw.Close();
                                sw = null;
                            }
                        }
                    }
                } while (!sr.EndOfStream && indexStatem < 50);
                sr.Close();
                Console.WriteLine("Processing of file finished");
            }
            catch (Exception e){
                Console.Write("Exception: " + e.Message);
            }
        }

    }
}
