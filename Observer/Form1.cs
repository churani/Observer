using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Observer
{
    public partial class Form1 : Form
    {
        IniFile MyIni;

        // exe 제거된 이름
        string exitProcess1 = ""; //종료시킬 프로그램
        string exitProcess2 = ""; //종료시킬 프로그램
        // 파일이름 포함 전체경로
        string startProcessPath1 = ""; //시작할 프로그램
        string startProcessPath2 = ""; //시작할 프로그램

        int programPeriodRestart1 = 0;
        int programPeriodRestart2 = 0;

        //const string userRoot = "HKEY_CURRENT_USER";
        string keyName = "HKEY_CURRENT_USER\\SOFTWARE\\CleanManager";

        string itemSelected = "";
        int programPeriod = 5;
        int[] programWeek = new int[7];
        string programStartTime = "";
        bool realCose = false;
        bool shown = false;
        bool restartFlag = false;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsRunningAppCheck() == false)
                {
                    MyIni = new IniFile("C:\\CleanManager\\Observer.ini");

                    programPeriodRestart1 = 0;
                    programPeriodRestart2 = 0;

                    string dbData = "";
                    dbData = MyIni.Read("period");

                    if (dbData == "")
                    {
                        MyIni.Write("period", "5");
                        programPeriod = 5;

                        MyIni.Write("sun", "0");
                        programWeek[0] = 0;
                        MyIni.Write("mon", "1");
                        programWeek[1] = 1;
                        MyIni.Write("tue", "0");
                        programWeek[2] = 0;
                        MyIni.Write("wed", "1");
                        programWeek[3] = 1;
                        MyIni.Write("thu", "0");
                        programWeek[4] = 0;
                        MyIni.Write("fri", "1");
                        programWeek[5] = 1;
                        MyIni.Write("sat", "0");
                        programWeek[6] = 0;

                        MyIni.Write("time", "09:00:00");
                        programStartTime = "09:00:00";

                        startProcessPath1 = @"C:\CleanManager\CleanManager.exe";
                        if (File.Exists(startProcessPath1))
                        {
                            textBox1.Text = startProcessPath1;
                            exitProcess1 = "CleanManager";
                            keyName = "HKEY_CURRENT_USER\\SOFTWARE\\CleanManager";
                            listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} CleanManager 파일이 존재합니다...🥲💦");
                        }
                        else
                        {
                            startProcessPath1 = "";
                            exitProcess1 = "";
                            programPeriodRestart1 = 0;
                            textBox1.Text = "N/A";
                            listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} CleanManager 파일이 존재하지 않습니다...🥲💦");
                        }

                        startProcessPath2 = @"C:\CleanManager\CleanManagerKiosk.exe";
                        if (File.Exists(startProcessPath2))
                        {
                            Console.WriteLine("파일이 존재합니다!✨");

                            startProcessPath2 = "C:\\CleanManager\\CleanManagerKiosk.exe";
                            textBox2.Text = startProcessPath2;
                            exitProcess2 = "CleanManagerKiosk";
                            keyName = "HKEY_CURRENT_USER\\SOFTWARE\\CleanManagerKiosk";
                            listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} CleanManagerKiosk 파일이 존재합니다...🥲💦");
                        }
                        else
                        {
                            startProcessPath2 = "";
                            exitProcess2 = "";
                            programPeriodRestart2 = 0;
                            textBox2.Text = "N/A";
                            listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} CleanManagerKiosk 파일이 존재하지 않습니다...🥲💦");
                        }
                        
                    }
                    else
                    {
                        programPeriod = Int32.Parse(dbData.ToString());

                        dbData = MyIni.Read("sun");
                        programWeek[0] = Int32.Parse(dbData.ToString());
                        dbData = MyIni.Read("mon");
                        programWeek[1] = Int32.Parse(dbData.ToString());
                        dbData = MyIni.Read("tue");
                        programWeek[2] = Int32.Parse(dbData.ToString());
                        dbData = MyIni.Read("wed");
                        programWeek[3] = Int32.Parse(dbData.ToString());
                        dbData = MyIni.Read("thu");
                        programWeek[4] = Int32.Parse(dbData.ToString());
                        dbData = MyIni.Read("fri");
                        programWeek[5] = Int32.Parse(dbData.ToString());
                        dbData = MyIni.Read("sat");
                        programWeek[6] = Int32.Parse(dbData.ToString());

                        dbData = MyIni.Read("time");
                        programStartTime = dbData.ToString();
                        
                        dbData = MyIni.Read("name1");
                        exitProcess1 = dbData.ToString();
                        dbData = MyIni.Read("path1");
                        startProcessPath1 = dbData.ToString();
                        keyName = "HKEY_CURRENT_USER\\SOFTWARE\\" + exitProcess1;

                        dbData = MyIni.Read("name2");
                        exitProcess2 = dbData.ToString();
                        dbData = MyIni.Read("path2");
                        startProcessPath2 = dbData.ToString();
                        keyName = "HKEY_CURRENT_USER\\SOFTWARE\\" + exitProcess2;
                        
                        textBox1.Text = startProcessPath1;
                        textBox2.Text = startProcessPath2;
                        
                    }


                    string[] data = { "No use", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

                    // 각 콤보박스에 데이타를 초기화
                    comboBox1.Items.AddRange(data);

                    if (programWeek[0] == 1) checkBox7.Checked = true;
                    else checkBox7.Checked = false;
                    if (programWeek[1] == 1) checkBox1.Checked = true;
                    else checkBox7.Checked = false;
                    if (programWeek[2] == 1) checkBox2.Checked = true;
                    else checkBox7.Checked = false;
                    if (programWeek[3] == 1) checkBox3.Checked = true;
                    else checkBox7.Checked = false;
                    if (programWeek[4] == 1) checkBox4.Checked = true;
                    else checkBox7.Checked = false;
                    if (programWeek[5] == 1) checkBox5.Checked = true;
                    else checkBox7.Checked = false;
                    if (programWeek[6] == 1) checkBox6.Checked = true;
                    else checkBox7.Checked = false;

                    DateTime enteredDate = DateTime.Parse(programStartTime);

                    try
                    {
                        dateTimePicker1.Value = Convert.ToDateTime(programStartTime);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }

                    // 종료 후 실행 주기(초)
                    comboBox1.SelectedIndex = programPeriod;

                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Start");

                    timer1.Interval = 1000;
                    timer1.Start();


                    writeINI();

                }
                else
                {
                    MessageBox.Show("Aleady started");
                    //본 프로그램 종료

                    realCose = true;
                    Application.Exit();
                }
            }
            catch
            {

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to exit?", "Observer", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                realCose = true;

                //트레이아이콘 없앰
                notifyIcon1.Visible = false;

                timer1.Stop();
                //본 프로그램 종료
                Application.Exit();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                this.itemSelected = comboBox1.SelectedItem as string;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            writeINI();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //프로그램 찾아서 종료
            Process[] process = Process.GetProcessesByName(exitProcess1);

            if (process.GetLength(0) > 0)
            {
                listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} End CleanManager");
                process[0].Kill();
                programPeriodRestart1 = 0;
            }
            else
            {
                //프로그램 시작
                try
                {
                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Start CleanManager");
                    Process.Start(startProcessPath1);
                }
                catch (Exception ex)
                {
                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Start fail CleanManager {ex.ToString()}");
                    //MessageBox.Show($"제어프로그램 재시작하지 못했습니다 \r\n {ex}", "에러");
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //프로그램 찾아서 종료
            Process[] process = Process.GetProcessesByName(exitProcess2);

            if (process.GetLength(0) > 0)
            {
                listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} End CleanManagerKiosk");
                process[0].Kill();
                programPeriodRestart2 = 0;
            }
            else
            {
                //프로그램 시작
                try
                {
                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Start CleanManagerKiosk");
                    Process.Start(startProcessPath2);
                }
                catch (Exception ex)
                {
                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Start fail CleanManagerKiosk {ex.ToString()}");
                    //MessageBox.Show($"제어프로그램 재시작하지 못했습니다 \r\n {ex}", "에러");
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if( shown == false)
            {
                shown = true;
                this.Close();
            }

            DateTime nowDt = DateTime.Now;
            //프로그램 찾아서 종료
            Process[] process1 = Process.GetProcessesByName(exitProcess1);
            Process[] process2 = Process.GetProcessesByName(exitProcess2);

            // 주간 재 실행 설정
            if (nowDt.Hour == dateTimePicker1.Value.Hour && nowDt.Minute == dateTimePicker1.Value.Minute && nowDt.Second == 0)
            {
                if (nowDt.DayOfWeek == DayOfWeek.Sunday && programWeek[0] == 1)
                {
                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Stop Sunday");
                    if (process1.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 실행 중이에요 💖");
                        process1[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 종료했어요 💖");
                        programPeriodRestart1 = 0;
                    }
                    if (process2.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 실행 중이에요 💖");
                        process2[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 종료했어요 💖");
                        programPeriodRestart2 = 0;
                    }
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Monday && programWeek[1] == 1)
                {
                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Stop Monday");
                    if (process1.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 실행 중이에요 💖");
                        process1[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 종료했어요 💖");
                        programPeriodRestart1 = 0;
                    }
                    if (process2.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 실행 중이에요 💖");
                        process2[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 종료했어요 💖");
                        programPeriodRestart2 = 0;
                    }
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Tuesday && programWeek[2] == 1)
                {
                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Stop Tuesday");
                    if (process1.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 실행 중이에요 💖");
                        process1[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 종료했어요 💖");
                        programPeriodRestart1 = 0;
                    }
                    if (process2.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 실행 중이에요 💖");
                        process2[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 종료했어요 💖");
                        programPeriodRestart2 = 0;
                    }
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Wednesday && programWeek[3] == 1)
                {
                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Stop Wednesday");
                    if (process1.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 실행 중이에요 💖");
                        process1[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 종료했어요 💖");
                        programPeriodRestart1 = 0;
                    }
                    if (process2.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 실행 중이에요 💖");
                        process2[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 종료했어요 💖");
                        programPeriodRestart2 = 0;
                    }
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Thursday && programWeek[4] == 1)
                {
                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Stop Thursday");
                    if (process1.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 실행 중이에요 💖");
                        process1[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 종료했어요 💖");
                        programPeriodRestart1 = 0;
                    }
                    if (process2.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 실행 중이에요 💖");
                        process2[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 종료했어요 💖");
                        programPeriodRestart2 = 0;
                    }
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Friday && programWeek[5] == 1)
                {
                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Stop Friday");
                    if (process1.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 실행 중이에요 💖");
                        process1[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 종료했어요 💖");
                        programPeriodRestart1 = 0;
                    }
                    if (process2.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 실행 중이에요 💖");
                        process2[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 종료했어요 💖");
                        programPeriodRestart2 = 0;
                    }
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Saturday && programWeek[6] == 1)
                {
                    listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Stop Saturday");
                    if (process1.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 실행 중이에요 💖");
                        process1[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process1.Length}개 종료했어요 💖");
                        programPeriodRestart1 = 0;
                    }
                    if (process2.GetLength(0) > 0)
                    {
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 실행 중이에요 💖");
                        process2[0].Kill();
                        listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {exitProcess1}' 프로세스가 {process2.Length}개 종료했어요 💖");
                        programPeriodRestart2 = 0;
                    }
                }

                restartFlag = true;
            }
            else
            {
                restartFlag = false;
            }

            if (process1.GetLength(0) > 0)
            {
                button2.BackColor = Color.Green;
                button2.Text = "CleanManger Run";
            }
            else
            {
                button2.BackColor = Color.Red; ;
                button2.Text = "CleanManager Stop";

                if (startProcessPath1 != "")
                {
                    // 강제시작 설정되어 있으면
                    if (programPeriod > 0)
                    {
                        if (programPeriodRestart1 == programPeriod)
                        {
                            listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Start CleanManager ({programPeriod}초)");
                            Process.Start(startProcessPath1);
                            programPeriodRestart1 = 0;
                        }
                        else
                        {
                            programPeriodRestart1++;

                            listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Ready CleanManager ({programPeriodRestart1}초)");

                            if (programPeriodRestart1 > programPeriod)
                                programPeriodRestart1 = programPeriod;
                        }
                    }
                }
            }

            if (process2.GetLength(0) > 0)
            {
                button3.BackColor = Color.Green;
                button3.Text = "CleanMangerKiosk Run";
            }
            else
            {
                button3.BackColor = Color.Red; ;
                button3.Text = "CleanMangerKiosk Stop";

                if (startProcessPath2 != "")
                {
                    // 강제시작 설정되어 있으면
                    if (programPeriod > 0)
                    {
                        if (programPeriodRestart2 == programPeriod)
                        {
                            listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Start CleanMangerKiosk ({programPeriod}초)");
                            Process.Start(startProcessPath2);
                            programPeriodRestart2 = 0;
                        }
                        else
                        {
                            programPeriodRestart2++;

                            listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Ready CleanMangerKiosk ({programPeriodRestart2}초)");

                            if (programPeriodRestart2 > programPeriod)
                                programPeriodRestart2 = programPeriod;
                        }
                    }
                }
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.ShowIcon = false; //아이콘의 모습을 보인다.
            notifyIcon1.Visible = false; //시스템트레이의 모습을 보인다.
            this.Visible = true; // 폼의 표시
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal; // 최소화를 멈춘다 
            this.Activate(); // 폼을 활성화 시킨다
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (realCose == false)
            {
                e.Cancel = true; // 종료 이벤트를 취소 시킨다
                this.Visible = false; // 폼을 표시하지 않는다;
                this.ShowIcon = true; //아이콘의 모습을 보인다.
                notifyIcon1.Visible = true; //시스템트레이의 모습을 보인다.
            }
        }

        /// <summary>
        /// 프로그램의 실행여부를 확인하는 함수.
        /// </summary>
        /// <returns></returns>
        public static bool IsRunningAppCheck()
        {
            System.Diagnostics.Process Process = System.Diagnostics.Process.GetCurrentProcess();
            string ProcName = Process.ProcessName;

            if (System.Diagnostics.Process.GetProcessesByName(ProcName).Length > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void StartUpProgram(bool opt)
        {
            if (opt == true)
            {
                try
                {
                    // 시작프로그램 등록하는 레지스트리
                    string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                    RegistryKey strUpKey = Registry.LocalMachine.OpenSubKey(runKey);
                    if (strUpKey.GetValue("Observer") == null)
                    {
                        strUpKey.Close();
                        strUpKey = Registry.LocalMachine.OpenSubKey(runKey, true);

                        // 시작프로그램 등록명과 exe경로를 레지스트리에 등록
                        strUpKey.SetValue("Observer", Application.ExecutablePath);

                    }
                    MessageBox.Show("Add Startup Success");

                }
                catch
                {
                    MessageBox.Show("Add Startup Fail");
                }
            }
            else
            {
                try
                {
                    // 시작프로그램 등록하는 레지스트리
                    string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                    RegistryKey strUpKey = Registry.LocalMachine.OpenSubKey(runKey);

                    if (strUpKey.GetValue("Observer") == null)
                    {
                        strUpKey.Close();
                        strUpKey = Registry.LocalMachine.OpenSubKey(runKey, true);

                        // 시작프로그램 등록명과 exe경로를 레지스트리에 등록
                        strUpKey.SetValue("Observer", Application.ExecutablePath);
                    }
                    MessageBox.Show("Add Startup Success");
                }
                catch
                {
                    MessageBox.Show("Add Startup Fail");

                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //StartUpProgram(true);
            AddStartupProgram("Observer", Application.ExecutablePath);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //StartUpProgram(false);
            RemoveStartupProgram("Observer");
        }

        // 부팅시 시작 프로그램을 등록하는 레지스트리 경로
        private static readonly string _startupRegPath =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        private Microsoft.Win32.RegistryKey GetRegKey(string regPath, bool writable)
        {
            return Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regPath, writable);
        }

        // 부팅시 시작 프로그램 등록
        public void AddStartupProgram(string programName, string executablePath)
        {
            using (var regKey = GetRegKey(_startupRegPath, true))
            {
                try
                {
                    // 키가 이미 등록돼 있지 않을때만 등록
                    if (regKey.GetValue(programName) == null)
                    {
                        regKey.SetValue(programName, executablePath);
                        MessageBox.Show("Added");
                    }
                    else
                    {
                        MessageBox.Show("Aleady has Added");
                    }

                    regKey.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed {ex.ToString()}");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // 등록된 프로그램 제거
        public void RemoveStartupProgram(string programName)
        {
            using (var regKey = GetRegKey(_startupRegPath, true))
            {
                try
                {
                    // 키가 이미 존재할때만 제거
                    if (regKey.GetValue(programName) != null)
                    {
                        regKey.DeleteValue(programName, false);
                        MessageBox.Show("Removed");
                    }
                    else
                    {
                        MessageBox.Show("Aleady has removed");
                    }

                    regKey.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed {ex.ToString()}");
                    Console.WriteLine(ex.Message);
                }
            }
        }
        

        private void writeINI()
        {

            programPeriod = comboBox1.SelectedIndex;
            MyIni.Write("period", programPeriod.ToString());

            if (checkBox7.Checked == true) programWeek[0] = 1;
            else programWeek[0] = 0;
            if (checkBox1.Checked == true) programWeek[1] = 1;
            else programWeek[1] = 0;
            if (checkBox2.Checked == true) programWeek[2] = 1;
            else programWeek[2] = 0;
            if (checkBox3.Checked == true) programWeek[3] = 1;
            else programWeek[3] = 0;
            if (checkBox4.Checked == true) programWeek[4] = 1;
            else programWeek[4] = 0;
            if (checkBox5.Checked == true) programWeek[5] = 1;
            else programWeek[5] = 0;
            if (checkBox6.Checked == true) programWeek[6] = 1;
            else programWeek[6] = 0;


            MyIni.Write("sun", programWeek[0].ToString());
            MyIni.Write("mon", programWeek[1].ToString());
            MyIni.Write("tue", programWeek[2].ToString());
            MyIni.Write("wed", programWeek[3].ToString());
            MyIni.Write("thu", programWeek[4].ToString());
            MyIni.Write("fri", programWeek[5].ToString());
            MyIni.Write("sat", programWeek[6].ToString());

            programStartTime = dateTimePicker1.Value.ToString("HH:mm:ss");
            MyIni.Write("time", programStartTime);

            MyIni.Write("name1", exitProcess1);
            MyIni.Write("name2", exitProcess2);
            MyIni.Write("path1", startProcessPath1);
            MyIni.Write("path2", startProcessPath2);

            listBox1.Items.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} Change INI");
        }

    }
}
