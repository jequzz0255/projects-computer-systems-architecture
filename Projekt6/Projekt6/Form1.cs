using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Projekt6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Pigeon
        {
            public string Id { get; set; }
            public string Color { get; set; }
            public string Gender { get; set; }
            public bool IsInFlight { get; set; }
        }

        public class FlightLog
        {
            public string PigeonId { get; set; }
            public string Location { get; set; }
            public double DistanceKm { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime? ArrivalTime { get; set; }
            public double SpeedKmh { get; set; }
        }


        // listy przechowujace zmienne
        private List<Pigeon> allPigeons = new List<Pigeon>();
        private List<FlightLog> flightHistory = new List<FlightLog>();

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // lista plci
            cmbGender.Items.Add("Samiec");
            cmbGender.Items.Add("Samica");

            // domyslny pierwszy
            cmbGender.SelectedIndex = 0;

            tabPage1.Text = "Dodaj gołębia";
            tabPage2.Text = "Planowanie lotu";
            tabPage3.Text = "Centrum przylotów";
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // dodawanie golebia
        private void button1_Click(object sender, EventArgs e)
        {
            // czy pole nie jest puste
            if (string.IsNullOrWhiteSpace(txtRingNumber.Text))
            {
                MessageBox.Show("Wpisz numer obrączki!");
                return;
            }

            var newPigeon = new Pigeon
            {
                Id = txtRingNumber.Text,
                Color = txtColor.Text,
                Gender = cmbGender.SelectedItem?.ToString() ?? "Nieznana", //jesli brak wybranej plci to nieznana
                IsInFlight = false
            };

            allPigeons.Add(newPigeon);

            // odswiezenie tabeli
            dgvPigeons.DataSource = null;
            dgvPigeons.DataSource = allPigeons;
            RefreshPigeonsLists();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RefreshPigeonsLists()
        {
            listBox1.Items.Clear();

            // musza byc tylko golebie ktore nie sa w locie
            var availablePigeons = allPigeons.Where(p => !p.IsInFlight).ToList();

            foreach (var pigeon in availablePigeons)
            {
                listBox1.Items.Add(pigeon.Id); // Dodajemy numer obrączki do listy wyboru
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 1. pobieranie danych
            string location = textBox3.Text; // Miejscowość lotowania
            double distance = (double)numericUpDown1.Value; // Dystans w km
            DateTime startTime = dateTimePicker1.Value; // Czas rozpoczęcia lotu

            
            if (string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("Proszę podać miejscowość lotowania!");
                return;
            }
            if (distance <= 0)
            {
                MessageBox.Show("Dystans musi być większy niż 0 km!");
                return;
            }

            // uzytkownik musi wybrac goleba
            if (listBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Zaznacz przynajmniej jednego gołębia na liście do lotu!");
                return;
            }

            // 2. petla przez elementy zaznaczone w liscie
            
            var selectedIds = listBox1.SelectedItems.Cast<string>().ToList(); //kopia danych

            foreach (string pigeonId in selectedIds)
            {
                
                var pigeon = allPigeons.FirstOrDefault(p => p.Id == pigeonId);

                if (pigeon != null && !pigeon.IsInFlight)
                {
                    
                    pigeon.IsInFlight = true;

                    
                    var newFlight = new FlightLog
                    {
                        PigeonId = pigeon.Id,
                        Location = location,
                        DistanceKm = distance,
                        StartTime = startTime,
                        ArrivalTime = null, 
                        SpeedKmh = 0 
                    };

                    flightHistory.Add(newFlight);
                }
            }

            
            RefreshFlyingPigeonsComboBox(); 
            RefreshPigeonsLists();          

            MessageBox.Show($"Wypuszczono gołębie na lot z miejscowości {location}!");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // po to aby w 3 zakladce przyloty w liscie rozwijanej byly tylko goleb
        // ktore faktycznie sa lotowane

        private void RefreshFlyingPigeonsComboBox()
        {
            cmbFlyingPigeons.Items.Clear(); 

            
            var flyingPigeons = allPigeons.Where(p => p.IsInFlight).ToList();

            foreach (var pigeon in flyingPigeons)
            {
                cmbFlyingPigeons.Items.Add(pigeon.Id); 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // pobierany jest numer golebie z comboboxa
            string selectedId = cmbFlyingPigeons.SelectedItem?.ToString();
            if (selectedId == null) return;

            // znaajdź log tego lotu, który nie ma jeszcze czasu przylotu
            var currentFlight = flightHistory.FirstOrDefault(f => f.PigeonId == selectedId && f.ArrivalTime == null);

            if (currentFlight != null)
            {
                // zapisz aktualny czas systemowy jako moment przylotu
                currentFlight.ArrivalTime = DateTime.Now;

                // oblliczanie parametru lotu
                TimeSpan flightTime = currentFlight.ArrivalTime.Value - currentFlight.StartTime;
                double totalMinutes = flightTime.TotalMinutes;

                // prędkość w km/h
                if (totalMinutes > 0)
                {
                    currentFlight.SpeedKmh = (currentFlight.DistanceKm / totalMinutes) * 60;
                }

                // golab znowu w bazie danych jako nie w locie
                var pigeon = allPigeons.FirstOrDefault(p => p.Id == selectedId);
                if (pigeon != null) pigeon.IsInFlight = false;

                // odswiezanie widokow
                RefreshFlyingPigeonsComboBox();
                dgvHistory.DataSource = null;
                dgvHistory.DataSource = flightHistory.Where(f => f.ArrivalTime != null).ToList();
                RefreshPigeonsLists();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
