// Fenix Icon Manager 0.7
// © Ismael Heredia , Argentina , 2017

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Fenix_Icon_Manager
{
    public partial class FormHome : Form
    {
        public FormHome()
        {
            InitializeComponent();
        }

        public void preview()
        {

            var peticion = WebRequest.Create(lvIcons.FocusedItem.SubItems[2].Text);
            var respuesta = peticion.GetResponse();
            var mostrar = respuesta.GetResponseStream();
            ImgPreview1.Image = Bitmap.FromStream(mostrar);
        }

        public void bajar_png()
        {
            string bajar = lvIcons.FocusedItem.SubItems[2].Text;

            Uri urisplit = new Uri(bajar);
            string archivo = urisplit.AbsolutePath;
            FileInfo parte = new FileInfo(archivo);
            archivo = parte.Name;

            try
            {
                status.Text = "[+] Downloading ...";
                this.Refresh();
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = "Opera/9.80 (Windows NT 6.0) Presto/2.12.388 Version/12.14";
                web.DownloadFile(bajar, archivo);
                status.Text = "[+] Downloaded !";
                this.Refresh();
            }
            catch
            {
                MessageBox.Show("Error");
            }

            get_files();

        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SoundPlayer sound = new SoundPlayer(Fenix_Icon_Manager.Properties.Resources.click);
            sound.Play();

            preview();
        }

        private void pNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundPlayer sound = new SoundPlayer(Fenix_Icon_Manager.Properties.Resources.click);

            sound.Play();
            bajar_png();
        }

        private void iCOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundPlayer sound = new SoundPlayer(Fenix_Icon_Manager.Properties.Resources.click);
            sound.Play();

            bajar_ico();
        }

        public void bajar_ico()
        {
            SoundPlayer sound = new SoundPlayer(Fenix_Icon_Manager.Properties.Resources.click);
            sound.Play();

            string bajar = "http://www.iconfinder.com/icons/" + lvIcons.SelectedItems[0].Text + "/download/ico";

            try
            {
                status.Text = "[+] Downloading ...";
                this.Refresh();
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = "Opera/9.80 (Windows NT 6.0) Presto/2.12.388 Version/12.14";
                web.DownloadFile(bajar,lvIcons.SelectedItems[0].Text + ".ico");
                status.Text = "[+] Downloaded !";
                this.Refresh();
            }
            catch
            {
                MessageBox.Show("Error");
            }

            get_files();

        }

        public void get_files()
        {

            lvIconsFoundSave.Items.Clear();
            lbData.Items.Clear();

            string[] archivos = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (string archivo in archivos)
            {

                if (Path.GetExtension(archivo) == ".png" || Path.GetExtension(archivo) == ".ico")
                {

                    lbData.Items.Add(archivo);
                    FileInfo basename = new FileInfo(archivo);
                    string nombre = basename.Name;

                    ListViewItem agregar = new ListViewItem();
                    agregar.Text = nombre;
                    FileInfo info = new FileInfo(nombre);
                    DateTime cuando = info.CreationTime;
                    agregar.SubItems.Add(Convert.ToString(cuando));

                    lvIconsFoundSave.Items.Add(agregar);
                }

            }

            gbIconsFoundSave.Text = "Icons found [" + lvIconsFoundSave.Items.Count + "]";
        }

        public void preview_icono()
        {
            string ruta = lbData.Items[lvIconsFoundSave.FocusedItem.Index].ToString();

            if (Path.GetExtension(ruta) == ".ico")
            {
                imgPreview2.Image = Bitmap.FromHicon(new Icon(ruta, new Size(48, 48)).Handle);
            }
            else
            {
                imgPreview2.Image = Image.FromFile(ruta);
            }
        }

        private void previewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SoundPlayer sound = new SoundPlayer(Fenix_Icon_Manager.Properties.Resources.click);
            sound.Play();
            preview_icono();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundPlayer sound = new SoundPlayer(Fenix_Icon_Manager.Properties.Resources.click);
            sound.Play();
            get_files();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SoundPlayer sound = new SoundPlayer(Fenix_Icon_Manager.Properties.Resources.click);
            sound.Play();

            lvIcons.Items.Clear();

            List<string> lista_id = new List<string> { };
            List<string> lista_size = new List<string> { };
            List<string> lista_url = new List<string> { };

            status.Text = "[+] Searching ...";
            this.Refresh();

            WebClient web = new WebClient();
            web.Headers["User-Agent"] = "Opera/9.80 (Windows NT 6.0) Presto/2.12.388 Version/12.14";
            string link = "https://www.iconfinder.com/xml/search/?q=" + txtSearch.Text + "&c=" + txtIconsForPage.Value + "&p=" + txtPage.Value + "&min=" + txtMinimumSize.Value + "&max=" + txtMaximumSize.Value + "&api_key=170f67bacc584d7f96bcd26970d0c1d0";
            string codigofuente = web.DownloadString(link);
            //richTextBox1.AppendText(code);

            Match search1 = Regex.Match(codigofuente, "<id>(.*?)</id>", RegexOptions.IgnoreCase);

            while (search1.Success)
            {
                string id = search1.Groups[1].Value;
                lista_id.Add(id);
                //richTextBox1.AppendText(id + "\n");
                search1 = search1.NextMatch();
            }

            Match search2 = Regex.Match(codigofuente, "<size>(.*?)</size>", RegexOptions.IgnoreCase);

            while (search2.Success)
            {
                string size = search2.Groups[1].Value;
                lista_size.Add(size);
                //richTextBox1.AppendText(size + "\n");
                search2 = search2.NextMatch();
            }

            Match search3 = Regex.Match(codigofuente, "<image>(.*?)</image>", RegexOptions.IgnoreCase);

            while (search3.Success)
            {
                string imagen = search3.Groups[1].Value;
                lista_url.Add(imagen);
                //richTextBox1.AppendText(imagen + "\n");
                search3 = search3.NextMatch();
            }

            for (int i = 0; i <= lista_id.Count - 1; i++)
            {
                ListViewItem agregar = new ListViewItem();
                agregar.Text = lista_id[i];

                agregar.SubItems.Add(lista_size[i]);
                agregar.SubItems.Add(lista_url[i]);
                lvIcons.Items.Add(agregar);

            }

            status.Text = "[+] Finished";
            this.Refresh();

            gbIconsFound.Text = "Icons found [" + lvIcons.Items.Count + "]";

            SoundPlayer sound1 = new SoundPlayer(Fenix_Icon_Manager.Properties.Resources.scanfin);
            sound1.Play();
        }

        private void lvIcons_DoubleClick(object sender, EventArgs e)
        {
            //MessageBox.Show(listView1.FocusedItem.SubItems[2].Text);
            SoundPlayer sound = new SoundPlayer(Fenix_Icon_Manager.Properties.Resources.click);
            sound.Play();
            preview();
        }

        private void lvIconsFoundSave_DoubleClick(object sender, EventArgs e)
        {
            SoundPlayer sound = new SoundPlayer(Fenix_Icon_Manager.Properties.Resources.click);
            sound.Play();
            preview_icono();
        }

        private void FormHome_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("icons_downloads"))
            {
                System.IO.Directory.CreateDirectory("icons_downloads");
                Directory.SetCurrentDirectory("icons_downloads");
            }
            else
            {
                Directory.SetCurrentDirectory("icons_downloads");
            }

            get_files();

            SoundPlayer sound = new SoundPlayer(Fenix_Icon_Manager.Properties.Resources.formcreate);
            sound.Play();
        }

        private void lblPowered_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.iconfinder.com");
        }

    }
}
