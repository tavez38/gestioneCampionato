using System.IO;
namespace funWriteFile
{
    internal class Program
    {
        static String [,] membriSquadre = new String[12, 3];
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
        static void caricamentoSquadra(String nomeFile)
        {
            String informazione="";
            int countCharSep = 0;
            int riga = 0;
            int colonna = 0;
            String testoFile = File.ReadAllText($"{PERCORSO_FILE}{nomeFile}.txt");
            if (riga < 12)
            {
                for (int i = 0; i < testoFile.Length; i++)
                {
                    if (colonna < 3)
                    {
                        if (testoFile[i] != ',')
                        {
                            informazione = informazione + testoFile[i];
                        }
                        else
                        {
                            membriSquadre[riga, colonna] = informazione;
                            countCharSep++;
                            colonna++;
                        }
                    }
                    else
                    {
                        riga++;
                        colonna = 0;
                    }
                }
            }

        }
    }
}
