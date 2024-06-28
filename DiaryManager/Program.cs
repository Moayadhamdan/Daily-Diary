using System;
using System.IO;

namespace DiaryManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory,"mydiary.txt");
            var diary = new DailyDiary(filePath);
            diary.Run();
        }
    }
}
