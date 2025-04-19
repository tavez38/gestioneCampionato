using System.IO;
using System.Runtime.CompilerServices;
namespace funWriteFile
{
    internal class Program
    {
       
        static String PERCORSO_FILE = Environment.CurrentDirectory;
        const int GIOCATORI = 12;
        const int PARAMETRI = 3;
        const int SQUADRE = 16;
        const int DATA_MATCH_ESITO = 4;
        const int INFO_MAX = 36;
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
                Console.WriteLine("2. inserisci giocatore di una squadra");
                Console.WriteLine("3. cerca una squadra o il giocatore di una squadra");
                Console.WriteLine("4. visualizza il tabellone dei match");
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
                        Console.WriteLine("inserisci il nome della squadra: ");
                        String nomeFile=Console.ReadLine();
                        do
                        {
                            Console.WriteLine("inserisci il nome del giocatore: ");
                            string name = Console.ReadLine();
                            Console.WriteLine("inseirsci il cognome: ");
                            string sName = Console.ReadLine();
                            Console.WriteLine("inserisci il ruolo del giocatore: ");
                            string role = Console.ReadLine();
                            inserisciGiocatore(name, sName, role, nomeFile);
                            Console.Write("vuoi continuare a inserire giocatori della stessa squadra?(S/qualsiasi cosa): ");
                        } while (continuo.ToUpper() == "S");
                        break;
                    case 3:
                        Console.Write("quale squadra vuoi cercare: ");
                        string squadraCercata = Console.ReadLine();
                        bool checkLoad = caricamentoSquadra(squadraCercata);
                        if (!checkLoad)
                        {
                            Console.WriteLine("Squadra non trovata");
                        }
                        else
                        {
                            Console.WriteLine("Caricamento squadra in corso ...");
                            Console.WriteLine("1 per visualizzare l'intera squadra 2 per visualizzare il singolo giocatore, qualsiasi altro tasto per annullare");
                            
                            int scegli = int.Parse(Console.ReadLine());
                            if (scegli == 1)
                            {
                                stampaSquadra();
                            }
                            else if (scegli == 2)
                            {
                                Console.WriteLine("Inserire il cognome del giocatore");
                                String cognome = Console.ReadLine();
                                int resRicerca = cercaGiocatore(cognome);
                                if(resRicerca == -1)
                                {
                                    Console.WriteLine("giocatore non trovato");
                                }
                                else
                                {
                                    for (int i = 0; i < PARAMETRI; i++)
                                    {
                                        Console.Write($"{membriSquadra[resRicerca, i]}, ");
                                    }
                                }
                            }
                        }
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
        static bool inserisciGiocatore(string nomeGiocatore, string cognomeGiocatore, string ruoloGiocatore,String nomeFile)
        {
            if (!isFull(nomeFile))
            {
                for (int j = 0; j < PARAMETRI; j++)
                {
                    switch (j)
                    {
                        case 0:
                            writeFile(nomeGiocatore, nomeFile);
                            break;
                        case 1:
                            writeFile(cognomeGiocatore, nomeFile);
                            break;
                        case 2:
                            writeFile(ruoloGiocatore, nomeFile);
                            break;
                    }
                }
                return true;
            }
            return false;
              
        }
        static bool isFull(string nomeFile) {
            int counterInfo = 0;
            String testoFile = File.ReadAllText($"{PERCORSO_FILE}{nomeFile}.txt");
            for (int i = 0; i < testoFile.Length; i++)
            {
                if (counterInfo == INFO_MAX)
                {
                    return true;
                }
                else
                {
                    if (testoFile[i] == ',')
                    {
                        counterInfo++;
                    }
                }
            }
            return false;
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
        static bool caricamentoSquadra(String nomeFile) { 
            String informazione = "";
            int riga = 0;
            int colonna = 0;
            if (!checkFile(nomeFile))
            {
                return false;
            }
            else
            {
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
                                membriSquadra[riga, colonna] = informazione;

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
                return true;
            }
        }
        static void stampaSquadra()
        {
            Console.WriteLine("Nome \t Cognome \t Ruolo");
            for (int i = 0; i < GIOCATORI; i++)
            {
                for (int j = 0; j < PARAMETRI; j++)
                {
                    Console.Write(membriSquadra[i,j]+"\t");
                }
                Console.WriteLine("\n");
            }
        }
        static int cercaGiocatore(String cognome)
        {
            for (int i = 0; i < GIOCATORI; i++)
            {
                if (membriSquadra[i, 1] == cognome)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
