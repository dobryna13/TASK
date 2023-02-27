using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string csvFilePath = "transactions.csv"; // шлях до файлу CSV
        string dateFormat = "MM/dd/yyyy"; // формат дати

        // Створення делегатів
        Func<string, DateTime> getDate = s => DateTime.ParseExact(s.Split(',')[0], dateFormat, null);
        Func<string, double> getAmount = s => double.Parse(s.Split(',')[1]);
        Action<DateTime, double> printDailyTotal = (date, total) => Console.WriteLine("{0}: {1}", date.ToString(dateFormat), total.ToString("C"));

        // Зчитування рядків з файлу CSV
        List<string> lines = File.ReadAllLines(csvFilePath).ToList();

        // Обчислення загальної суми грошей, витраченої за кожен день
        Dictionary<DateTime, double> dailyTotals = new Dictionary<DateTime, double>();
        foreach (string line in lines)
        {
            DateTime date = getDate(line);
            double amount = getAmount(line);
            if (dailyTotals.ContainsKey(date))
            {
                dailyTotals[date] += amount;
            }
            else
            {
                dailyTotals[date] = amount;
            }
        }

        // Відображення загальної суми грошей, витраченої за кожен день
        int i = 0;
        string newCsvFilePath = string.Format("transactions-{0}.csv", i);
        StreamWriter writer = new StreamWriter(newCsvFilePath);
        foreach (KeyValuePair<DateTime, double> dailyTotal in dailyTotals)
        {
            printDailyTotal(dailyTotal.Key, dailyTotal.Value);
            writer.WriteLine("{0},{1}", dailyTotal.Key.ToString(dateFormat), dailyTotal.Value.ToString());
            if (++i % 10 == 0)
            {
                writer.Close();
                newCsvFilePath = string.Format("transactions-{0}.csv", i);
                writer = new StreamWriter(newCsvFilePath);
            }
        }
        writer.Close();
    }
}

