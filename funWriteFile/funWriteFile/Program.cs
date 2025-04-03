using System.IO;
namespace funWriteFile
{
    internal class Program
    {
       
        static String PERCORSO_FILE = Environment.CurrentDirectory;
        const int GIOCATORI = 12;
        const int PARAMETRI = 3;
        const int SQUADRE = 16;
        const int DATA_MATCH_ESITO = 4;
        static string[] nomiSquadre = new string[GIOCATORI];
        static string[] motti = new string[GIOCATORI];
        static string[,] membriSquadra = new string[GIOCATORI, PARAMETRI];
        static string[,] gare = new string[DATA_MATCH_ESITO, DATA_MATCH_ESITO];
        static int counter = 0;
        static void Main(string[] args)
        {
            int scelta;
            do
            {
                Console.WriteLine("--- MENU ---");
                Console.WriteLine("1. inserisci squadra");
                Console.WriteLine("2. visualizza giocatore");
                Console.WriteLine("qualsiasi tasto: esci");

                if (!int.TryParse(Console.ReadLine(), out scelta))
                {
                    break;
                }
                switch (scelta)
                {
                    case 1:
                        string sceltaGiocatori = "";
                        Console.Write("inserisci il nome della squadra: ");
                        string nomeSquadra = Console.ReadLine();
                        Console.Write("inserisci il motto della squadra: ");
                        string mottoSquadra = Console.ReadLine();
                        inserisciSquadre(nomeSquadra, mottoSquadra);
                        break;
                    case 2:
                        string continuo = "";
                        do
                        {
                            Console.WriteLine("inserisci il nome del giocatore: ");
                            string name = Console.ReadLine();
                            Console.WriteLine("inseirsci il cognome: ");
                            string sName = Console.ReadLine();
                            Console.WriteLine("inserisci il ruolo del giocatore: ");
                            string role = Console.ReadLine();
                            inserisciGiocatore(name, sName, role);
                            Console.Write("vuoi continuare a inserire?(S/qualsiasi cosa): ");
                        } while (continuo.ToUpper() == "S");
                        break;
                }
            } while (scelta > 0 && scelta < 4);
            Console.WriteLine("Uscita in corso...");
        }
        static bool checkInserimento()
        {

            if (counter < SQUADRE)
            {
                return true;
            }
            return false;
        }
        static void inserisciSquadre(String nome, String motto)
        {
            if (checkInserimento())
            {
                for (int i = 0; i < GIOCATORI; i++)
                {
                    if (nomiSquadre[i] == "")
                    {
                        nomiSquadre[i] = nome;
                        motti[i] = motto;
                    }
                }
            }
            else
            {
                Console.WriteLine("iscrizioni chiuse");
            }
        }
        static int checkFirstEmptyRow()
        {
            for (int i = 0; i < GIOCATORI; i++)
            {
                if (membriSquadra[i, 0] == "")
                {
                    return i;
                }
            }
            return -1;
        }
        static void inserisciGiocatore(string nomeGiocatore, string cognomeGiocatore, string ruoloGiocatore)
        {
            for (int i = 0; i < GIOCATORI; i++)
            {
                if (membriSquadra[i, 0] == "")
                {
                    for (int j = 0; j < PARAMETRI; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                membriSquadra[i, j] = nomeGiocatore;
                                break;
                            case 1:
                                membriSquadra[i, j] = cognomeGiocatore;
                                break;
                            case 2:
                                membriSquadra[i, j] = ruoloGiocatore;
                                break;
                        }
                    }
                }
            }
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
