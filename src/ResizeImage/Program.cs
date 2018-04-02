using System;
using System.IO;

namespace ResizeImage
{
    public class Program
    {
        public static string ver = "v20180402";
        static string sourcePath = @"C:\\Photos\Source\";
        static string destinationPath = @"C:\\Photos\Resize\";
        static string processedPath = @"C:\\Photos\Processed\";
        static int resizeWidthPixel = 2000;
        static int resizeHeightPixel = 2000;
        static void Main(string[] args)
        {
            Console.Title = "Resize Image - " + ver;

            string[] filePaths = Directory.GetFiles(sourcePath, "*.jpg");

            string[] directories = new string[] { sourcePath, destinationPath, processedPath };

            foreach (string dir in directories)
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }

            if(args.Length > 1)
            {
                if(args[0].Contains("square"))
                {
                    int number;

                    bool result = Int32.TryParse(args[1], out number);
                    if (result)
                    {
                        resizeWidthPixel = resizeHeightPixel = Convert.ToInt32(args[1]);
                    }
                }
            }

            try
            {
                if (filePaths.Length > 0)
                {
                    foreach (string imgPath in filePaths)
                    {
                        string fileName = Path.GetFileName(imgPath);

                        string newFileName = fileName + "_" + resizeWidthPixel + "_" + resizeHeightPixel + ".jpg";

                        Imager.PerformImageResizeAndPutOnCanvas(sourcePath, fileName, resizeWidthPixel, resizeHeightPixel, newFileName, destinationPath);

                        MoveFile(imgPath, processedPath + fileName);
                        
                        Console.WriteLine(fileName + " - Resized");
                    }
                    Console.WriteLine("All images in " + sourcePath + " are resized and save in " + destinationPath + " successfully!");
                }
                else
                {
                    Console.WriteLine("No images found!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            Console.WriteLine("Press any key to EXIT program.");
            Console.ReadLine();
        }

        private static async void MoveFile(string sourceFile, string destinationFile)
        {
            try
            {
                using (FileStream sourceStream = File.Open(sourceFile, FileMode.Open))
                {
                    using (FileStream destinationStream = File.Create(destinationFile))
                    {
                        await sourceStream.CopyToAsync(destinationStream);

                        sourceStream.Close();
                        File.Delete(sourceFile);
                    }
                }
            }
            catch (IOException ioex)
            {
                Console.WriteLine("An IOException occured during move, " + ioex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Exception occured during move, " + ex.Message);
            }
        }
    }
}
