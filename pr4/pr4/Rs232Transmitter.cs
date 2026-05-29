using System.Text;

public class Rs232Transmitter
{

    public string Encode(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        // stringbuilder bo szybki
        StringBuilder bitStream = new StringBuilder();

        foreach (char c in text)
        {
            // bit startu
            bitStream.Append("0");

            // pobieranie wartosc ASCII znaku
            byte asciiValue = (byte)c;

            // bity od LSB do MSB
            for (int i = 0; i < 8; i++)
            {

                // żeby wyciągnąć wartość konkretnego bitu (0 lub 1)
                int bit = (asciiValue >> i) & 1;
                bitStream.Append(bit);

            }

            // + 2 bity stopu
            bitStream.Append("11");
        }

        return bitStream.ToString();
    }
}