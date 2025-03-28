using System.IO;
namespace funWriteFile
{
    internal class Program
    {
        static String PERCORSO_FILE = Environment.CurrentDirectory;
        static void Main(string[] args)
        {
           
        }
        static bool checkFile(String nomeFile)
        {
            if (File.Exists($"{PERCORSO_FILE}{nomeFile}.txt"))
            {
                return true;
            }
            return false;
        }
        static void writeFile(String testo, String nomeFile)
        {
            if (!checkFile(nomeFile)) {
                File.WriteAllText($"{PERCORSO_FILE}{nomeFile}.txt", testo+",");
            }
            else{
                File.AppendAllText($"{PERCORSO_FILE}{nomeFile}.txt", testo + ",");
            }
        }
        static String readFileSinglePlayer(String nomeFile)
        {
            int countCharSep = 0;
            String informazione = "";
           String testoFile = File.ReadAllText($"{PERCORSO_FILE}{nomeFile}.txt");
            for (int i = 0; i < testoFile.Length; i++)
            {
                if (countCharSep < 3)
                {
                    if (testoFile[i] != ',')
                    {
                        informazione = informazione+ testoFile[i];
                    }
                    else
                    {
                        informazione = informazione + "\t";
                        countCharSep++;
                    }
                }
                else
                {
                    break;
                }
            }
            return informazione;
        }
    }
}
