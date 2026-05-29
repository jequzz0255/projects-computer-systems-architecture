using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace Projekt5_z_osk_ostat
{
    public partial class Form1 : Form
    {

        private Microprocessor cpu = new Microprocessor();


        public Form1()
        {
            InitializeComponent();
            UpdateRegisterDisplay();
        }

        private void UpdateRegisterDisplay()
        {
            // rejestry 16-bitowe w formacie Hex (X4 oznacza 4 cyfry szesnastkowe, np. 00FF)
            lblAX.Text = $"AX: {cpu.AX:X4}h";
            lblBX.Text = $"BX: {cpu.BX:X4}h";
            lblCX.Text = $"CX: {cpu.CX:X4}h";
            lblDX.Text = $"DX: {cpu.DX:X4}h";

            // rejestry 8-bitowe (X2 oznacza 2 cyfry szesnastkowe, np. FF)
            lblAH.Text = $"AH: {cpu.AH:X2}h";
            lblAL.Text = $"AL: {cpu.AL:X2}h";
            lblBH.Text = $"BH: {cpu.BH:X2}h";
            lblBL.Text = $"BL: {cpu.BL:X2}h";
            lblCH.Text = $"CH: {cpu.CH:X2}h";
            lblCL.Text = $"CL: {cpu.CL:X2}h";
            lblDH.Text = $"DH: {cpu.DH:X2}h";
            lblDL.Text = $"DL: {cpu.DL:X2}h";

            // Licznik instrukcji
            lblIP.Text = $"Aktualna linia (IP): {cpu.IP}";
        }

        private void LoadCodeToProcessor()
        {
            cpu.ProgramLines.Clear();
            // rozbijamy zawartość txtCode na pojedyncze linie
            string[] lines = txtCode.Lines;
            foreach (string line in lines)
            {
                // wrzucamy tylko niepuste linie
                if (!string.IsNullOrWhiteSpace(line))
                {
                    cpu.ProgramLines.Add(line.Trim());
                }
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblBH_Click(object sender, EventArgs e)
        {

        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblBX_Click(object sender, EventArgs e)
        {

        }

        private void lblCX_Click(object sender, EventArgs e)
        {

        }

        private void lblDX_Click(object sender, EventArgs e)
        {

        }

        private void lblAH_Click(object sender, EventArgs e)
        {

        }

        private void lblAL_Click(object sender, EventArgs e)
        {

        }

        private void lblBL_Click(object sender, EventArgs e)
        {

        }

        private void lblCH_Click(object sender, EventArgs e)
        {

        }

        private void lblCL_Click(object sender, EventArgs e)
        {

        }

        private void lblDH_Click(object sender, EventArgs e)
        {

        }

        private void lblDL_Click(object sender, EventArgs e)
        {

        }

        private void lblIP_Click(object sender, EventArgs e)
        {

        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            try
            {
                // Jeśli program nie był jeszcze wczytany lub IP jest na początku
                if (cpu.IP == 0)
                {
                    LoadCodeToProcessor();
                }

                // Sprawdzamy, czy są jeszcze jakieś linie do wykonania
                if (cpu.IP < cpu.ProgramLines.Count)
                {
                    string currentLine = cpu.ProgramLines[cpu.IP];

                    // Wykonujemy bieżącą linię
                    cpu.ExecuteLine(currentLine);

                    // Zwiększamy licznik instrukcji (wskazujemy na kolejną linię)
                    cpu.IP++;

                    // Odświeżamy GUI
                    UpdateRegisterDisplay();
                }
                else
                {
                    MessageBox.Show("Koniec programu! Wszystkie instrukcje zostały wykonane.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd w linii {cpu.IP}: {ex.Message}", "Błąd wykonania", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRunAll_Click(object sender, EventArgs e)
        {
            try
            {
                LoadCodeToProcessor();

                // Wykonujemy w pętli od aktualnego miejsca IP do końca programu
                while (cpu.IP < cpu.ProgramLines.Count)
                {
                    cpu.ExecuteLine(cpu.ProgramLines[cpu.IP]);
                    cpu.IP++;
                }

                UpdateRegisterDisplay();
                MessageBox.Show("Program wykonany w całości!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateRegisterDisplay();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Pliki tekstowe (*.txt)|*.txt|Pliki assemblera (*.asm)|*.asm";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, txtCode.Text);
                    MessageBox.Show("Program zapisany pomyślnie!", "Zapis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Zerujemy rejestry
            cpu.AX = 0;
            cpu.BX = 0;
            cpu.CX = 0;
            cpu.DX = 0;
            cpu.IP = 0;
            cpu.ProgramLines.Clear();

            UpdateRegisterDisplay();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Pliki tekstowe (*.txt)|*.txt|Pliki assemblera (*.asm)|*.asm";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtCode.Text = File.ReadAllText(ofd.FileName);
                    btnReset_Click(null, null); // Automatyczny reset stanu przy wczytaniu nowego kodu
                    MessageBox.Show("Program wczytany pomyślnie!", "Odczyt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
