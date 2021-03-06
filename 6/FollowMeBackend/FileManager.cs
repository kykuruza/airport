﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FollowMeBackend
{
    public sealed class FileManager
    {
        private static readonly object singleLock = new object();

        private static FileManager instance = null;

        public static FileManager Instance
        {
            get
            {
                lock (singleLock)
                {
                    if (instance == null)
                    {
                        instance = new FileManager();
                    }
                    return instance;
                }
            }
        }

        public void Set(string input, string path, bool mode) 
        {
            StreamWriter sw = new StreamWriter(path, mode, Encoding.Default);
            sw.WriteLine(input);
            sw.Close();
        }

        public string Get(int line, string path)
        {

            try
            {
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var textReader = new StreamReader(fileStream))
                {
                    
                    string temp;
                    List<string> lineList = new List<string>();
                    while (!string.IsNullOrEmpty((temp = textReader.ReadLine())))
                    {

                        lineList.Add(temp);
                    }
                    
                    if (lineList.Count >= line)
                    {
                        return lineList[line - 1];
                    }
                    else
                    {
                        return "empty line";     //такого номера строки нет
                    }
                }
                
                



            }
            catch (FileNotFoundException ) //если нет файла,создаём новый с нулевым состоянием
            {
                FileStream fs = File.Create(path);
                byte[] buf = new byte[] {0};
                fs.Write(buf,0, 1);
                fs.Close();
                return "0";
                
            }
        }
    
    }
}
