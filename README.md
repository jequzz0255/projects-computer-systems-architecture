# Academic Windows Forms Projects (C#) / Projekty Akademickie

A professional collection of desktop applications developed in C# using Windows Forms as part of the university laboratory curriculum. These applications demonstrate concepts of low-level hardware simulation, serial communication, and object-oriented lifecycle management.

Kolekcja zaawansowanych aplikacji okienkowych opracowanych w języku C# (Windows Forms) w ramach laboratoriów akademickich. Projekty demonstrują praktyczne zastosowanie operacji bitowych, emulacji protokołów sieciowych/szeregowych oraz obiektowego zarządzania stanem procesów.

---

## 🚀 Featured Projects / Zawartość Repozytorium

### 1. x86 Microprocessor Simulator (Zadanie 5)
* **EN:** An educational tool that simulates a 16-bit x86 microprocessor architecture. It emulates general-purpose registers (`AX`, `BX`, `CX`, `DX`) and their 8-bit components (`AH`, `AL`, etc.) using low-level bitwise operations (`<<`, `>>`, `&`, `|`). It parses assembly instructions (`MOV`, `ADD`, `SUB`) and supports step-by-step code execution with Instruction Pointer (`IP`) tracking, as well as saving/loading source code from files.
* **PL:** Edukacyjny symulator 16-bitowej architektury procesora x86. Odwzorowuje rejestry ogólnego przeznaczenia (`AX`, `BX`, `CX`, `DX`) oraz ich 8-bitowe połówki (`AH`, `AL` itd.) przy użyciu operacji bitowych. Obsługuje pracę krokową z licznikiem instrukcji (`IP`), automatyczne parsowanie kodu assemblera oraz zapis/odczyt plików `.txt`/`.asm`.

### 2. RS-232 Emulator with Profanity Filter (Zadanie 4)
* **EN:** A software simulator of asynchronous serial communication following the UART/RS-232 standard. It decomposes ASCII text into individual 11-bit transmission frames (1 start bit `0`, 8 data bits from LSB to MSB, and 2 stop bits `11`) and reconstructs it on the receiver side. It features an integrated regular expression (`Regex`) profanity filter module that automatically censors forbidden words from an external dictionary file before transmission.
* **PL:** Symulator asynchronicznej transmisji szeregowej zgodnej ze standardem UART/RS-232. Dokonuje dekompozycji znaków ASCII do 11-bitowych ramek transmisyjnych (1 bit startu `0`, 8 bitów danych od LSB do MSB, 2 bity stopu `11`) oraz przeprowadza ich ponowne dekodowanie. Zawiera zaawansowany moduł cenzury wulgaryzmów oparty o wyrażenia regularne (`Regex`) oraz zewnętrzny plik słownika.

### 3. Pigeon Flight Racing Log & Manager (Zadanie 6 - Autorski)
* **EN:** An object-oriented system designed to track and evaluate training and racing flights for sport pigeons. It implements a strict state-machine logic via flags to monitor bird presence (whether the bird is in the loft or currently in flight) and uses integrated LINQ queries for real-time GUI filtering. Features automated kinematic calculations using `TimeSpan` to dynamically determine flight duration and average speed (km/h) upon registered arrival.
* **PL:** Obiektowy system wspomagający rejestrację hodowlaną oraz automatyczne rozliczanie lotów konkursowych gołębi pocztowych. Wykorzystuje architekturę maszyny stanów do kontroli statusu ptaków (w locie / w gołębniku) oraz zapytania LINQ do dynamicznego filtrowania widoków GUI. Automatycznie wyznacza czas przelotu (`TimeSpan`) oraz wylicza średnią prędkość (km/h) w momencie rejestracji powrotu.

---

## 🛠️ Core Technical Features / Kluczowe Rozwiązania

### Low-level Bit Manipulation / Operacje Bitowe (x86 & RS-232)
* **Register Binding:** Combines two 8-bit registers into one 16-bit virtual property via shifted bitwise OR: `(AH << 8) | AL`.
* **Bit Extraction:** Decomposes bytes into individual bits for serial transmission using mask alignment: `(asciiValue >> i) & 1`.
* **Bit Reconstruction:** Rebuilds ASCII characters by summing bit weights using bitwise left shift: `asciiValue += (1 << bitIndex)`.

### Advanced String Processing / Zaawansowane Przetwarzanie Tekstu (RS-232)
* **Regex Word Boundaries:** Uses word boundary anchors (`\b`) to eliminate partial matches and prevent over-censoring (e.g., ensuring safe words aren't accidentally triggered by bad word substrings).

### Precision Time Tracking / Precyzyjny Pomiar Czasu (Pigeon Manager)
* **TimeSpan Kinematics:** Utilizes system-level chronological operations to guarantee accurate flight time calculations across multi-day time frames.

---

## 🖥️ Technologies Used / Użyte Technologie
* **Language:** C#
* **Framework:** .NET Framework / Windows Forms (Desktop GUI)
* **Data Pipelines:** LINQ (Language Integrated Query)
* **Text Parsing:** Regular Expressions (Regex), Substring tokenization, File I/O
