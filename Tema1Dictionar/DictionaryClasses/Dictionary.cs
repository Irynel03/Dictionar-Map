using System.IO;
using Newtonsoft.Json;

namespace Tema1Dictionar
{
    public class Dictionary
    {
        public List<Word> Words {  get; set; }
        public List<Admin> Admins {  get; set; } 

        public Dictionary() 
        {
            Words = new List<Word>();
            Admins = new List<Admin>();
            LoadWordsFromJson("C:/Users//andre//Desktop/sem II/MAP/Tema 1 Dictionar/Tema1Dictionar/Tema1Dictionar/DictionaryClasses/Data.json");
            LoadAdminsFromJson("C:/Users//andre//Desktop/sem II/MAP/Tema 1 Dictionar/Tema1Dictionar/DictionaryClasses/AdminsData.json");
        }

        private void LoadWordsFromJson(string filePath)
        {
            if(File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Words = JsonConvert.DeserializeObject<List<Word>>(json);
            }
        }
        private void LoadAdminsFromJson(string filePath)
        {
            if(File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Admins = JsonConvert.DeserializeObject<List<Admin>>(json);
            }
        }
        public Word GetRandomWord()
        {
            Random random = new Random();

            int randomIndex = random.Next(0, Words.Count);
            return Words[randomIndex];
        }
        public Word GetFirstWord()
        {
            return Words[0];
        }
        public void SaveWordsToJson(string filePath)
        {
            string json = JsonConvert.SerializeObject(Words, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        public void AddWord(Word word)
        {
            if(!ContainsWord(word.Name)) 
            {
                Words.Add(word);
            }
        }
        public bool ContainsWord(string word)
        {
            return Words.Any(w => w.Name == word);
        }
        public void RemoveWord(string word) 
        {
            Words.RemoveAll(w => w.Name == word);
            SaveWordsToJson("C:/Users//andre//Desktop/sem II/MAP/Tema 1 Dictionar/Tema1Dictionar/Tema1Dictionar/DictionaryClasses/Data.json");
        }
        public bool IsAdminLoginDataCorect(string email, string password)
        {
            return Admins.Any(admin => admin.Email == email && admin.Password == password);
        }
        public bool ImageExists(string imageUri)
        {
            return File.Exists(imageUri);
        }
        public List<string> GetAllCategoriesList()
        {
            List<string> allCategories = Words.Select(word => word.Category).Distinct().ToList();
            return allCategories;
        }

    }
}
