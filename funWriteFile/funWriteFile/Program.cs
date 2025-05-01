using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
        const int MATCH_TOTALI = SQUADRE - 1;
        const int PARAMETRI_MATCH = 5;
        static string[] nomiSquadre = new string[SQUADRE];
        static string[] motti = new string[SQUADRE];
        static string[,] membriSquadra = new string[GIOCATORI, PARAMETRI];
        static string[,] gare = new string[match, PARAMETRI_MATCH];
        static int giorni;
        static int counterSquadre = 0;
        static int incontriGiocati = 0;
        static int match = SQUADRE / 2;
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
                        if (squadraExist(nomeSquadra))
                        {
                            Console.WriteLine("Squadra con lo stesso nome già presente");
                        }
                        else
                        {
                            Console.Write("inserisci il motto della squadra: ");
                            string mottoSquadra = Console.ReadLine();
                            inserisciSquadre(nomeSquadra, mottoSquadra);
                        }
                        break;
                    case 2:
                        string continuo = "";
                        Console.WriteLine("inserisci il nome della squadra: ");
                        String nomeFile = Console.ReadLine();
                        if (checkSquadra(nomeFile))
                        {
                            do
                            {
                                Console.WriteLine("inserisci il nome del giocatore: ");
                                string name = Console.ReadLine();
                                Console.WriteLine("inseirsci il cognome: ");
                                string sName = Console.ReadLine();
                                Console.WriteLine("inserisci il ruolo del giocatore: ");
                                string role = Console.ReadLine();
                                inserisciGiocatore(name, sName, role, nomeFile);
                                Console.Write("vuoi continuare a inserire giocatori della stessa squadra?(S/qualsiasi cosa per annullare): ");
                                continuo = Console.ReadLine();
                            } while (continuo.ToUpper() == "S");
                        }
                        else
                        {
                            Console.WriteLine("Squadra inesistente");
                        }
                        break;
                    case 3:
                        Console.Write("quale squadra vuoi cercare: ");
                        string squadraCercata = Console.ReadLine();
                        bool checkLoad = caricamentoSquadra(squadraCercata);
                        if (checkLoad)
                        {
                            Console.WriteLine("Caricamento squadra in corso ...");
                            Console.WriteLine("1 per visualizzare l'intera squadra 2 per visualizzare il singolo giocatore, qualsiasi altro tasto per annullare");

                            int scegli = int.Parse(Console.ReadLine());
                            if (scegli == 1)
                            {
                                stampaSquadra(squadraCercata);
                            }
                            else if (scegli == 2)
                            {
                                Console.WriteLine("Inserire il cognome del giocatore");
                                String cognome = Console.ReadLine();
                                int resRicerca = cercaGiocatore(cognome);
                                if (resRicerca == -1)
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
                    case 4:
                        if (counterSquadre % 8 == 0 && counterSquadre != 0)
                        {
                            do
                            {
                                Console.Write("che giorno inizia il torneo? ");
                            } while (int.TryParse(Console.ReadLine(), out giorni) && checkGiorni() == false);
                            data();
                            gestioneMatch();
                            torneo();
                        }
                        else
                        {
                            Console.WriteLine("squadre insufficienti");
                        }
                        break;
                }
            } while (scelta > 0 && scelta < 5);
            Console.WriteLine("Vuoi eliminare tutti i file delle squadre che hai creato?(S/qualsiasi tasto)");
            String answer = Console.ReadLine();
            if (answer.ToUpper() == "S")
            {
                deleteFile();
            }
            Console.WriteLine("Uscita in corso...");
        }
        static bool checkInserimento()
        {
            if (counterSquadre < SQUADRE)
            {
                return true;
            }
            return false;
        }
        static void inserisciSquadre(String nome, String motto)
        {
            if (checkInserimento())
            {
                nomiSquadre[counterSquadre] = nome;
                motti[counterSquadre] = motto;
                counterSquadre++;
            }
            else
            {
                Console.WriteLine("iscrizioni chiuse");
            }
        }
        static bool inserisciGiocatore(string nomeGiocatore, string cognomeGiocatore, string ruoloGiocatore, String nomeFile)
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
        static bool checkSquadra(String squadra)
        {
            for (int i = 0; i < SQUADRE; i++)
            {
                if (squadra == nomiSquadre[i])
                {
                    return true;
                }
            }
            return false;
        }
        static bool isFull(string nomeFile)
        {
            int counterInfo = 0;
            if (checkFile(nomeFile))
            {
                String testoFile = File.ReadAllText($"{PERCORSO_FILE}\\{nomeFile}.txt");
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
            }
            return false;
        }
        static bool checkFile(String nomeFile)
        {
            if (File.Exists($"{PERCORSO_FILE}\\{nomeFile}.txt"))
            {
                return true;
            }
            return false;
        }
        static void writeFile(String testo, String nomeFile)
        {
            if (!checkFile(nomeFile))
            {
                File.WriteAllText($"{PERCORSO_FILE}\\{nomeFile}.txt", testo + ",");
            }
            else
            {
                File.AppendAllText($"{PERCORSO_FILE}\\{nomeFile}.txt", testo + ",");
            }
        }
        static bool caricamentoSquadra(String nomeFile)
        {
            String informazione = "";
            int riga = 0;
            int colonna = 0;
            if (!checkFile(nomeFile) && !checkSquadra(nomeFile))
            {
                Console.WriteLine("Squadra non trovata");
                return false;
            }
            else if (!checkFile(nomeFile) && checkSquadra(nomeFile))
            {
                Console.WriteLine("Squadra trovata ma vuota");
                return false;
            }
            else
            {
                String testoFile = File.ReadAllText($"{PERCORSO_FILE}\\{nomeFile}.txt");
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
                                informazione = "";
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
        static void stampaSquadra(String nomeSquadra)
        {
            int indice = cercaSquadra(nomeSquadra);
            int numElenco = 0;
            Console.WriteLine($"Nome squadra: {nomiSquadre[indice]} \t Motto: {motti[indice]}");
            Console.WriteLine("  Nome \t Cognome \t Ruolo");
            for (int i = 0; i < GIOCATORI; i++)
            {
                if (membriSquadra[i, 0] != null)
                {
                    Console.Write(numElenco + " ");
                    for (int j = 0; j < PARAMETRI; j++)
                    {
                        Console.Write(membriSquadra[i, j] + " \t ");
                    }
                    Console.WriteLine("\n");
                    numElenco++;
                }
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
        static void gestioneMatch()
        {
            int squadra1;
            int squadra2;
            for (int i = 0; i < match; i++)
            {
                do
                {
                    Random rnd = new Random();
                    squadra1 = rnd.Next(0, counterSquadre);
                    squadra2 = rnd.Next(0, counterSquadre);
                } while (controlloSquadre(squadra1, squadra2) == false);
                gare[i, 1] = nomiSquadre[squadra1];
                gare[i, 2] = nomiSquadre[squadra2];
                incontriGiocati++;
            }
            stampaTabellone();
        }
        static bool controlloSquadre(int primaSquadra, int secondaSquadra)
        {
            for (int i = 0; i < match; i++)
            {
                if (nomiSquadre[primaSquadra] == gare[i, 1] || nomiSquadre[secondaSquadra] == gare[i, 2] || primaSquadra == secondaSquadra)
                {
                    return false;
                }
            }
            return true;
        }
        static void punteggio()
        {
            int punteggio1;
            int punteggio2;
            for (int i = 0; i < match; i++)
            {
                Random rnd = new Random();
                punteggio1 = rnd.Next(50, 90);
                punteggio2 = rnd.Next(50, 90);
                gare[i, 3] = Convert.ToString(punteggio1);
                gare[i, 4] = Convert.ToString(punteggio2);
            }
        }
        static void torneo()
        {
            do
            {
                for (int i = 0; i < match; i += 2)
                {
                    if (i % 2 == 0)
                    {
                        if (int.Parse(gare[i, 3]) < int.Parse(gare[i, 4]))
                        {
                            gare[i, 3] = gare[i, 4];
                        }
                    }
                    else
                    {
                        if (int.Parse(gare[i + 1, 3]) > int.Parse(gare[i + 1, 4]))
                        {
                            gare[i, 4] = gare[i + 1, 3];
                        }
                        else
                        {
                            gare[i, 4] = gare[i + 1, 4];
                        }
                    }
                    incontriGiocati++;
                }
                match /= 2;
                data();
                punteggio();
                stampaTabellone();
            } while (incontriGiocati != MATCH_TOTALI);
        }
        static void stampaTabellone()
        {
            Console.WriteLine("giorno\tsquadra1\tsquadra2\tpunti1\tpunti2");
            for (int i = 0; i < match; i++)
            {
                for (int j = 0; j < PARAMETRI_MATCH; j++)
                {
                    Console.Write(gare[i, j] + "\t");
                }
                Console.WriteLine("\n");
            }
        }
        static int cercaSquadra(string nameSquad)
        {
            for (int i = 0; i < counterSquadre; i++)
            {
                if (nomiSquadre[i] == nameSquad)
                {
                    return i;
                }
            }
            return -1;
        }
        static void data()
        {
            for (int i = 0; i < match; i++)
            {
                gare[i, 0] = Convert.ToString(giorni);
            }
            giorni++;
        }
        static bool checkGiorni()
        {
            if (giorni <= 0 || giorni > 31)
            {
                return false;
            }
            return true;
        }
        static void deleteFile()
        {
            for (int i = 0; i < SQUADRE; i++)
            {
                if (nomiSquadre[i] != null)
                {
                    File.Delete($"{PERCORSO_FILE}\\{nomiSquadre[i]}.txt");
                }
            }
        }
        static bool squadraExist(String nameSquadra)
        {
            for (int i = 0; i < SQUADRE; i++)
            {
                if (nomiSquadre[i] == nameSquadra)
                {
                    return true;
                }
            }
            return false;
        }
    }
}