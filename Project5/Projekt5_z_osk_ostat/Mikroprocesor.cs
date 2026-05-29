using System;
using System.Collections.Generic;

public class Microprocessor
{
    // Rejestry 8-bitowe
    // get (otrzymaj) - pozwala pobrac wartosc Console.Write(cpu.AH) - wtedy jest get
    // set(ustaw) - pozwala zapisac nowa wartosc gdy piszesz cpu.AH = 15 - wtedy set
    // { get; set; } - mozna wtedy ortzymywac i ustawiac

    public byte AH { get; set; } = 0; 
    public byte AL { get; set; } = 0;
    public byte BH { get; set; } = 0;
    public byte BL { get; set; } = 0;
    public byte CH { get; set; } = 0;
    public byte CL { get; set; } = 0;
    public byte DH { get; set; } = 0;
    public byte DL { get; set; } = 0;

    // Rejestry 16-bitowe
    // ushort bo = 16 bitow

    // tworzenie rejestrow 16 bitow przez laczenie 2 rejestrow 8 bitowych (wszystkie te public ushort)

    public ushort AX
    {
        // get:
        // tworzenie 16 bitowej liczby (00000000 00000000)
        // AH << 8: branie wartosci AH (np. 10101111) i przesuwanie jej w lewo (na najstarsze bity) (10101111 00000000)
        // następuje operacja OR (|) z AL (np. AL to 11110000) czyli
        //
        //    10101111 00000000
        //    00000000 11110000
        //           OR
        //    10101111 11110000

        get => (ushort)((AH << 8) | AL);

        // set:
        // value to jakas liczba 16 bitowa
        // AH = (byte)... -> wartosc rejestru AH jest ustawiana tak, ze wartosc value (16 bit) jest przesuwana w prawo o 8 bitow
        // i nakladana jest maska 0xFF czyli 00000000 11111111 (tutaj maska nie jest konieczne, bo przesuwajac w prawo i tak 8
        // bitow poczatkowych sie zeruje)
        // AL = .. -> AL otrzymuje wartosci z value wraz z maską 0xFF, czyli następuje zero 8 bitow z lewej strony
        // np. 10101111 11110000 to AL = 00000000 11110000
        set
        {
            AH = (byte)((value >> 8) & 0xFF);
            AL = (byte)(value & 0xFF);
        }
    }

    public ushort BX
    {
        get => (ushort)((BH << 8) | BL);
        set
        {
            BH = (byte)((value >> 8) & 0xFF);
            BL = (byte)(value & 0xFF);
        }
    }

    public ushort CX
    {
        get => (ushort)((CH << 8) | CL);
        set
        {
            CH = (byte)((value >> 8) & 0xFF);
            CL = (byte)(value & 0xFF);
        }
    }

    public ushort DX
    {
        get => (ushort)((DH << 8) | DL);
        set
        {
            DH = (byte)((value >> 8) & 0xFF);
            DL = (byte)(value & 0xFF);
        }
    }

    // Licznik insktrucji czyli np robi się 0 linijka, a potem pierwsza, to licznik zwiększa się o jeden (poźniej jest to 
    // opisane

    public int IP { get; set; } = 0;

    // Lista przechowująca linie programu
    public List<string> ProgramLines { get; set; } = new List<string>();


    // Pomocnicza metoda do pobierania wartości z rejestru lub liczby
    private ushort GetValue(string arg)
    {
        arg = arg.Trim().ToUpper();

        // Jeśli argument to rejestr 16-bitowy
        if (arg == "AX") return AX;
        if (arg == "BX") return BX;
        if (arg == "CX") return CX;
        if (arg == "DX") return DX;

        // Jeśli argument to rejestr 8-bitowy
        if (arg == "AH") return AH;
        if (arg == "AL") return AL;
        if (arg == "BH") return BH;
        if (arg == "BL") return BL;
        if (arg == "CH") return CH;
        if (arg == "CL") return CL;
        if (arg == "DH") return DH;
        if (arg == "DL") return DL;

        // jesli to nie rejestr to proba sparsowania
        // Obsluga formatu hex 
        if (arg.EndsWith("H"))
        {
            string hexValue = arg.Substring(0, arg.Length - 1);
            return Convert.ToUInt16(hexValue, 16);
        }

        // Standardowa liczba dziesietna
        return Convert.ToUInt16(arg);
    }

    // metoda zapisujaca wartość do odpowiedniego rejestru na podstawie jego nazwy tekstowej
    private void SetValue(string registerName, ushort value)
    {
        registerName = registerName.Trim().ToUpper();
        switch (registerName)
        {
            case "AX": AX = value; break;
            case "BX": BX = value; break;
            case "CX": CX = value; break;
            case "DX": DX = value; break;
            case "AH": AH = (byte)value; break;
            case "AL": AL = (byte)value; break;
            case "BH": BH = (byte)value; break;
            case "BL": BL = (byte)value; break;
            case "CH": CH = (byte)value; break;
            case "CL": CL = (byte)value; break;
            case "DH": DH = (byte)value; break;
            case "DL": DL = (byte)value; break;
            default: throw new Exception($"Nieprawidłowa nazwa rejestru docelowego: {registerName}");
        }
    }

    // Główna funkcja wykonująca pojedynczą linię kodu
    public void ExecuteLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return;

        // usuniecie komentarzy (jak sie wysyła kod w okienku to moga byc komentarze po semicolonie
        // wiec trzeba je zignorowac

        int semicolonIndex = line.IndexOf(';');
        if (semicolonIndex >= 0)
        {
            line = line.Substring(0, semicolonIndex);
        }

        line = line.Trim();
        if (string.IsNullOrWhiteSpace(line)) return;

        // dzielenie linii na rozkaz i argumenty (rozkaz MOV i argumernt np AH, 00h)
        string[] parts = line.Split(new char[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2) throw new Exception($"Błędna składnia instrukcji: {line}"); 

        string command = parts[0].ToUpper();

        // dzielenie argukmentu po przerciunku (bo tam jest te AH i 00h np to masz przcinek)
        string[] args = parts[1].Split(',');
        if (args.Length != 2) throw new Exception($"Rozkaz {command} wymaga dokładnie 2 argumentów!");


        // 1. wyciągamy nazwę rejestru docelowego (pierwszy argument przed przecinkiem)
        // Trim() obcina przypadkowe i zbędne spacje z początku i końca tekstu.
        string dest = args[0].Trim();      

        // 2. pobieramy wartość argumentu źródłowego (drugi argument po przecinku)
        // Metoda GetValue sprawdzi, czy args[1] to nazwa rejestru (np. "BX"), czy żywa liczba (np. "12h" lub "5"),
        // a następnie zamieni to na czystą wartość liczbową typu ushort (16-bitową).
        ushort srcValue = GetValue(args[1]); 

        // 3. Pobieramy aktualną ("starą") wartość, która w tym momencie fizycznie znajduje się w rejestrze docelowym.
        // Jest to niezbędne dla operacji takich jak ADD lub SUB, ponieważ procesor musi wiedzieć, 
        // od jakiej wartości ma zacząć dodawanie lub odejmowanie.
        ushort destValue = GetValue(dest);   

        // Wykonanie operacji
        switch (command)
        {
            case "MOV":
                SetValue(dest, srcValue);
                break;
            case "ADD":
                SetValue(dest, (ushort)(destValue + srcValue));
                break;
            case "SUB":
                SetValue(dest, (ushort)(destValue - srcValue));
                break;
            default:
                throw new Exception($"Nieznany rozkaz mnemotechniczny: {command}");
        }
    }
}