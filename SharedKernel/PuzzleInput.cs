﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharedKernel
{
    public class PuzzleInput
    {
        private List<string> _lines = new List<string>();

        public List<string> Lines 
        {
            get { return _lines; }
        }

        public string FullText
        {
            get { return string.Join(System.Environment.NewLine, _lines); }
        }

        public PuzzleInput(string filePath) : this(filePath, false) { }

        public PuzzleInput(string filePath, bool ignoreEmptyLines)
        {
            string line;
            StreamReader file = File.OpenText(filePath);

            while ((line = file.ReadLine()) != null)
            {
                if(!ignoreEmptyLines | line != "")
                {
                    _lines.Add(line);
                }
            }

            file.Close();
        }

    }
}
