using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class ProfanityFilter
{
    // lista przechow wczytane wulgaryzmy
    private List<string> badWords = new List<string>();

    /// <summary>
    /// Wczytuje słownik z podanego pliku tekstowego.
    /// Zakładamy, że w pliku każde słowo znajduje się w nowej linii.
    /// </summary>
    /// 
    public void LoadDictionary(string filePath)
    {
        badWords.Clear(); 

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string word = line.Trim(); // usuwanie spacji po brzegach
                if (!string.IsNullOrEmpty(word))
                {
                    badWords.Add(word);
                }
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono pliku ze słownikiem!");
        }
    }

    /// <summary>
    /// Zamienia wulgarne słowa ze słownika na ciąg gwiazdek.
    /// </summary>
    /// 
    public string CensorText(string input)
    {
        // jesli testk pusty lub nie jest wulgarny to zwracamy tekst
        if (string.IsNullOrEmpty(input) || badWords.Count == 0)
            return input;

        string output = input;

        foreach (string badWord in badWords)
        {
            // \b grniaca slowa
            // Regex.Escape zabezpiuecznie przed znakami specjlanymi
            string pattern = $@"\b{Regex.Escape(badWord)}\b";

            // g
            string replacement = new string('*', badWord.Length);

            // podmiana slow niezwazajac na wielkosc liter
            output = Regex.Replace(output, pattern, replacement, RegexOptions.IgnoreCase);
        }

        return output;
    }
}