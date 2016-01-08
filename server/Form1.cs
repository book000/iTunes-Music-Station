using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using iTunesLib;
using System.Net;

namespace iTunesLib
{
    public partial class Form1 : Form
    {
        private iTunesApp iTunes;
        String nowstatus;
        public Form1()
        {

            InitializeComponent();
            //iTunes COMのセットアップ
            iTunes = new iTunesApp();
            iTunes.OnAboutToPromptUserToQuitEvent += new _IiTunesEvents_OnAboutToPromptUserToQuitEventEventHandler(iTunes_OnAboutToPromptUserToQuitEvent);
            iTunes.OnPlayerPlayEvent += new _IiTunesEvents_OnPlayerPlayEventEventHandler(iTunes_OnPlayerPlayEvent);

        }
        iTunesApp app = new iTunesLib.iTunesApp();

        void iTunes_OnPlayerPlayEvent(object iTrack)
        {
            string text = "";
            iTunesApp app = new iTunesLib.iTunesApp();
            IITTrack track = app.CurrentTrack;
            if (track != null && track.Enabled)
            {
                text = string.Format("「{0} - {1}」 by {2}", track.Name, track.Album, track.Artist);
            }
            Marshal.ReleaseComObject(app);

        }
        //iTunes COM SDKを解放
        void ReleaseCOM()
        {
            iTunes.OnPlayerPlayEvent -= iTunes_OnPlayerPlayEvent;
            iTunes.OnAboutToPromptUserToQuitEvent -= iTunes_OnAboutToPromptUserToQuitEvent;

            Marshal.ReleaseComObject(iTunes);
            iTunes = null;
        }
        void iTunes_OnAboutToPromptUserToQuitEvent()
        {
            ReleaseCOM();
            StreamWriter writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\music.txt", false, Encoding.Default);
            writer.WriteLine("OFFLINE");
            writer.Close();
            writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\album.txt", false, Encoding.Default);
            writer.WriteLine("OFFLINE");
            writer.Close();
            writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\artist.txt", false, Encoding.Default);
            writer.WriteLine("OFFLINE");
            writer.Close();
            writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\now.txt", false, Encoding.Default);
            writer.WriteLine("stop");
            writer.Close();
            this.Invoke((MethodInvoker)delegate()
            {
                //pictureBox1.Visible = true;
                this.TopMost = true;
                timer2.Enabled = true;
                timer2.Start();
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iTunesApp app = new iTunesLib.iTunesApp();
            app.Play();
            IITTrack track = app.CurrentTrack;
            if (track != null && track.Enabled)
            {
                label1.Text = track.Name;
                label2.Text = track.Album;
                label3.Text = track.Artist;
            }
            Marshal.ReleaseComObject(app);
            nowstatus = "start";
            StreamWriter writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\now.txt", false, Encoding.Default);
            writer.WriteLine("start");
            writer.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            iTunesApp app = new iTunesLib.iTunesApp();
            app.Pause();
            IITTrack track = app.CurrentTrack;
            if (track != null && track.Enabled)
            {
                label1.Text = track.Name;
                label2.Text = track.Album;
                label3.Text = track.Artist;
            }
            Marshal.ReleaseComObject(app);
            nowstatus = "stop";
            StreamWriter writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\now.txt", false, Encoding.Default);
            writer.WriteLine("stop");
            writer.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            iTunesApp app = new iTunesLib.iTunesApp();
            IITTrack track = app.CurrentTrack;
            if (track != null && track.Enabled)
            {
                label1.Text = track.Name;
                label2.Text = track.Album;
                label3.Text = track.Artist;
                StreamWriter writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\music.txt", false, Encoding.Default);
                writer.WriteLine(track.Name);
                writer.Close();
                writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\album.txt", false, Encoding.Default);
                writer.WriteLine(track.Album);
                writer.Close();
                writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\artist.txt", false, Encoding.Default);
                writer.WriteLine(track.Artist);
                writer.Close();
                
            }
            else {
                label1.Text = "null";
                label2.Text = "null";
                label3.Text = "null";
            }
            Marshal.ReleaseComObject(app);
            StreamReader sr = new StreamReader("C:\\xampp\\htdocs\\music\\now.txt", Encoding.Default);
            if (File.Exists("C:\\xampp\\htdocs\\music\\now.txt"))
            {
                nowstatus = sr.ReadLine();

            }
            sr.Close();
        }

        String music, album, artist;
        private void timer1_Tick(object sender, EventArgs e)
        {
            iTunesApp app = new iTunesLib.iTunesApp();
            IITTrack track = app.CurrentTrack;

            if (track != null && track.Enabled)
            {
                if (music != track.Name)
                {
                    music = track.Name;
                    album = track.Album;
                    artist = track.Artist;
                    label1.Text = track.Name;
                    label2.Text = track.Album;
                    label3.Text = track.Artist;
                    StreamWriter writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\music.txt", false, Encoding.Default);
                    writer.WriteLine(track.Name);
                    writer.Close();
                    writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\album.txt", false, Encoding.Default);
                    writer.WriteLine(track.Album);
                    writer.Close();
                    writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\artist.txt", false, Encoding.Default);
                    writer.WriteLine(track.Artist);
                    writer.Close();
                    
                }

            }
            else
            {
                label1.Text = "null";
                label2.Text = "null";
                label3.Text = "null";
            }
            Marshal.ReleaseComObject(app);

            string strs = "";
            StreamReader srs = new StreamReader("C:\\xampp\\htdocs\\music\\now.txt", Encoding.Default);
            if (File.Exists("C:\\xampp\\htdocs\\music\\now.txt"))
            {
                strs = srs.ReadLine();
                //Console.Write(str);

                srs.Close();
                iTunesApp apps = new iTunesLib.iTunesApp();
                
                if (strs == "start" && nowstatus != "start")
                {
                    apps.Play();
                    nowstatus = "start";
                }
                else if (strs == "stop" && nowstatus != "stop")
                {
                    apps.Pause();
                    nowstatus = "stop";
                }
                else if (strs == "back")
                {
                    apps.BackTrack();
                    StreamWriter writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\now.txt", false, Encoding.Default);
                    writer.WriteLine(nowstatus);
                    writer.Close();
                }
                else if (strs == "next")
                {
                    apps.NextTrack();
                    StreamWriter writer = new StreamWriter(@"C:\\xampp\\htdocs\\music\\now.txt", false, Encoding.Default);
                    writer.WriteLine(nowstatus);
                    writer.Close();
                }
                StreamReader sr = new StreamReader("C:\\xampp\\htdocs\\music\\voice.txt", Encoding.Default);
                string str = sr.ReadLine();
                int voice = int.Parse(str);
                if (voice != apps.SoundVolume || progressBar1.Value == 0)
                {
                    apps.SoundVolume = voice;
                    progressBar1.Value = voice;
                }
                sr.Close();
                
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
