using System.IO;
namespace funWriteFile
{
    internal class Program
    {
        static String PERCORSO_FILE = Environment.CurrentDirectory;
        static void Main(string[] args)
        {
            String testo = "ciao";
            Console.Write("Inserisci il nome del file: ");
            String nome=Console.ReadLine();
            writeFile(testo, nome);
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
        static void readFile(String nomeFile)
        {
           String testoFile = File.ReadAllText($"{PERCORSO_FILE}{nomeFile}.txt");
        }
    }
}
