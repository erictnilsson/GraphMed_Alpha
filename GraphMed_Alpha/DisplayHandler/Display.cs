﻿using GraphMed_Alpha.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.DisplayHandler
{
    class Display
    {
        public string ConceptId { get; set; }
        public string Term { get; set; }

        public Display() { }

        public Display(string conceptId, string term)
        {
            this.ConceptId = conceptId;
            this.Term = term;
        }

        public void Print()
        {
            foreach(var prop in this.GetType().GetProperties())
            {
                Console.WriteLine(prop.Name + ": " + prop.GetValue(this)); 
            }
        }
    }
}
