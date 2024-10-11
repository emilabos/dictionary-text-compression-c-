namespace DictionaryEncoding
{
    class Program
    {
        public static string ExtractTextFromFile(string path)
        {
            try
            {
                string text = File.ReadAllText(path);
                return text;
            } catch (FileNotFoundException)
            {
                Console.WriteLine($"Cannot find {path}!");
                return "";
            }
        }

        public static void WriteToLZWFile(string filePath, List<int> compressedData)
        {
            // if (File.Exists(filePath))
            // {

            // }
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                foreach (int code in compressedData)
                {
                    writer.Write((ushort)code);
                }

                Console.WriteLine($"Compressed data to {filePath}");
            }
        }
        

        public static void CompressTextFile(string filePath, ushort dictMaxLength)
        {
            string text = ExtractTextFromFile(filePath);

            if (text.Length > 0)
            {
                Dictionary<string, int> codedDict = new Dictionary<string, int>();
                List<int> compressedFormat = new List<int>();

                List<string> wordList = text.Split(" ").ToList();
                int currentWordCode = 1;
                
                foreach (string word in wordList)
                {
                    if (!codedDict.ContainsKey(word))
                    {
                        codedDict[word] = currentWordCode;
                        currentWordCode++;
                    }
                }

                string currentSequence = "";
                foreach (string word in wordList)
                {
                    string newSequence = currentSequence == "" ? word : currentSequence + " " + word;

                    if (codedDict.ContainsKey(newSequence))
                    {
                        currentSequence = newSequence;
                    }
                    else
                    {
                        compressedFormat.Add(codedDict[currentSequence]);
                        if (codedDict.Count < dictMaxLength)
                        {
                            codedDict[newSequence] = currentWordCode++;
                        }
                        currentSequence = word;
                    }
                }

                if (!string.IsNullOrEmpty(currentSequence) )//this checks if the string exists basically
                {
                    compressedFormat.Add(codedDict[currentSequence]);
                }

                try
                {
                    WriteToLZWFile(filePath.Replace(".txt", ".lzw"), compressedFormat);
                    
                    long sizeNew = new System.IO.FileInfo(filePath.Replace(".txt", ".lzw")).Length; // taken from stack overflow
                    long sizeOriginal = new System.IO.FileInfo(filePath).Length; // taken from stack overflow

                    Console.WriteLine($"Original file size {sizeOriginal} KB");
                    Console.WriteLine($"Compressed file size {sizeNew} KB");
                    Console.WriteLine($"{Math.Round(Convert.ToDouble(sizeNew)/Convert.ToDouble(sizeOriginal)*100, 2)}% reduction in file size");

                }
                catch
                {
                    Console.WriteLine("Error writing compressed data to .lzw, pls try again");
                }
            }
        }


        public static void Main(string[] args)
        {
            CompressTextFile("data.txt", 3);
        }
    }
}