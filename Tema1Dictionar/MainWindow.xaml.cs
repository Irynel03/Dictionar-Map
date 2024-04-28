using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tema1Dictionar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary Dictionary = new Dictionary();
        private List<Word> WordsNotGuessed = new List<Word>();
        private List<Word> WordsToGuess = new List<Word>();
        private List<string> WordsNameGuessed = new List<string>(new string[5]);
        private List<int> WordsGuessImageShow = new List<int>();    //meh

        private int NoOfCurrentWordToGuess;
        public MainWindow()
        {
            InitializeComponent();

            
            //de implementat metoda pentru caractere speciale
        }

        private void AdminEnterButton_Clicked(object sender, RoutedEventArgs e)
        {
            EntryMenuGrid.Visibility = Visibility.Collapsed;
            AdminLoginGrid.Visibility = Visibility.Visible;
        }
        private void WordSearchButton_Click(object sender, RoutedEventArgs e)
        {
            EntryMenuGrid.Visibility = Visibility.Collapsed;
            SearchWordGrid.Visibility = Visibility.Visible;
           
            List<string> wordNames = Dictionary.Words.Select(word => word.Name).ToList();
            SearchWordBar.ItemsSource = wordNames;
            List<string> categoryNames = Dictionary.Words.Select(word => word.Category.ToString()).Distinct().ToList();
            SearchCategoryBar.ItemsSource = categoryNames;

            SearchWordBar.Text = "introdu cuvantul...";
            SearchWordBar.Foreground = Brushes.Gray;

            SearchCategoryBar.Text = "categoria...";
            SearchCategoryBar.Foreground = Brushes.Gray;
        }
        private void GameEnterButton_Click(object sender, RoutedEventArgs e)
        {
            EntryMenuGrid.Visibility = Visibility.Collapsed;

            WordsNotGuessed = Dictionary.Words.ToList();

            NoOfCurrentWordToGuess = 0;
            Word word = GetRandomWordToGuess();
            WordsToGuess.Add(word);
            WordsNameGuessed = new List<string>(new string[5]);
            WordsGuessImageShow = new List<int>(new int[5]);

            bool hasImage = false;
            if (word.Image.ToString() != "file:///C:/Users/andre/Desktop/sem II/MAP/Tema 1 Dictionar/Tema1Dictionar/Tema1Dictionar/Assets/noImage.png")
                hasImage = true;

            if (hasImage)
            {
                int randomValue = new Random().Next(2);
                WordsGuessImageShow[NoOfCurrentWordToGuess] = randomValue;
                if (randomValue == 0)
                {
                    ToBeGuessedImage.Source = word.Image;
                    ToBeGuessedImage.Visibility = Visibility.Visible;
                    WordDescriptionTextBlock.Visibility = Visibility.Collapsed;
                }
                else
                {
                    WordDescriptionTextBlock.Text = word.Description;
                    WordDescriptionTextBlock.Visibility = Visibility.Visible;
                    ToBeGuessedImage.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                WordDescriptionTextBlock.Text = word.Description;
                WordDescriptionTextBlock.Visibility = Visibility.Visible;
            }
            NoOfCurrentWordToGuess = 1;
            GuessWordGameGrid.Visibility = Visibility.Visible;
        }
        private void BackFromAdmin_Click(object sender, RoutedEventArgs e)
        {
            EntryMenuGrid.Visibility = Visibility.Visible;
            AdminLoginGrid.Visibility = Visibility.Collapsed;
            PasswordTextBox.Password = "";
            EmailTextBox.Text = "";
        }
        private void LoginAdminButton_Click(object sender, RoutedEventArgs e)
        {
            string parola = PasswordTextBox.Password;
            string email = EmailTextBox.Text;

            if(Dictionary.IsAdminLoginDataCorect(email, parola))
            {
                AdminOptionsGrid.Visibility = Visibility.Visible;
                AdminLoginGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Date incorecte, mai incearca.");
            }
            PasswordTextBox.Password = "";
            EmailTextBox.Text = "";
        }
        private void DeleteWordButton_Click(object sender, RoutedEventArgs e)
        {
            AdminOptionsGrid.Visibility = Visibility.Collapsed;
            DeleteWordGrid.Visibility = Visibility.Visible;
        }
        private void AddWordButton_Click(object sender, RoutedEventArgs e)
        {
            AdminOptionsGrid.Visibility = Visibility.Collapsed;
            AddWordGrid.Visibility = Visibility.Visible;
            WordCategoryTextBar.ItemsSource = Dictionary.GetAllCategoriesList();
        }
        private void BackToAdminLogin_Click(object sender, RoutedEventArgs e)
        {
            AdminOptionsGrid.Visibility = Visibility.Collapsed;
            DeleteWordGrid.Visibility = Visibility.Collapsed;
            AdminLoginGrid.Visibility = Visibility.Visible;
            AddWordGrid.Visibility = Visibility.Collapsed;
            WordInfoGrid.Visibility = Visibility.Collapsed;
        }
        private void DeleteThisWordButton_Click(object sender, RoutedEventArgs e)
        {
            string word = WordToDeleteTextBox.Text;
            if(Dictionary.ContainsWord(word))
            {
                Dictionary.RemoveWord(word);
                MessageBox.Show("Cuvantul a fost sters cu succes");
            }
            else
            {
                MessageBox.Show("Cuvantul nu a fost gasit");
            }
        }
        private void AddThisWordButton_Click(object sender, RoutedEventArgs e)
        {
            string wordName = WordNameTextBox.Text;
            string wordCategory = WordCategoryTextBar.Text;
            string wordDescription = WordDescriptionTextBox.Text;
            string wordImage = WordImageTextBox.Text;

            if (Dictionary.ContainsWord(wordName) || wordName.Length < 2)
            {
                MessageBox.Show("Cuvantul deja exista sau este prea scurt");
                return;
            }
            if (wordDescription.Length < 10)
            {
                MessageBox.Show("Descrierea este prea mica");
                return;
            }
            if (wordImage.Length < 1)
            {
                wordImage = "C:\\Users\\andre\\Desktop\\sem II\\MAP\\Tema 1 Dictionar\\Tema1Dictionar\\Tema1Dictionar\\Assets\\noImage.png";
            }
            else if (Dictionary.ImageExists(wordImage))
            {
                string destinationFolder = "C:\\Users\\andre\\Desktop\\sem II\\MAP\\Tema 1 Dictionar\\Tema1Dictionar\\Tema1Dictionar\\Assets";

                // Construiește calea completă pentru noua locație
                string destinationPath = System.IO.Path.Combine(destinationFolder, WordNameTextBox.Text+".jpg");

                // Copiază fișierul
                try
                {
                    // Copiază fișierul
                    System.IO.File.Copy(wordImage, destinationPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la copierea imaginii: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Nu s-a gasit imaginea");
                return;
            }

            Word newWord = new Word(wordCategory, wordName, wordDescription, new BitmapImage(new Uri(wordImage)));
            Dictionary.Words.Add(newWord);
            Dictionary.SaveWordsToJson("C:/Users//andre//Desktop/sem II/MAP/Tema 1 Dictionar/Tema1Dictionar/Tema1Dictionar/DictionaryClasses/Data.json");

            MessageBox.Show("Cuvantul a fost adaugat cu succes");

            WordNameTextBox.Text = "";
            WordCategoryTextBar.Text = "";
            WordDescriptionTextBox.Text = "";
            AddWordImg.Visibility = Visibility.Collapsed;
            WordImageTextBox.Text = "";
        }
        private void NextWordToGuessButton_Click(object sender, RoutedEventArgs e)
        {
            if (PreviousWordToGuessButton.Visibility == Visibility.Collapsed)
            {
                PreviousWordToGuessButton.Visibility = Visibility.Visible;
            }

            if (NoOfCurrentWordToGuess <= 4)
            {
                if (WordsToGuess.Count < NoOfCurrentWordToGuess + 1)
                {
                    WordsNameGuessed[NoOfCurrentWordToGuess - 1] = WordToGuessTextBox.Text;
                    Word word = GetRandomWordToGuess();
                    if (WordsToGuess.Count != 5)
                    {
                        WordsToGuess.Add(word);
                    }
                    bool hasImage = false;

                    if (word.Image.ToString() != "file:///C:/Users/andre/Desktop/sem II/MAP/Tema 1 Dictionar/Tema1Dictionar/Tema1Dictionar/Assets/noImage.png")
                        hasImage = true;

                    if (hasImage)
                    {
                        int randomValue = new Random().Next(2);
                        WordsGuessImageShow[NoOfCurrentWordToGuess] = randomValue;
                        if (randomValue == 0)
                        {
                            ToBeGuessedImage.Source = word.Image;
                            ToBeGuessedImage.Visibility = Visibility.Visible;
                            WordDescriptionTextBlock.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            WordDescriptionTextBlock.Text = word.Description;
                            WordDescriptionTextBlock.Visibility = Visibility.Visible;
                            ToBeGuessedImage.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        WordDescriptionTextBlock.Text = word.Description;
                        WordDescriptionTextBlock.Visibility = Visibility.Visible;
                        ToBeGuessedImage.Visibility = Visibility.Collapsed;
                    }
                    if (NoOfCurrentWordToGuess == 4)
                    {
                        NextWordToGuessButton.Content = "Finish";
                    }
                }
                else
                {
                    WordsNameGuessed[NoOfCurrentWordToGuess - 1] = WordToGuessTextBox.Text;
                    Word word = WordsToGuess[NoOfCurrentWordToGuess];
                    bool hasImage = false;
                    if (word.Image.ToString() != "file:///C:/Users/andre/Desktop/sem II/MAP/Tema 1 Dictionar/Tema1Dictionar/Tema1Dictionar/Assets/noImage.png")
                        hasImage = true;
                    if (hasImage)
                    {
                        int randomValue = WordsGuessImageShow[NoOfCurrentWordToGuess];
                        if (randomValue == 0)
                        {
                            ToBeGuessedImage.Source = word.Image;
                            ToBeGuessedImage.Visibility = Visibility.Visible;
                            WordDescriptionTextBlock.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            WordDescriptionTextBlock.Text = word.Description;
                            WordDescriptionTextBlock.Visibility = Visibility.Visible;
                            ToBeGuessedImage.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        WordDescriptionTextBlock.Text = word.Description;
                        WordDescriptionTextBlock.Visibility = Visibility.Visible;
                        ToBeGuessedImage.Visibility = Visibility.Collapsed;
                    }
                    if (NoOfCurrentWordToGuess == 4)
                    {
                        NextWordToGuessButton.Content = "Finish";
                    }
                }
                if (NoOfCurrentWordToGuess != 5)
                    NoOfCurrentWordToGuess++;
                WordToGuessTextBox.Text = WordsNameGuessed[NoOfCurrentWordToGuess - 1];
            }
            else
            {
                WordsNameGuessed[NoOfCurrentWordToGuess - 1] = WordToGuessTextBox.Text;
                GetResultsGuessGame();
            }
            
        }
        private void PreviousWordToGuessButton_Click(object sender, RoutedEventArgs e)
        {
            WordsNameGuessed[NoOfCurrentWordToGuess-1] = WordToGuessTextBox.Text;

            if (NoOfCurrentWordToGuess != 1) 
            {
                NoOfCurrentWordToGuess--;
            }
            if (NoOfCurrentWordToGuess == 1)
            {
                PreviousWordToGuessButton.Visibility = Visibility.Collapsed;
            }
            if(NextWordToGuessButton.Content == "Finish")
            {
                NextWordToGuessButton.Content = "Next";
            }

            Word word = WordsToGuess[NoOfCurrentWordToGuess - 1];
            bool hasImage = false;
            if (word.Image.ToString() != "file:///C:/Users/andre/Desktop/sem II/MAP/Tema 1 Dictionar/Tema1Dictionar/Tema1Dictionar/Assets/noImage.png")
                hasImage = true;
            if (hasImage)
            {
                int randomValue = WordsGuessImageShow[NoOfCurrentWordToGuess - 1];
                if (randomValue == 0)
                {
                    ToBeGuessedImage.Source = word.Image;
                    ToBeGuessedImage.Visibility = Visibility.Visible;
                    WordDescriptionTextBlock.Visibility = Visibility.Collapsed;
                }
                else
                {
                    WordDescriptionTextBlock.Text = word.Description;
                    WordDescriptionTextBlock.Visibility = Visibility.Visible;
                    ToBeGuessedImage.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                WordDescriptionTextBlock.Text = word.Description;
                WordDescriptionTextBlock.Visibility = Visibility.Visible;
                ToBeGuessedImage.Visibility = Visibility.Collapsed;
            }
            WordToGuessTextBox.Text = WordsNameGuessed[NoOfCurrentWordToGuess-1];

        }
        private Word GetRandomWordToGuess()
        {
            Random random = new Random();
            if (WordsNotGuessed.Count > 0)
            {
                int randomIndex = random.Next(0, WordsNotGuessed.Count);
                Word selectedWord = WordsNotGuessed[randomIndex];
                WordsNotGuessed.RemoveAt(randomIndex);
                return selectedWord;
            }
            else
            {
                return null; // Poți gestiona cazul în care lista de cuvinte este goală
            }
        }
        private void GetResultsGuessGame()
        {
            int goodGuesses = 0;
            for(int i = 0; i < 5; i++)
            {
                if (WordsToGuess[i].Name == WordsNameGuessed[i])
                {
                    goodGuesses++;
                    WordsNameGuessed[i] = "";
                }
            }

            MessageBox.Show("Ai ghicit: " + goodGuesses + " cuvinte!");
            WordsToGuess.Clear();
            WordsNameGuessed.Clear();
            WordToGuessTextBox.Text = "";



            NextWordToGuessButton.Content = "Next";
            ToBeGuessedImage.Visibility = Visibility.Collapsed;
            WordDescriptionTextBlock.Visibility = Visibility.Collapsed;
            PreviousWordToGuessButton.Visibility = Visibility.Collapsed;

            GuessWordGameGrid.Visibility = Visibility.Collapsed;
            EntryMenuGrid.Visibility = Visibility.Visible;
        }
        private void SearchWordButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedCategory = SearchCategoryBar.Text;
            bool categoryIsSelected = Dictionary.Words.Select(word => word.Category).Distinct().ToList().Contains(SearchCategoryBar.Text);

            if (categoryIsSelected) 
            {
                //revin

            }
            string word = SearchWordBar.Text;
            Word foundWord = Dictionary.Words.FirstOrDefault(w => w.Name == word);
            if (foundWord != null)
            {
                string category = foundWord.Category.ToString();
                string description = foundWord.Description;
                ImageSource image = foundWord.Image;

                WordNameTBInfo.Text = word;
                WordCategoryTBInfo.Text = "Categorie: "+ category;
                WordDescriptionTBInfo.Text = "     " + description;
                WordImageInfo.Source = image;

                SearchWordGrid.Visibility = Visibility.Collapsed;
                WordInfoGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("cuvantul nu a fost gasit.");
            }

        }
        private void SetCategorySearchButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> categoryNames = Dictionary.Words.Select(word => word.Category.ToString()).Distinct().ToList();
            string selectedCategory = SearchCategoryBar.Text;
            if (categoryNames.Contains(SearchCategoryBar.Text))
            {
                List<string> filteredWordNames = Dictionary.Words.Where(word => word.Category.ToString() == selectedCategory).Select(word => word.Name).ToList();
                SearchWordBar.ItemsSource = filteredWordNames;
            }
            else
            {
                MessageBox.Show("Categoria nu exista");
            }
        }
        private void SearchCategoryBar_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox autoCompleteBox = (AutoCompleteBox)sender;
            if (autoCompleteBox.Text == "categoria...")
            {
                autoCompleteBox.Text = string.Empty;
                autoCompleteBox.Foreground = Brushes.Black; // Schimbă culoarea textului la negru (sau la ce dorești)
            }
        }
        private void SearchCategoryBar_LostFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox autoCompleteBox = (AutoCompleteBox)sender;
            if (string.IsNullOrWhiteSpace(autoCompleteBox.Text))
            {
                autoCompleteBox.Text = "categoria...";
                autoCompleteBox.Foreground = Brushes.Gray; // Schimbă culoarea textului la gri (sau la ce dorești)
            }
        }
        private void SearchWordBar_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox autoCompleteBox = (AutoCompleteBox)sender;
            if (autoCompleteBox.Text == "introdu cuvantul...")
            {
                autoCompleteBox.Text = string.Empty;
                autoCompleteBox.Foreground = Brushes.Black;
            }
        }
        private void SearchWordBar_LostFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox autoCompleteBox = (AutoCompleteBox)sender;
            if (string.IsNullOrWhiteSpace(autoCompleteBox.Text))
            {
                autoCompleteBox.Text = "introdu cuvantul...";
                autoCompleteBox.Foreground = Brushes.Gray; // Schimbă culoarea textului la gri (sau la ce dorești)
            }
        }
        private void BackToMenuFromSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchWordGrid.Visibility = Visibility.Collapsed;
            EntryMenuGrid.Visibility = Visibility.Visible;
        }
        private void BackToMenuFromWordInfo_Click(object sender, RoutedEventArgs e)
        {
            WordInfoGrid.Visibility = Visibility.Collapsed;
            EntryMenuGrid.Visibility = Visibility.Visible;
        }
        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = ""; // Default file name
            dialog.DefaultExt = ".png"; // Default file extension
            dialog.Filter = "Imagini (.png, .jpg)|*.png;*.jpg|Toate fișierele (*.*)|*.*"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                MessageBox.Show("s-a citit bine"+ filename);
                WordImageTextBox.Text = filename;
                BitmapImage bitmap = new BitmapImage(new Uri(filename));
                AddWordImg.Source = bitmap;
            }
        }
    }
}