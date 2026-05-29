using System.Text;

public class Rs232Receiver
{

    public string Decode(string bitStream)
    {
        if (string.IsNullOrEmpty(bitStream))
            return string.Empty;

        StringBuilder decodedText = new StringBuilder();

        // ramka z 11 bitów (1 start + 8 dane + 2 stop)
        int frameSize = 11;

        // przechodzenie przez strumien bitow przeskakując co 11 znaków 
        for (int i = 0; i <= bitStream.Length - frameSize; i += frameSize)
        {
            // pobier 1 ramke o dl 11 znaków 
            string frame = bitStream.Substring(i, frameSize);

            // spr bity startu i stopu 
            // Indeks 0 to bit startu, indeksy 9 i 10 to dwa bity stopu
            if (frame[0] == '0' && frame[9] == '1' && frame[10] == '1')
            {
                int asciiValue = 0;

                // oeczyt 8 bitow danych 
                for (int bitIndex = 0; bitIndex < 8; bitIndex++)
                {
                    // jesli odczytany bit to '1', musimy dodać jego wagę do odtwarzanej liczby
                    if (frame[1 + bitIndex] == '1')
                    {
                        // przeesunięcie bitowe 1 w lewo odtwarza nam potęgi dwójki (wagi bitów): 
                        // 1, 2, 4, 8, 16, 32, 64, 128
                        asciiValue += (1 << bitIndex);
                    }
                }
                decodedText.Append((char)asciiValue);
            }
        }

        return decodedText.ToString();
    }
}