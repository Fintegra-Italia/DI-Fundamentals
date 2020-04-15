using System;
using System.IO;


namespace DataLayer
{
    public class UserWriter
    {
        public void Append(string filename, string riga)
        {
            if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            if (String.IsNullOrEmpty(riga) || String.IsNullOrWhiteSpace(riga))
                throw new ArgumentNullException("contenuto riga");
            if (!File.Exists(filename))
            {
                using (StreamWriter sw = File.CreateText(filename))
                {
                    sw.WriteLine(riga);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filename))
                {
                    sw.WriteLine(riga);
                }
            }
        }
        public void Reset(string filename)
        {
            if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");
            File.Delete(filename);
        }
    }
}
