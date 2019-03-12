using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

//Binome : Louis B et Stéphan A
//Objectifs de la séance :

//Reussir le niveau de gris et noir et blanc : reussi
//Reussir la rotation 180 : reussi
//effet mirroir : reussi
//Agrandir ou retrecir l'image : en cours

namespace ProblèmeScientifique
{
    public class Pixel
    {
        int R;
        int V;
        int B;

        public Pixel(int R, int V, int B)
        {
            this.R = R;
            this.V = V;
            this.B = B;
        }
        Pixel()
        {
            this.R = 0;
            this.V = 0;
            this.B = 0;
        }

        public int GetR
        {
            get { return R; }
        }
        public int GetV
        {
            get { return V; }
        }
        public int GetB
        {
            get { return B; }
        }
        public int SetR
        {
            set { this.R = value; }
        }
        public int SetV
        {
            set { this.V = value; }
        }
        public int SetB
        {
            set { this.B = value; }
        }
        public Pixel Gris()
        {
            int niveau = (R + V + B) / 3;
            R = niveau;
            B = niveau;
            V = niveau;
            return new Pixel(R, V, B);
        }
        public Pixel PopArt()
        {
            Pixel pix = new Pixel();
            if (R<128)
            {
                pix.SetR = 0;
            }
            if (V < 128)
            {
                pix.SetV = 0;
            }
            if (B < 128)
            {
                pix.SetB = 0;
            }


            if (R >= 128)
            {
                pix.SetR = 255;
            }
            if (V >= 128)
            {
                pix.SetV = 255;
            }
            if (B >= 128)
            {
                pix.SetB = 255;
            }
            return new Pixel(R, V, B);
        }
        public Pixel NoirEtBlanc()
        {
            int niveau = (R + V + B) / 3;
            Pixel pix = new Pixel();
            if (niveau < 128)
            {
                pix.SetR = 0;
                pix.SetV = 0;
                pix.SetB = 0;
            }

            if (niveau >= 128)
            {
                pix.SetR = 255;
                pix.SetV = 255;
                pix.SetB = 255;
            }
            return new Pixel(R, V, B);
        }
    }
    public class MyImage
    {
        //variable d'instance
        byte[] header = new byte[54];
        int nbrBitsParCouleur;
        string[] type;
        int taille;
        int OffSet;
        int largeur;
        int hauteur;
        Pixel[,] image;

        //Constructor
        /* Version CSV
        public MyImageCSV(string myfile)
        {
            string[] Image;
            List<int> listMyFile = new List<int>();          
            Image = File.ReadAllLines(myfile);
            //Process.Start(@"D:\Stéphan\OneDrive - De Vinci\Année 2\S4\Problème scientifique informatique\coco.bmp");
            Console.ReadKey();
            for (int i = 0; i < Image.Length; i++)
            {
                string[] temp = Image[i].Split(',');
                for (int j = 0; j < temp.Length; j++)
                {
                    if (temp[j] != "")
                    {
                            listMyFile.Add(Convert.ToInt32(temp[j]));           
                    }
                }
            }

            type = ConvertEndianToString(listMyFile, 0, 1);
            taille = ConvertEndianToInt(listMyFile, 2, 5);
            OffSet = listMyFile[10];
            hauteur = ConvertEndianToInt(listMyFile, 18, 21);
            largeur = ConvertEndianToInt(listMyFile, 22, 25);

            Console.WriteLine(taille);
            Console.ReadKey();
            

            //On crée la matrice de Pixels contenant l'image
            image = new Pixel[hauteur, largeur];
            int compteur = 54;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j] = new Pixel(listMyFile[compteur],  listMyFile[compteur+1], listMyFile[compteur+2]);
                    compteur += 3;
                }
            }

            Console.WriteLine("HEADER");
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; i < 14; i++)
            {
                Console.Write(listMyFile[i] + " ");
            }
            Console.Write("\n\nHEADER INFO\n\n");
            for (int i = 14; i < 54; i++)
            {
                Console.Write(listMyFile[i] + " ");
            }
            Console.WriteLine("\n\nIMAGE\n");
            for (int i = 54; i < listMyFile.Count; i++)
            {
                //La tabulation fait le taffe.
                Console.Write(listMyFile[i] + "\t");
            }
            Console.ReadKey();

            
            //Assign le type
            this.type = (string)(list[0] + list[1]);
            //Asign la taille
            for (int i = 2; i < 6; i++)
            {
                this.taille += Convert.ToInt32(list[i]);
            }
            //Assign
            


            From_Image_To_File(listMyFile, "marche.bmp");
            Process.Start("marche.bmp");

        }
    */
        public MyImage(string fichier)
        {
            byte[] file = File.ReadAllBytes(fichier);
            for (int i = 0; i <= 53; i++)
            {
                header[i] = file[i];
            }

            type = ConvertEndianToString(header, 0, 1);
            taille = ConvertEndianToInt(header, 2, 5);
            OffSet = header[10];
            largeur = ConvertEndianToInt(header, 18, 21);
            hauteur = ConvertEndianToInt(header, 22, 25);
            Console.WriteLine(largeur);
            Console.WriteLine(hauteur);
            Console.WriteLine(taille);
            Console.WriteLine(OffSet);
            Console.WriteLine(type[0] + " " + type[1]);
            Console.ReadKey();

            image = new Pixel[hauteur, largeur];                     
            int compt = 54;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j] = new Pixel(file[compt+2], file[compt + 1], file[compt]);
                    compt += 3;
                }
            }
            //NiveauDeGris();
            //Rotation();
            //PopArt();
            //NoirEtBlanc();
            //Miroir();

            Pixel[,] imageSortie = Agrandissement();

            From_Image_To_File(MatricePixelToByte(imageSortie), NewHeader(largeur*2, hauteur*2), "sortie.png");
            Process.Start("sortie.png");
        }

        public static int ConvertEndianToInt(byte[] Image, int début, int fin) 
        {
            int result = 0;
            int power = 0;
            for (int i = début; i <= fin; i++)
            {
                result += Convert.ToInt32(Image[i]) * (Convert.ToInt32(Math.Pow(256, power)));
                ++power;
            }
            return result;
        }
        public static string[] ConvertEndianToString(byte[] Image, int début, int fin)
        {
            string[] typeImage = new string[2];
            int compteur = 0;

            for (int i = début; i <= fin; i++)
            {
                string MyString = Image[i].ToString();
                typeImage[compteur] = MyString;
                compteur++;
            }
            return typeImage;
        }
        public byte[] Convert_Int_To_Endian(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            //attention on est en little endian
            if (BitConverter.IsLittleEndian == false)
                Array.Reverse(bytes);
            return bytes;
        }

        /// <summary>
        /// Reconstitue le tableau de byte à l'aide du header
        /// </summary>
        /// <param name="image"></param>
        /// <param name="header"></param>
        /// <param name="NomDuFichier"></param>
        public void From_Image_To_File(byte[] image, byte[] header, string NomDuFichier)
        {
            byte[] ImageSortie = new byte[header.Length + image.Length];
            for (int i = 0; i < header.Length; i++)
            {
                ImageSortie[i] = header[i];
            }
            for (int j = 0; j < image.Length; j++)
            {
                ImageSortie[j+header.Length] = image[j];
            }
            File.WriteAllBytes(NomDuFichier, ImageSortie);
        }
        public byte[] MatricePixelToByte(Pixel[,] Image)
        {
            //3 pixels donc on multiplie par 3.
            byte[] image = new byte[(Image.GetLength(0) * Image.GetLength(1)) * 3];

            int compt = 0;
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    image[compt] = Convert.ToByte(Image[i, j].GetB);
                    image[compt + 1] = Convert.ToByte(Image[i, j].GetV);
                    image[compt + 2] = Convert.ToByte(Image[i, j].GetR); 
                    compt += 3;
                }
            }
            return image;
        }
        public byte[] NewHeader(int largeur, int hauteur)
        {
            byte[] NewHeader = new byte[54];
            for (int i = 0; i <= 53; i++)
            {
                NewHeader[i] = header[i];
            }
            //Ici on calcule la nouvelle taille du fichier transformé
            byte[] taille = Convert_Int_To_Endian(header.Length + (largeur * hauteur * 3));
            for (int i = 0; i <= 3; i++)
            {
                NewHeader[2 + i] = taille[i];
            }
            byte[] Largeur = Convert_Int_To_Endian(largeur);
            for (int i = 0; i <= 3; i++)
            {
                NewHeader[18 + i] = Largeur[i];
            }
            byte[] Hauteur = Convert_Int_To_Endian(hauteur);
            for (int i = 0; i <= 3; i++)
            {
                NewHeader[22 + i] = Hauteur[i];
            }
            return NewHeader;
        }
        public Pixel[,] Agrandissement()
        {
                int nouvelleH = hauteur * 2;
                int nouvelleL = largeur * 2;

                Pixel[,] temp = new Pixel[nouvelleL, nouvelleH];

                int var = 0;
                for (int i = 0; i < image.GetLength(0); i++)
                {
                    for (int j = 0; j < image.GetLength(1); j++)
                    {
                        temp[i + var, j] = image[i, j];
                        temp[i + var, j + 1] = image[i, j];
                        temp[i + 1 + var, j] = image[i, j];
                        temp[i + 1 + var, j + 1] = image[i, j];
                        var += 2;
                    }
                    var = 0;
                }
                return temp;
        }
        public Pixel[,] Rotation()
        {
            Pixel[,] temp = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int k = 0; k < image.GetLength(1); k++)
                {
                    temp[image.GetLength(0) - 1 - i, image.GetLength(1) - 1 - k] = image[i, k];
                }
            }
            return temp;
        }
        public Pixel[,] Miroir()
        {
            Pixel[,] temp = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int k = 0; k < image.GetLength(1); k++)
                {
                    temp[i, k] = image[i, image.GetLength(1) - 1 - k];
                }
            }
            return temp;
        }
        public Pixel[,] NiveauDeGris()
        {
            Pixel[,] temp = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int k = 0; k < image.GetLength(1); k++)
                {
                    temp[i,k] = image[i, k].Gris();
                }
            }
            return temp;
        }
        public Pixel[,] NoirEtBlanc()
        {
            Pixel[,] temp = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int k = 0; k < image.GetLength(1); k++)
                {
                    temp[i, k] = image[i, k].NoirEtBlanc();
                }
            }
            return temp;
        }
        public Pixel[,] PopArt()
        {
            Pixel[,] temp = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int k = 0; k < image.GetLength(1); k++)
                {
                    temp[i, k] = image[i, k].PopArt();
                }
            }
            return temp;
        }
        public Pixel[,] Rotation90()
        {
            Pixel[,] temp = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int k = 0; k < image.GetLength(1); k++)
                {
                    temp[image.GetLength(0) - 1 - i, image.GetLength(1) - 1 - k] = image[i, k];
                }
            }
            return temp;
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            //Process.Start(@"D:\Stéphan\OneDrive - De Vinci\Année 2\S4\Problème scientifique informatique\Image qui marche.csv");
            MyImage image = new MyImage(@"D:\Stéphan\OneDrive - De Vinci\Année 2\S4\Problème scientifique informatique\Test.bmp");
    

            Console.ReadKey();
        }
    }
}
