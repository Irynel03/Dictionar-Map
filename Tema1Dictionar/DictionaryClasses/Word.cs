using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tema1Dictionar
{
    public class Word
    {
        // descriere, imagine, categorie, nume
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ImageSource Image {  get; set; }

        public Word(string category, string name, string description, ImageSource image)
        {
            Category = category;
            Name = name;
            Description = description;
            Image = image;
        }
    }


}