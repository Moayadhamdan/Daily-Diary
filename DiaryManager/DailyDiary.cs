using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace DiaryManager
{
    public class DailyDiary
    {
        private readonly string _filePath;

        public DailyDiary(string filePath)
        {
            _filePath = filePath;
        }

        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Daily Diary Manager");
                Console.WriteLine("1. Read Diary");
                Console.WriteLine("2. Add Entry");
                Console.WriteLine("3. Delete Entry");
                Console.WriteLine("4. Count Entries");
                Console.WriteLine("5. Search Entries by Date");
                Console.WriteLine("6. Search Entries by Keyword");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                ReadDiary();
                                break;
                            case 2:
                                AddEntry();
                                break;
                            case 3:
                                DeleteEntry();
                                break;
                            case 4:
                                DisplayCountEntries();
                                break;
                            case 5:
                                SearchEntriesByDate();
                                break;
                            case 6:
                                SearchEntriesByKeyword();
                                break;
                            case 7:
                                exit = true;
                                break;
                            default:
                                Console.WriteLine("Invalid option. Please try again.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        public void ReadDiary()
        {
            try
            {
                var entries = ReadDiaryFile();
                foreach (var entry in entries)
                {
                    Console.WriteLine(entry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading diary: {ex.Message}");
            }
        }

        public void AddEntry()
        {
            try
            {
                Console.Write("Enter the date (YYYY-MM-DD): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                {
                    Console.Write("Enter the content: ");
                    var content = Console.ReadLine();
                    AddEntry(new Entry(date, content));
                    Console.WriteLine("Entry added successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid date format.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding entry: {ex.Message}");
            }
        }

        public void DeleteEntry()
        {
            try
            {
                Console.Write("Enter the date (YYYY-MM-DD) of the entry to delete: ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                {
                    DeleteEntry(date);
                    Console.WriteLine("Entry deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid date format.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting entry: {ex.Message}");
            }
        }

        public void DisplayCountEntries()
        {
            try
            {
                Console.WriteLine($"Total entries: {CountEntries()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting entries: {ex.Message}");
            }
        }

        public void SearchEntriesByDate()
        {
            try
            {
                Console.Write("Enter the date (YYYY-MM-DD) to retrieve entries: ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                {
                    var entries = SearchEntries(date);
                    foreach (var entry in entries)
                    {
                        Console.WriteLine(entry);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date format.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching entries by date: {ex.Message}");
            }
        }

        public void SearchEntriesByKeyword()
        {
            try
            {
                Console.Write("Enter the keyword to search entries: ");
                var keyword = Console.ReadLine();
                var entries = SearchEntries(keyword);
                foreach (var entry in entries)
                {
                    Console.WriteLine(entry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching entries by keyword: {ex.Message}");
            }
        }

        public List<Entry> ReadDiaryFile()
        {
            var entries = new List<Entry>();
            try
            {
                if (File.Exists(_filePath))
                {
                    var lines = File.ReadAllLines(_filePath);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(lines[i]) && DateTime.TryParse(lines[i], out DateTime date))
                        {
                              
                            if (i + 1 < lines.Length && !string.IsNullOrWhiteSpace(lines[i + 1]))
                            {
                                entries.Add(new Entry(date, lines[i + 1]));
                                i++; 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading diary file: {ex.Message}");
            }
            return entries;
        }


        public void AddEntry(Entry entry)
        {
            try
            {
                using (var writer = new StreamWriter(_filePath, true))
                {
                    writer.WriteLine(entry.Date.ToString("yyyy-MM-dd"));
                    writer.WriteLine(entry.Content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding entry to file: {ex.Message}");
            }
        }

        public void DeleteEntry(DateTime date)
        {
            try
            {
                var entries = ReadDiaryFile();
                var updatedEntries = entries.Where(e => e.Date != date).ToList();
                using (var writer = new StreamWriter(_filePath))
                {
                    foreach (var entry in updatedEntries)
                    {
                        writer.WriteLine(entry.Date.ToString("yyyy-MM-dd"));
                        writer.WriteLine(entry.Content);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting entry from file: {ex.Message}");
            }
        }

        public int CountEntries()
        {
            try
            {
                return ReadDiaryFile().Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting entries: {ex.Message}");
                return 0;
            }
        }

        public List<Entry> SearchEntries(DateTime date)
        {
            try
            {
                return ReadDiaryFile().Where(e => e.Date == date).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching entries by date: {ex.Message}");
                return new List<Entry>();
            }
        }

        public List<Entry> SearchEntries(string keyword)
        {
            try
            {
                return ReadDiaryFile().Where(e => e.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching entries by keyword: {ex.Message}");
                return new List<Entry>();
            }
        }
    }
}
