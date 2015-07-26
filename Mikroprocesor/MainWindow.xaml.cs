using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using System.Management;
using System.IO;
namespace Mikroprocesor
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //REGISTERS AND TEMPORARY REGISTERS (XXt) DECLARATION
        public int AX = 0; public int AXt = 0; 
        public int BX = 0; public int BXt = 0;
        public int CX = 0; public int CXt = 0;
        public int DX = 0; public int DXt = 0;
        public int counter = 0;
        public double allRam;
        //TIMER DECLARATION
        public DispatcherTimer timer = new DispatcherTimer(); int seconds = 0;

        public MainWindow()
        {
            InitializeComponent();
            insert();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1); //TIMER WILL TICK EVERY SEC
        }

        private void insert() //FILL REGISTERS' TEXTBOXES 
        {
            tbAXs.Text = AX.ToString(); bin(AX, tbAX);
            tbBXs.Text = BX.ToString(); bin(BX, tbBX);
            tbCXs.Text = CX.ToString(); bin(CX, tbCX);
            tbDXs.Text = DX.ToString(); bin(DX, tbDX);
        }

        private void timer_Tick(object sender, EventArgs e)//TIMER FUNCTION
        {
            seconds += 1;
            if (seconds == 6)
            {
                AX = AXt; BX = BXt; CX = CXt; DX = DXt;
                bin(AX, tbAX); bin(BX, tbBX); bin(CX, tbCX); bin(DX, tbDX);
                cbOrder.IsEnabled = true;
                cbRegInput.IsEnabled = true;
                cbRegOutput.IsEnabled = true;
                tbRegVal.IsEnabled = true;
                btDoIt.IsEnabled = true;
                insert();
                timer.Stop();
                seconds = 0;
            }
        }

        private void bin(int dec, TextBox text) //DEC TO BIN
        {
            text.Clear();
            for(int i=15;i>=0;i--)
            {
                if (i == 7) text.Text += " ";
                text.Text += Convert.ToString((dec >> i) % 2);
            }
        }

        private void fillCommandLine() //INSERT MICROPROCESSOR COMMANDS TO THE COMMAND LINE
        {
            DateTime timekeeper = DateTime.Now;
            if (cbOrder.SelectedIndex == 0 || cbOrder.SelectedIndex == 1)
            {
                counter += 1;
                tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " " + cbOrder.Text + " " + cbRegInput.Text + " " + tbRegVal.Text + "\r\n";
            }
            if(cbOrder.SelectedIndex == 2)
            {
                counter += 1;
                if(rbIns.IsChecked == true)
                    tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " " + cbOrder.Text + " " + cbRegInput.Text + " " + tbRegVal.Text + "\r\n";
                if(rbReg.IsChecked == true)
                    tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " " + cbOrder.Text + " " + cbRegInput.Text + " " + cbRegOutput.Text + "\r\n";
            }
        }
        //
        //MICROPROCESSOR COMMANDS
        //
        private void ADD() 
        {
            switch(cbRegInput.SelectedIndex)
            {
                case 0: AX = AX + Convert.ToInt32(tbRegVal.Text); bin(AX, tbAX); tbAXs.Text = AX.ToString(); break;
                case 1: BX = BX + Convert.ToInt32(tbRegVal.Text); bin(BX, tbBX); tbBXs.Text = BX.ToString(); break;
                case 2: CX = CX + Convert.ToInt32(tbRegVal.Text); bin(CX, tbCX); tbCXs.Text = CX.ToString(); break;
                case 3: DX = DX + Convert.ToInt32(tbRegVal.Text); bin(DX, tbDX); tbDXs.Text = DX.ToString(); break;
                default: MessageBox.Show("Anyone register selected!"); break;
            }
        }

        private void SUB()
        {
            switch (cbRegInput.SelectedIndex)
            {
                case 0: AX = AX - Convert.ToInt32(tbRegVal.Text); bin(AX, tbAX); tbAXs.Text = AX.ToString(); break;
                case 1: BX = BX - Convert.ToInt32(tbRegVal.Text); bin(BX, tbBX); tbBXs.Text = BX.ToString(); break;
                case 2: CX = CX - Convert.ToInt32(tbRegVal.Text); bin(CX, tbCX); tbCXs.Text = CX.ToString(); break;
                case 3: DX = DX - Convert.ToInt32(tbRegVal.Text); bin(DX, tbDX); tbDXs.Text = DX.ToString(); break;
                default: MessageBox.Show("Anyone register selected!"); break;
            }
        }

        private int MOV_reg() //AUX FUNCTION FOR MOV COMMAND
        {
            int co = 0;
            switch (cbRegInput.SelectedIndex)
            {
                case 0: co = AX; AX = 0; tbAXs.Text = AX.ToString(); bin(AX, tbAX); break;
                case 1: co = BX; BX = 0; tbBXs.Text = BX.ToString(); bin(BX, tbBX); break;
                case 2: co = CX; CX = 0; tbCXs.Text = CX.ToString(); bin(CX, tbCX); break;
                case 3: co = DX; DX = 0; tbDXs.Text = DX.ToString(); bin(DX, tbDX); break;
                default: MessageBox.Show("Anyone register selected!"); break;
            }
            return co;
        }

        private void MOV()
        {
            if (rbReg.IsChecked == true) //REGISTER MODE
            {
                switch (cbRegOutput.SelectedIndex)
                {
                    case 0: AX = MOV_reg(); bin(AX, tbAX); tbAXs.Text = AX.ToString(); break;
                    case 1: BX = MOV_reg(); bin(BX, tbBX); tbBXs.Text = BX.ToString(); break;
                    case 2: CX = MOV_reg(); bin(CX, tbCX); tbCXs.Text = CX.ToString(); break;
                    case 3: DX = MOV_reg(); bin(DX, tbDX); tbDXs.Text = DX.ToString(); break;
                    default: MessageBox.Show("No one register selected"); break; 
                }
            }
            if (rbIns.IsChecked == true) //IMMEDIATE MODE
            {
                switch (cbRegInput.SelectedIndex)
                {
                    case 0: AX = Convert.ToInt32(tbRegVal.Text); bin(AX, tbAX); tbAXs.Text = AX.ToString(); break;
                    case 1: BX = Convert.ToInt32(tbRegVal.Text); bin(BX, tbBX); tbBXs.Text = BX.ToString(); break;
                    case 2: CX = Convert.ToInt32(tbRegVal.Text); bin(CX, tbCX); tbCXs.Text = CX.ToString(); break;
                    case 3: DX = Convert.ToInt32(tbRegVal.Text); bin(DX, tbDX); tbDXs.Text = DX.ToString(); break;
                    default: MessageBox.Show("No one register selected"); break; 
                }
            }
        }
        //
        //MICROPROCESSOR INTERRUPTS
        //
        private void GetCursor() //GET CURSOR POSITION (WORKS ONLY WITHIN PROGRAM'S WINDOW)
        {
            counter += 1;
            DateTime timekeeper = DateTime.Now;
            AXt = AX; BXt = BX; CXt = CX; DXt = DX;
            AX = 3;//(0x10, 0x03) 
            BX = (int)Mouse.GetPosition(this).X;
            CX = (int)Mouse.GetPosition(this).Y;
            DX = 0;
            tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " INT 10h,03h: GET A CURSOR POSITION: " + BX + "x" + CX + "\r\n";
            insert(); bin(AX, tbAX); bin(BX, tbBX); bin(CX, tbCX); bin(DX, tbDX);
            tbRegVal.IsEnabled = false; cbOrder.IsEnabled = false; cbRegInput.IsEnabled = false; cbRegOutput.IsEnabled = false; btDoIt.IsEnabled = false;
            timer.Start();
        }

        private void GetResolution()
        {
            counter += 1;
            DateTime timekeeper = DateTime.Now;
            AXt = AX; BXt = BX; CXt = CX; DXt = DX;
            ManagementObjectSearcher search = new ManagementObjectSearcher("Select * From Win32_VideoController");
            foreach (ManagementObject mObject in search.Get())
            {
                double ramBytes = (Convert.ToDouble(mObject["CurrentHorizontalResolution"]));
                allRam = ramBytes;
            }
            AX = 24; //(0x18)
            BX = Convert.ToInt32(allRam);
            foreach (ManagementObject mObject in search.Get())
            {
                double ramBytes = (Convert.ToDouble(mObject["CurrentVerticalResolution"]));
                allRam = ramBytes;
            }
            CX = Convert.ToInt32(allRam);
            DX = 0;
            tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " INT 18h: GET A SCREEN RESOLUTION: " + BX + "x" + CX + "\r\n";
            insert(); bin(AX, tbAX); bin(BX, tbBX); bin(CX, tbCX); bin(DX, tbDX);
            tbRegVal.IsEnabled = false; cbOrder.IsEnabled = false; cbRegInput.IsEnabled = false; cbRegOutput.IsEnabled = false; btDoIt.IsEnabled = false;
            timer.Start();
        }

        private void GetTime() 
        {
            counter += 1;
            DateTime timekeeper = DateTime.Now;
            DateTime time = DateTime.Now;
            AXt = AX; BXt = BX; CXt = CX; DXt = DX;
            AX = 2; //(0x02)
            BX = time.Hour;
            CX = time.Minute;
            DX = time.Second;
            tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " INT 02h: GET A SYSTEM TIME: " + BX + ":" + CX + ":" + DX + "\r\n";
            insert(); bin(AX, tbAX); bin(BX, tbBX); bin(CX, tbCX); bin(DX, tbDX);
            tbRegVal.IsEnabled = false; cbOrder.IsEnabled = false; cbRegInput.IsEnabled = false; cbRegOutput.IsEnabled = false; btDoIt.IsEnabled = false;
            timer.Start();
        }

        private void GetDate()
        {
            counter += 1;
            DateTime timekeeper = DateTime.Now;
            DateTime date = DateTime.Now;
            AXt = AX; BXt = BX; CXt = CX; DXt = DX;
            AX = 4; //(0x04)
            BX = date.Year;
            CX = date.Month;
            DX = date.Day;
            tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " INT 04h: GET A SYSTEM DATE: " + BX + "." + CX + "." + DX + "\r\n";
            insert(); bin(AX, tbAX); bin(BX, tbBX); bin(CX, tbCX); bin(DX, tbDX);
            tbRegVal.IsEnabled = false; cbOrder.IsEnabled = false; cbRegInput.IsEnabled = false; cbRegOutput.IsEnabled = false; btDoIt.IsEnabled = false;
            timer.Start();
        }

        private void GetFreeRAM()
        {
            counter += 1;
            DateTime timekeeper = DateTime.Now;
            AXt = AX; BXt = BX; CXt = CX; DXt = DX;
            ManagementObjectSearcher search = new ManagementObjectSearcher("Select * From Win32_OperatingSystem");
            foreach (ManagementObject mObject in search.Get())
            {
                double ramBytes = (Convert.ToDouble(mObject["FreePhysicalMemory"]));
                allRam = Math.Round(ramBytes/1024);
            }
            AX = 135; //(0x11, 0x87)
            BX = Convert.ToInt32(allRam);
            CX = 0;
            DX = 0;
            tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " INT 11h, 87h: GET A FREE MEMORY SPACE: " + BX + " Megabytes\r\n";
            insert(); bin(AX, tbAX); bin(BX, tbBX); bin(CX, tbCX); bin(DX, tbDX);
            tbRegVal.IsEnabled = false; cbOrder.IsEnabled = false; cbRegInput.IsEnabled = false; cbRegOutput.IsEnabled = false; btDoIt.IsEnabled = false;
            timer.Start();
        }

        private void GetAllRAM()
        {
            counter += 1;
            DateTime timekeeper = DateTime.Now;
            AXt = AX; BXt = BX; CXt = CX; DXt = DX;
            ManagementObjectSearcher search = new ManagementObjectSearcher("Select * From Win32_ComputerSystem");
            foreach (ManagementObject mObject in search.Get())
            {
                double ramBytes = (Convert.ToDouble(mObject["TotalPhysicalMemory"]));
                allRam = Math.Round(ramBytes / 1048576);
            }
            AX = 136; //(0x11, 0x88)
            BX = Convert.ToInt32(allRam);
            CX = 0;
            DX = 0;
            tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " INT 11h, 88h: GET A MEMORY SIZE: " + BX + " Megabytes\r\n";
            insert(); bin(AX, tbAX); bin(BX, tbBX); bin(CX, tbCX); bin(DX, tbDX);
            tbRegVal.IsEnabled = false; cbOrder.IsEnabled = false; cbRegInput.IsEnabled = false; cbRegOutput.IsEnabled = false; btDoIt.IsEnabled = false;
            timer.Start();
        }

        private void GetFreeDriveSpace()
        {
            counter += 1;
            DateTime timekeeper = DateTime.Now;
            AXt = AX; BXt = BX; CXt = CX; DXt = DX;
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady) tbAssist.Text = (Convert.ToString(drive.TotalFreeSpace / (1024 * 1024 * 1024)));
            }
            AX = 21; //(0x11, 0x15)
            BX = Convert.ToInt32(tbAssist.Text);
            CX = 0;
            DX = 0;
            tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " INT 11h, 15h: GET A FREE DRIVE SPACE: " + BX + " Gigabytes\r\n";
            insert(); bin(AX, tbAX); bin(BX, tbBX); bin(CX, tbCX); bin(DX, tbDX);
            tbRegVal.IsEnabled = false; cbOrder.IsEnabled = false; cbRegInput.IsEnabled = false; cbRegOutput.IsEnabled = false; btDoIt.IsEnabled = false;
            timer.Start();
        }

        private void GetAllDriveSpace()
        {
            counter += 1;
            DateTime timekeeper = DateTime.Now;
            AXt = AX; BXt = BX; CXt = CX; DXt = DX;
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach(DriveInfo drive in drives)
            {
                if (drive.IsReady) tbAssist.Text = (Convert.ToString(drive.TotalSize/(1024*1024*1024)));
            }
            AX = 22; //(0x11, 0x16)
            BX = Convert.ToInt32(tbAssist.Text);
            CX = 0;
            DX = 0;
            tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " INT 11h, 16h: GET A DRIVE SIZE: " + BX + " Gigabytes\r\n";
            insert(); bin(AX, tbAX); bin(BX, tbBX); bin(CX, tbCX); bin(DX, tbDX);
            tbRegVal.IsEnabled = false; cbOrder.IsEnabled = false; cbRegInput.IsEnabled = false; cbRegOutput.IsEnabled = false; btDoIt.IsEnabled = false;
            timer.Start();
        }

        private void GetProcessorClock() // GET PROCESSOR CLOCK FREQ
        {
            counter += 1;
            DateTime timekeeper = DateTime.Now;
            AXt = AX; BXt = BX; CXt = CX; DXt = DX;
            ManagementObjectSearcher search = new ManagementObjectSearcher("Select * From Win32_Processor");
            foreach (ManagementObject mObject in search.Get())
            {
                double ramBytes = (Convert.ToDouble(mObject["MaxClockSpeed"]));
                allRam = ramBytes;
            }
            AX = 98; //(0x12, 0x62)
            BX = Convert.ToInt32(allRam);
            CX = 0;
            DX = 0;
            tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " INT 12h, 62h: GET A PROCESSOR CLOCK FREQUENCY: " + BX + " MHz\r\n";
            insert(); bin(AX, tbAX); bin(BX, tbBX); bin(CX, tbCX); bin(DX, tbDX);
            tbRegVal.IsEnabled = false; cbOrder.IsEnabled = false; cbRegInput.IsEnabled = false; cbRegOutput.IsEnabled = false; btDoIt.IsEnabled = false;
            timer.Start();
        }

        private void GetProcessorBusy()
        {
            counter += 1;
            DateTime timekeeper = DateTime.Now;
            AXt = AX; BXt = BX; CXt = CX; DXt = DX;
            ManagementObjectSearcher search = new ManagementObjectSearcher("Select * From Win32_Processor");
            foreach (ManagementObject mObject in search.Get())
            {
                double ramBytes = (Convert.ToDouble(mObject["LoadPercentage"]));
                allRam = ramBytes;
            }
            AX = 41; //(0x12, 0x29)
            BX = Convert.ToInt32(allRam);
            CX = 0;
            DX = 0;
            tbAnwsers.Text += counter + ". " + timekeeper.Hour + ":" + timekeeper.Minute + " INT 12h, 29h: GET A PROCESSOR USAGE: " + BX + "%\r\n";
            insert(); bin(AX, tbAX); bin(BX, tbBX); bin(CX, tbCX); bin(DX, tbDX);
            tbRegVal.IsEnabled = false; cbOrder.IsEnabled = false; cbRegInput.IsEnabled = false; cbRegOutput.IsEnabled = false; btDoIt.IsEnabled = false;
            timer.Start();
        }
        //
        //END OF INTERRUPTS FUNCTIONS
        //
        private void btDoIt_Click(object sender, RoutedEventArgs e)
        {
            switch(cbInt.SelectedIndex)
            { 
                case 0: if (rbIns.IsChecked == true)
                        {
                            if (tbRegVal.Text.Length == 0)
                                MessageBox.Show("This field cannot be empty!");
                            else
                            {
                                if (cbOrder.SelectedIndex == 0)
                                ADD();
                                if (cbOrder.SelectedIndex == 1)
                                SUB();
                                if (cbOrder.SelectedIndex == 2)
                                MOV();
                            }
                        }
                        if (rbReg.IsChecked == true)
                        {
                            if (cbOrder.SelectedIndex == 0)
                                ADD();
                            if (cbOrder.SelectedIndex == 1)
                                SUB();
                            if (cbOrder.SelectedIndex == 2)
                                MOV();
                        } fillCommandLine();
                        break;
                case 1: GetCursor(); break;
                case 2: GetResolution(); break;
                case 3: GetTime(); break;
                case 4: GetDate(); break;
                case 5: GetFreeRAM(); break;
                case 6: GetAllRAM(); break;
                case 7: GetFreeDriveSpace(); break;
                case 8: GetAllDriveSpace(); break;
                case 9: GetProcessorClock(); break;
                case 10: GetProcessorBusy(); break;
                default: MessageBox.Show("No one interrupt selected"); break;
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)//SHORTCUT KEY TO CALL ABOVE FUNCTION
        {
            if (e.Key.ToString() == "F2") { btDoIt_Click(sender, e); e.Handled = true; }
        }
        //
        //RADIOBOXES
        //
        private void rbReg_Checked(object sender, RoutedEventArgs e) //REGISTER
        {
            try
            {
                cbRegInput.IsEnabled = true;
                cbRegOutput.IsEnabled = true;
                tbRegVal.IsEnabled = true;
            }
            catch{}
        }

        private void rbIns_Checked(object sender, RoutedEventArgs e) //IMMEDIATE
        {
            cbRegInput.IsEnabled = true;
            cbRegOutput.IsEnabled = false;
            tbRegVal.IsEnabled = true;
        }

        private void btSave_Click(object sender, RoutedEventArgs e) //SAVE TO FILE
        {
            File.WriteAllLines("commands.txt", new string[] {tbAnwsers.Text});
            File.WriteAllLines("registers.txt", new string[] { "AX:\r\n" + tbAXs.Text + "\r\nBX:\r\n" + tbBXs.Text + "\r\nCX:\r\n" + tbCXs.Text + "\r\nDX:\r\n" + tbDXs.Text });
            MessageBox.Show("Saved!");
        }

        private void btLoad_Click(object sender, RoutedEventArgs e) //LOAD FROM FILE
        {
            tbAnwsers.Text = File.ReadAllText("commands.txt") + "***\r\n";
            string[] rejestry = File.ReadAllLines("registers.txt");
            AX = Convert.ToInt32(rejestry[1]); BX = Convert.ToInt32(rejestry[3]); CX = Convert.ToInt32(rejestry[5]); DX = Convert.ToInt32(rejestry[7]);
            insert();
            MessageBox.Show("Loaded!");
        }

        private void btClear_Click(object sender, RoutedEventArgs e) //CLEAR OUT COMMAND LINE
        {
            tbAnwsers.Text = "";
        }

        private void tbDelete_Click(object sender, RoutedEventArgs e) //ERASE REGISTERS
        {
            AX = 0; BX = 0; CX = 0; DX = 0;
            insert();
        }

        private void tbRegVal_PreviewTextInput(object sender, TextCompositionEventArgs e) //PREVENT TO TYPE ANYTHING ELSE THAN NUMBERS TO TEXTBOXES
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;

            }
        }
    }
}
