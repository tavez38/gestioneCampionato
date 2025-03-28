using System.IO;
namespace funWriteFile
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }
        static bool checkFile()
        {
            if (File.Exists("C:\\Users\\alunno\\Downloads\\DatiCampionato.txt"))
            {
                return true;
            }
            return false;
        }
        static void writeFile(String testo)
        {
            if (!checkFile()) {
                File.WriteAllText("C:\\Users\\alunno\\Downloads\\DatiCampionato.txt", testo+",");
            }
            else{
                File.AppendAllText("C:\\Users\\alunno\\Downloads\\DatiCampionato.txt", testo + ",");
            }
        }
    }
}
