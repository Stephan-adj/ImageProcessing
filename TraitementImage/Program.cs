using System;
using System.Diagnostics;
using System.IO;
//ADJARIAN Stéphan
namespace TraitementImage
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

        //Accesseurs et mutateurs utiles dans les fonctions de la classe myImage
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

        //Fonctions modifiant un pixel
        //En gris
        public Pixel Gris()
        {
            int niveau = (GetR + GetV + GetB) / 3;
            R = niveau;
            B = niveau;
            V = niveau;
            return new Pixel(R, V, B);
        }
        //En contraste
        public Pixel PopArt()
        {
            Pixel pix = new Pixel();
            if (R<128)
            {
                R = 0;
            }
            if (V < 128)
            {
                V = 0;
            }
            if (B < 128)
            {
                B = 0;
            }


            if (R >= 128)
            {
                R = 255;
            }
            if (V >= 128)
            {
                V = 255;
            }
            if (B >= 128)
            {
                B = 255;
            }
            return new Pixel(R, V, B);
        }
        public Pixel PopArt_2()
        {
            Pixel pix = new Pixel();

            if (B > R & B > V)
            {
                R = 255;
                V = 150;
                B = 255;
            }

            if (R > B & R > V)
            {
                R = 252;
                V = 220;
                B = 18;

            }

            if (V > R & V > B)
            {
                R = 135;
                V = 233;
                B = 144;
            }
            return new Pixel(R, V, B);
        }
        public Pixel PopArt_3()
        {
            Pixel pix = new Pixel();

            if (B > R & B > V)
            {
                R = 211;
                V = 115;
                B = 212;
            }

            if (R > B & R > V)
            {
                R = 255;
                V = 127;
                B = 0;

            }

            if (V > R & V > B)
            {
                R = 121;
                V = 248;
                B = 248;
            }
            return new Pixel(R, V, B);
        }
        public Pixel PopArt_4()
        {
            Pixel pix = new Pixel();

            if (B > R & B > V)
            {
                R = 187;
                V = 210;
                B = 255;
            }

            if (R > B & R > V)
            {
                R = 153;
                V = 122;
                B = 144;

            }

            if (V > R & V > B)
            {
                R = 121;
                V = 128;
                B = 127;
            }


            return new Pixel(R, V, B);
        }
        //En noir et blanc
        public Pixel NoirEtBlanc()
        {
            int niveau = (R + V + B) / 3;
            Pixel pix = new Pixel();
            if (niveau < 128)
            {
                R = 0;
                V = 0;
                B = 0;
            }

            if (niveau >= 128)
            {
                R = 255;
                V = 255;
                B = 255;
            }
            return new Pixel(R, V, B);
        }
    }
    public class MyImage
    {
        //variables d'instance
        byte[] header = new byte[54];
        string[] type;
        int taille;
        int OffSet;
        int largeur;
        int hauteur;
        //Le tableau de pixel qui stockera uniquement l'image (pas ses caractéristiques telles que sa hauteur ou largeur)
        Pixel[,] image;

        /// <summary>
        /// Constructor : il vient créer un objet MyImage en stockant en mémoire-machine les infos d'une image que l'utilisateur a choisi 
        /// </summary>
        /// <param name="fichier"> L'utilisateur entre le nom de son fichier. </param>
        public MyImage(string fichier)
        {
            //On vient lire toutes les infos de l'image et on les stock dans un un tableau de byte 'file'.
            byte[] file = File.ReadAllBytes(fichier);
            //On stock le header dans un autre tableau de byte
            for (int i = 0; i <= 53; i++)
            {
                header[i] = file[i];
            }
            //On stock ici les autres variables d'instance depuis le tableau de byte header, après les avoir converti car ils sont stockés en endian.
            type = ConvertEndianToString(header, 0, 1);
            taille = ConvertEndianToInt(header, 2, 5);
            OffSet = header[10];
            largeur = ConvertEndianToInt(header, 18, 21);
            hauteur = ConvertEndianToInt(header, 22, 25);
            //On vient ici remplir notre tableau de pixel stockant l'image choisi par l'utilisateur
            image = new Pixel[hauteur, largeur];  
            //On se décale de 54 pour prendre uniquement l'image (et pas le header)
            int compt = 54;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j] = new Pixel(file[compt+2], file[compt + 1], file[compt]);
                    compt += 3;
                }
            }
        }

        /// <summary>
        /// Cette fonction convertie depuis le tableau du header les informations (codé en endian) en int.
        /// </summary>
        /// <param name="Header"> On vient convertir des informations depuis le tableau de byte du header. </param>
        /// <param name="début"> Indique depuis quel index du tableau on commence la conversion. </param>
        /// <param name="fin"> Indique jusqu'à quel index va la conversion. </param>
        /// <returns>
        /// Cette fonction retourne un int qui est le résultat de la conversion.
        /// </returns>
        public static int ConvertEndianToInt(byte[] Header, int début, int fin) 
        {
            int result = 0;
            int power = 0;
            for (int i = début; i <= fin; i++)
            {
                result += Convert.ToInt32(Header[i]) * (Convert.ToInt32(Math.Pow(256, power)));
                ++power;
            }
            return result;
        }
        /// <summary>
        /// Cette fonction convertie depuis le tableau du header les informations (codé en endian) en string.
        /// </summary>
        /// <param name="Header"> On vient convertir des informations depuis le tableau de byte du header. </param>
        /// <param name="début"> Indique depuis quel index du tableau on commence la conversion. </param>
        /// <param name="fin"> Indique jusqu'à quel index va la conversion. </param>
        /// <returns>
        /// Cette fonction renvoi un tableau de string qui contient les informations converties.
        /// </returns>
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
        /// <summary>
        /// Convertie un int en endian.
        /// </summary>
        /// <param name="value"> Cette variable est converti en endian. </param>
        /// <returns>
        /// Cette fonction renvoie un tableau de byte contenant l'int traduit en endian.
        /// </returns>
        public byte[] Convert_Int_To_Endian(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            //attention on est en little endian
            if (BitConverter.IsLittleEndian == false)
                Array.Reverse(bytes);
            return bytes;
        }

        /// <summary>
        /// Cette fonction vient mettre le header puis l'image qui a été modifé dans un tableau de byte qui et ensuite écrit dans un fichier bmp.
        /// </summary>
        /// <param name="imageModifiée"> tableau de byte contenant l'image. </param>
        /// <param name="header"> tabkeau de byte contenant les headers. </param>
        /// <param name="NomDuFichier"></param>
        public void From_Image_To_File(byte[] imageModifiée, byte[] header, string NomDuFichier)
        {
            byte[] ImageSortie = new byte[header.Length + imageModifiée.Length];
            for (int i = 0; i < header.Length; i++)
            {
                ImageSortie[i] = header[i];
            }
            for (int j = 0; j < imageModifiée.Length; j++)
            {
                ImageSortie[j+header.Length] = imageModifiée[j];
            }
            File.WriteAllBytes(NomDuFichier, ImageSortie);
        }
        /// <summary>
        /// Cette fonction vient convertir la matrice de pixel en tableau de byte.
        /// </summary>
        /// <param name="Image"> </param>
        /// <returns>
        /// Cette fonction retourne un tableau de byte contenant l'image modifiée
        /// </returns>
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
        /// <summary>
        /// Cette fonction recrée un header en fonction de l'image modifiée.
        /// </summary>
        /// <param name="hauteur"> la hauteur de l'image modifiée. </param>
        /// <param name="largeur"> la largeur de l'image modifiée. </param>
        /// <returns>
        /// Elle retourne un tableau de byte contenant le header de l'image modifiée.
        /// </returns>
        public byte[] NewHeader(int hauteur, int largeur)
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

        /// <summary>
        /// Cette fonction crée une nouvelle matrice de pixel qui est une image agrandie d'un certain coeff d'agrandissement de l'image qu'à choisi l'utilisateur.
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel de l'image agrandie.
        /// </returns>
        public Pixel[,] Agrandissement()
        {
                Console.WriteLine("Saisissez la valeur de l'agrandissement de votre image (entre 1 et 40 max pour Coco) ==>");
                int coeffAgrandissement = Convert.ToInt16(Console.ReadLine());
                Pixel[,] imageAgrandie = new Pixel[image.GetLength(0) * coeffAgrandissement, image.GetLength(1) * coeffAgrandissement];
                int décalageLigne = 0;
                int décalageColonne = 0;
                for (int colonne = 0; colonne < image.GetLength(1); colonne++)
                {
                    for (int ligne = 0; ligne < image.GetLength(0); ligne++)
                    {
                        //Permet d'agrandir un pixel en un carré de pixel de côté égale au coeff d'agrandissement
                        for (int k = 0; k < coeffAgrandissement; k++)
                        {
                            for (int l = 0; l < coeffAgrandissement; l++)
                            {
                                imageAgrandie[ligne + k + décalageLigne, colonne + l + décalageColonne] = image[ligne, colonne];
                            }                        
                        }
                        décalageLigne +=(coeffAgrandissement-1);
                    }
                    décalageColonne+= (coeffAgrandissement-1);
                    décalageLigne = 0;
                }
                return imageAgrandie;
        }
        /// <summary>
        /// Cette fonction crée une nouvelle matrice de pixel qui est une image rétrécie d'un certain coeff de rétrécissement de l'image qu'à choisi l'utilisateur.
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel de l'image rétrécie.
        /// </returns>
        public Pixel[,] Rétrécissement()
        {
            Console.WriteLine("Saisissez la valeur du retrecissement de votre image (entre 1 et 40 max pour Coco) ==>");
            int coeffRétrécissement = Convert.ToInt16(Console.ReadLine());
            Pixel[,] ImageRétrécie = new Pixel[image.GetLength(0) / coeffRétrécissement, image.GetLength(1) / coeffRétrécissement];
            int décalageColonne = 0;
            int décalageLigne = 0;
            int moyenneR = 0;
            int moyenneV = 0;
            int moyenneB = 0;
            //On parcourt toute la matrice rétrécie
            for (int colonne = 0; colonne < image.GetLength(1) / coeffRétrécissement; colonne++)
            {
                for (int ligne = 0; ligne < image.GetLength(0) / coeffRétrécissement; ligne++)
                {
                    //permet de faire la moyenne des couleurs d'un carré de pixel voisins faisant la taille du coeff de rétrécissement
                    for (int k = 0; k < coeffRétrécissement; k++)
                    {
                        for (int l = 0; l < coeffRétrécissement; l++)
                        {
                            moyenneB += image[ligne + l + décalageLigne, colonne + k + décalageColonne].GetB;
                            moyenneV += image[ligne + l + décalageLigne, colonne + k + décalageColonne].GetV;
                            moyenneR += image[ligne + l + décalageLigne, colonne + k + décalageColonne].GetR;
                        }
                    }
                    ImageRétrécie[ligne, colonne] = new Pixel(moyenneR / (coeffRétrécissement * coeffRétrécissement), moyenneV / (coeffRétrécissement * coeffRétrécissement), moyenneB / (coeffRétrécissement * coeffRétrécissement));
                    //On décale la ligne
                    décalageLigne += (coeffRétrécissement - 1);
                    moyenneB = 0;
                    moyenneR = 0;
                    moyenneV = 0;
                }
                //on décale la colonne
                décalageColonne += (coeffRétrécissement - 1);
                //On reset le décalage pour rebalayer toute la ligne à nouveau
                décalageLigne = 0;
            }
            return ImageRétrécie;
        }
        /// <summary>
        /// Cette fonction crée une nouvelle matrice de pixel étant une rotation à 180° de l'image choisi par l'utilisateur.
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel de l'image tournée à 180°.
        /// </returns>
        public Pixel[,] Rotation180()
        {
            Pixel[,] ImageRetournée = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int k = 0; k < image.GetLength(1); k++)
                {
                    ImageRetournée[image.GetLength(0) - 1 - i, image.GetLength(1) - 1 - k] = image[i, k];
                }
            }
            return ImageRetournée;
        }
        /// <summary>
        /// Cette fonction crée une nouvelle matrice de pixel étant une rotation à 90° de l'image choisi par l'utilisateur.
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel de l'image tournée à 90°.
        /// </returns>
        public Pixel[,] Rotation90()
        {
            Pixel[,] ImagePenchée = new Pixel[image.GetLength(1), image.GetLength(0)];
            for (int i = 0; i < image.GetLength(1); i++)
            {
                for (int k = 0; k < image.GetLength(0); k++)
                {
                    ImagePenchée[i, image.GetLength(0) - k - 1] = image[k, i];
                }
            }
            return ImagePenchée;
        }
        /// <summary>
        /// Cette fonction crée une nouvelle matrice de pixel étant une rotation à 270° de l'image choisi par l'utilisateur.
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel de l'image tournée à 270°.
        /// </returns>
        public Pixel[,] Rotation270()
        {
            Pixel[,] ImagePenchée = new Pixel[image.GetLength(1), image.GetLength(0)];
            for (int i = 0; i < image.GetLength(1); i++)
            {
                for (int k = 0; k < image.GetLength(0); k++)
                {
                    ImagePenchée[image.GetLength(1) - i - 1, k] = image[k, i];
                }
            }
            return ImagePenchée;
        }
        /// <summary>
        /// Cette fonction crée une nouvelle matrice de pixel étant une image miroir de l'image choisi par l'utilisateur.
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel de l'image miroir.
        /// </returns>
        public Pixel[,] Miroir()
        {
            Pixel[,] ImageInversée = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int k = 0; k < image.GetLength(1); k++)
                {
                    ImageInversée[i, k] = image[i, image.GetLength(1) - 1 - k];
                }
            }
            return ImageInversée;
        }
        /// <summary>
        /// Cette fonction crée une nouvelle matrice de pixel étant une image en nuance de gris de l'image choisi par l'utilisateur.
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel de l'image en nuance de gris.
        /// </returns>
        public Pixel[,] NiveauDeGris()
        {
            Pixel[,] ImageGrise = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int k = 0; k < image.GetLength(1); k++)
                {
                    ImageGrise[i,k] = image[i, k].Gris();
                }
            }
            return ImageGrise;
        }
        /// <summary>
        /// Cette fonction crée une nouvelle matrice de pixel étant une image en noir et blanc de l'image choisi par l'utilisateur.
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel de l'image en noir et blanc.
        /// </returns>
        public Pixel[,] NoirEtBlanc()
        {
            Pixel[,] ImageNetB = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int k = 0; k < image.GetLength(1); k++)
                {
                    ImageNetB[i, k] = image[i, k].NoirEtBlanc();
                }
            }
            return ImageNetB;
        }
        /// <summary>
        /// Cette fonction crée une nouvelle matrice de pixel étant une image version PopArt de l'image choisi par l'utilisateur.
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel de l'image PopArt.
        /// </returns>
        public Pixel[,] PopArt()
        {
            Pixel[,] ImagePopArt = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int k = 0; k < image.GetLength(1); k++)
                {
                    ImagePopArt[i, k] = image[i, k].PopArt();
                }
            }
            return ImagePopArt;
        }
        /// <summary>
        /// Cette fonction crée une nouvelle matrice de pixel contenant soit une fractale de Mandelbrot soit de Julia.
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel de la fractale.
        /// </returns>
        public Pixel[,] Fractale()
        {
            Console.WriteLine("Quelle fractale voulez vous affichez? Tapez 1 pour la fractale de Mandelbrot, 2 pour celle de Julia ");
            string choix = Convert.ToString(Console.ReadLine());
            double x_min = 0;
            double x_max = 0;
            double y_min = 0;
            double y_max = 0;
            double c_x = 0;
            double c_y = 0;
            double x_n = 0;
            double y_n = 0;
            
            Console.WriteLine("Saisissez la hauteur de votre fractale (entre 0 et 1000) ==>");
            int hauteur = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Saisissez la largeur de votre fractale (entre 0 et 1000) ==>");
            int largeur = Convert.ToInt32(Console.ReadLine());
            Pixel[,] Image_fractale = new Pixel[largeur, hauteur];
            int iteration_max = 50;
            if (choix == "1")
            {
                x_min = -2;
                x_max = 0.5;
                y_min = -1.25;
                y_max = 1.25;
            }
            if (choix == "2")
            {
                x_min = -1.25;
                x_max = 1.25;
                y_min = -1.25;
                y_max = 1.25;
            }

            for (int y = 0; y < Image_fractale.GetLength(1); y++)
                for (int x = 0; x < Image_fractale.GetLength(0); x++)
                {
                    if (choix == "1")
                    {
                        c_x = (x * (x_max - x_min) / Image_fractale.GetLength(0) + x_min);
                        c_y = (y * (y_min - y_max) / Image_fractale.GetLength(1) + y_max);
                        x_n = 0;
                        y_n = 0;
                    }
                    if (choix == "2")
                    {
                        c_x = 0.285;
                        c_y = 0.01;
                        x_n = (x * (x_max - x_min) / Image_fractale.GetLength(1) + x_min);
                        y_n = (y * (y_min - y_max) / Image_fractale.GetLength(0) + y_max);

                    }
                    int compteur = 0;

                    while ((x_n * x_n + y_n * y_n) < 4 && compteur < iteration_max)
                    {
                        double temporaire_x = x_n;
                        double temporaire_y = y_n;
                        x_n = temporaire_x * temporaire_x - temporaire_y * temporaire_y + c_x;
                        y_n = 2 * temporaire_x * temporaire_y + c_y;
                        compteur = compteur + 1;
                    }
                    if (compteur == iteration_max)
                    {
                        Image_fractale[x, y] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        //amélioration trouvée sur Internet pour rendre l'image plus agréable à regarder.
                        Image_fractale[x, y] = new Pixel((3 * compteur) % 256, (1 * compteur) % 256, (10 * compteur) % 256);
                    }
                }
            return Image_fractale;
        }
        /// <summary>
        /// Cette fonction crée une nouvelle matrice de pixel contenant une image convulutionnée selon le choix de l'utilisateur (il y a différents choix de Kernek, flou, détection de contour etc...)
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel de l'image convolutionnée.
        /// </returns>
        public Pixel[,] Convolution()
        {
            int[,] Kernel = null;
            Console.WriteLine("Choississez votre type de convolution ==> \n"
                 + " 1 : Détetection de contour°\n"
                 + " 2 : Flou\n"
                 + " 3 : Repoussage\n"
                 + " 4 : Renforcement\n");

            string choix = Convert.ToString(Console.ReadLine());
            switch (choix)
            {
                case "1":
                    int[,] MatricedétectionContour ={ { 0, 1, 0 },
                                                      { 1, -4, 1 },
                                                      { 0, 1, 0 } };
                    Kernel = MatricedétectionContour;
                    break;
                case "2":
                    int[,] MatriceFlou ={ { 1, 1, 1 },
                                          { 1, 1, 1 },
                                          { 1, 1, 1 } };
                    Kernel = MatriceFlou;
                    break;
                case "3":
                    int[,] MatriceRepoussage ={ { -2, -1, 0 },
                                                { -1, 1, 1 },
                                                { 0, 1, 2 } };
                    Kernel = MatriceRepoussage;
                    break;
                case "4":
                    int[,] MatriceRenforcement ={ { 0, 0, 0 },
                                                  { -1, 1, 0 },
                                                  { 0, 0, 0 } };
                    Kernel = MatriceRenforcement;
                    break;
            }

            Pixel[,] ImageConvolue = new Pixel[image.GetLength(0), image.GetLength(1)];
            //Met la 1ère colonne et la dernière colonne en blanc
            for (int i = 0; i < image.GetLength(0); i++)
            {
                ImageConvolue[i, 0] = new Pixel(255, 255, 255);
                ImageConvolue[i, (image.GetLength(1) - 1)] = new Pixel(255, 255, 255);
            }
            //Idem avec les lignes
            for (int j = 0; j < image.GetLength(1); j++)
            {
                ImageConvolue[0, j] = new Pixel(255, 255, 255);
                ImageConvolue[(image.GetLength(0) - 1), j] = new Pixel(255, 255, 255);
            }

            int moyenneR = 0;
            int moyenneV = 0;
            int moyenneB = 0;
            for (int i = 1; i < image.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < image.GetLength(1) - 1; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            moyenneR += image[i + k, j + l].GetR * Kernel[k + 1, l + 1];
                            moyenneV += image[i + k, j + l].GetV * Kernel[k + 1, l + 1];
                            moyenneB += image[i + k, j + l].GetB * Kernel[k + 1, l + 1];
                        }
                    }
                    if(choix=="2")
                    {
                        moyenneR /= 9;
                        moyenneV /= 9;
                        moyenneB /= 9;
                    }
                    if(moyenneR > 255 || moyenneR < 0 )
                    {
                        moyenneR = 0;
                    }
                    if (moyenneV > 255 || moyenneV < 0)
                    {
                        moyenneV = 0;
                    }
                    if (moyenneB > 255 || moyenneB < 0)
                    {
                        moyenneB = 0;
                    }
                    ImageConvolue[i, j] = new Pixel(moyenneR, moyenneV, moyenneB);
                    moyenneR = 0;
                    moyenneV = 0;
                    moyenneB = 0;
                }
            }
            return ImageConvolue;
        }
        /// <summary>
        /// Cette fonction vient créer un histogramme d'une des couleurs primaires R, V ou B d'une image, 
        /// l'ordonnée varie selon le nombre d'occurence d'une nuance de couleur, tandis que l'abscisse varie de 0 à 255 
        /// (pour toutes les nuances possibles d'une couleur codée sur 8 bits).
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel contenant l'histogramme d'une des trois couleurs RVB.
        /// </returns>
        public Pixel[,] HistogrammeRVB()
        {
            Console.WriteLine("Choisissez l'histogramme que vous voulez. Tapez : \n 1 pour le rouge. \n 2 pour le vert. \n 3 pour le bleu. ");
            string choix = Convert.ToString(Console.ReadLine());
            int max_r = 0;
            int max_v = 0;
            int max_b = 0;
            int[] TabRouge = new int[256];
            int[] TabBleu = new int[256];
            int[] TabVert = new int[256];
            int valeurRouge = 0;
            int valeurVert = 0;
            int valeurBleu = 0;

            for (int i = 0; i < 256; i++)
            {
                TabRouge[i] = 0;
                TabVert[i] = 0;
                TabBleu[i] = 0;
            }

            for (int i = 0; i < image.GetLength(0); i++)
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    valeurRouge = image[i, j].GetR;
                    valeurVert = image[i, j].GetV;
                    valeurBleu = image[i, j].GetB;
                    TabRouge[valeurRouge] = TabRouge[valeurRouge] + 1;
                    TabBleu[valeurBleu] = TabBleu[valeurBleu] + 1;
                    TabVert[valeurVert] = TabVert[valeurVert] + 1;
                }

            for (int i = 0; i < 256; i++)
            {
                if (TabRouge[i] > max_r)
                    max_r = TabRouge[i];
                if (TabBleu[i] > max_b)
                    max_b = TabBleu[i];
                if (TabVert[i] > max_v)
                    max_v = TabVert[i];
            }
            int max = 0;
            if (choix == "1")
            {
                max = max_r;
            }
            if (choix == "2")
            {
                max = max_v;
            }
            if (choix == "3")
            {
                max = max_b;
            }

            Pixel[,] Histo = new Pixel[(max/10 + 1), 256];
            //on initialise en blanc
            for (int i = 0; i < max/10 + 1; i++)
                for (int j = 0; j < 256; j++)
                {
                    Histo[i, j] = new Pixel(255, 255, 255);
                }

            switch(choix)
            {
                case "1"://histo rouge
                    for (int i = 0; i < 256; i++)
                    {
                        int Compteur = TabRouge[i]/10;
                        for (int x = 0; x < Compteur; x++)
                            Histo[x, i] = new Pixel(255, 0, 0);
                    }
                    break;
                case "2"://histo vert
                    for (int i = 0; i < 256; i++)
                    {
                        int Compteur = TabVert[i]/10;
                        for (int x = 0; x < Compteur; x++)
                            Histo[x, i] = new Pixel(0, 255, 0);
                    }
                    break;
                case "3"://histo bleu
                    for (int i = 0; i < 256; i++)
                    {
                        //on divise par 10 pour pas avoir un histogramme trop grand
                        int Compteur = TabBleu[i]/10;
                        for (int x = 0; x < Compteur; x++)
                            Histo[x, i] = new Pixel(0, 0, 255);
                    }
                    break;
            }
            return Histo;
        }
        /// <summary>
        /// Cette fonction crée une image façon Andy Warhol. En dupliquant l'image 4 fois dans les coins et en la faisant passer dans un filtre popArt.
        /// </summary>
        /// <returns>
        /// Elle retourne une matrice de pixel contenant 4 fois l'image choisi par l'utilisateur dans des styles différents.
        /// </returns>
        public Pixel[,] Innovation()
        {
            Pixel[,] temp = new Pixel[image.GetLength(0) * 2, image.GetLength(1) * 2];
            for (int i = 0; i < image.GetLength(0); i++)
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    temp[i, j] = image[i, j].PopArt();
                }
            for (int i = image.GetLength(0); i < temp.GetLength(0); i++)
                for (int j = image.GetLength(1); j < temp.GetLength(1); j++)
                {
                    temp[i, j] = image[i - image.GetLength(0), j - image.GetLength(1)].PopArt_2();
                }
            for (int i = 0; i < image.GetLength(0); i++)
                for (int j = image.GetLength(1); j < temp.GetLength(1); j++)
                {
                    temp[i, j] = image[i, j - image.GetLength(1)].PopArt_3();
                }
            for (int i = image.GetLength(0); i < temp.GetLength(0); i++)
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    temp[i, j] = image[i - image.GetLength(0), j].PopArt_4();
                }
            return temp;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Programme de modification d'une image";
            Console.SetWindowSize(100, 65);
            //1ère boucle dans laquelle l'utilisateur peut choisir l'image qu'il souhaite traiter (il pourra revenir ici autant de fois qu'il le désire).
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Bienvenue dans le programme de modification d'une image.\n"
                                + "Veuillez entrez le nom du fichier -coco par exemple- sans l'extension .bmp) ou tapez enter pour sortir ==>");
                //il suffit de taper le nom du fichier sans l'extension .bmp, à condition que le fichier soit bel et bien dans le fichier bin,debug.
                string name = Convert.ToString(Console.ReadLine());
                //Exit program
                if(name=="")
                {
                    break;
                }
                //2ème boucle dans laquelle l'utilisateur peut choisir le traitement à effectuer autant de fois qu'il le désire. Pour sortir de cette boucle, il suffit d'appuyer sur enter.
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Que souhaitez-vous faire ?\n"
                                     + " 0 : Pop Art\n"
                                     + " 1 : Rotation à 90°\n"
                                     + " 2 : Rotation à 180°\n"
                                     + " 3 : Rotation à 270°\n"
                                     + " 4 : Noir et Blanc\n"
                                     + " 5 : Niveau de Gris\n"
                                     + " 6 : Agrandissement\n"
                                     + " 7 : Miroir\n"
                                     + " 8 : Création d'une fractale\n"
                                     + " 9 : Rétrécissement\n"
                                     + " 10 : Convolution\n"
                                     + " 11 : Histogramme\n"
                                     + " 12 : Innovation Andy Warhol\n"
                                     + "\n"
                                     + "Sélectionnez la modification désirée ou tapez Enter pour sortir => ");
                    MyImage image = new MyImage(name + ".bmp");
                    Pixel[,] imageSortie = null;
                    string choix = "0";
                    choix = Convert.ToString(Console.ReadLine());
                    //Exit image en cours
                    if (choix == "")
                    {
                        break;
                    }
                    switch (choix)
                    {
                        case "0":
                            imageSortie = image.PopArt();
                            break;
                        case "1":
                            imageSortie = image.Rotation90();
                            break;
                        case "2":
                            imageSortie = image.Rotation180();
                            break;
                        case "3":
                            imageSortie = image.Rotation270();
                            break;
                        case "4":
                            imageSortie = image.NoirEtBlanc();
                            break;
                        case "5":
                            imageSortie = image.NiveauDeGris();
                            break;
                        case "6":
                            imageSortie = image.Agrandissement();
                            break;
                        case "7":
                            imageSortie = image.Miroir();
                            break;
                        case "8":
                            imageSortie = image.Fractale();
                            break;
                        case "9":
                            imageSortie = image.Rétrécissement();
                            break;
                        case "10":
                            imageSortie = image.Convolution();
                            break;
                        case "11":
                            imageSortie = image.HistogrammeRVB();
                            break;
                        case "12":
                            imageSortie = image.Innovation();
                            break;
                        default:
                            break;
                    }//end switch
                    //Conversion de l'image en fichier "sortie.png".
                    image.From_Image_To_File(image.MatricePixelToByte(imageSortie), image.NewHeader(imageSortie.GetLength(0), imageSortie.GetLength(1)), "sortie.png");
                    //Affichage
                    Process.Start("sortie.png");
                }//end while
            }//end while          
        }//end main
    }//end class program
}//end namespace
