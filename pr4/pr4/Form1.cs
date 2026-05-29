using System;
using System.IO;
using System.Windows.Forms;

namespace pr4
{
    public partial class Form1 : Form
    {

        private ProfanityFilter filter;
        private Rs232Transmitter transmitter;
        private Rs232Receiver receiver;

        public Form1()
        {
            InitializeComponent();

            filter = new ProfanityFilter();
            transmitter = new Rs232Transmitter();
            receiver = new Rs232Receiver();

            string dictionaryPath = "slownik.txt";

            // Jeśli plik nie istnieje, tworzymy go z testową zawartością
            if (!File.Exists(dictionaryPath))
            {
                File.WriteAllLines(dictionaryPath, new string[] { "cholera", "kurcze", "motyla noga" });
            }

            filter.LoadDictionary(dictionaryPath);
        }

        // po kliknieciu wyslij
        private void btnSend_Click(object sender, EventArgs e)
        {

            // pobranie tekstu wpisanwego
            string originalText = txtInput.Text;


            // filtr wulgaryzmow
            string cleanText = filter.CensorText(originalText);


            // kodowanie rs232
            string bitStream = transmitter.Encode(cleanText);


            // wyswietlanie lancucha
            txtBitStream.Text = bitStream;



            // dekodowanie ramki na tekst
            string receivedText = receiver.Decode(bitStream);


            txtOutput.Text = receivedText;
        }
    }
}