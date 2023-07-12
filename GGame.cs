using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelPalette
{
    internal class GGame : Game
    {
        internal GraphicsDeviceManager graphicsDeviceManager;
        internal SpriteBatch spriteBatch;

        public GGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Position = new Point(-graphicsDeviceManager.PreferredBackBufferWidth, -graphicsDeviceManager.PreferredBackBufferHeight);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Exit();
        }

        internal void start()
        {
            Console.WriteLine("Starting...");

            Dictionary<string, Texture2D> dictionary = loadImageTextures("pallet", GraphicsDevice);
            Console.WriteLine($"Found files in pallet folder: {dictionary.Keys.Count}");

            List<Color> colors = new();

            foreach (string key in dictionary.Keys)
            {
                Console.WriteLine($"Loading pallet: {key}");

                Texture2D palletTexture = dictionary[key];
                Color[] data = new Color[palletTexture.Width * palletTexture.Height];
                palletTexture.GetData(data);

                for (int i = 0; i < data.Length; i++)
                {
                    Color color = data[i];

                    if (!colors.Contains(color))
                    {
                        colors.Add(color);
                    }
                }
            }

            Console.WriteLine($"Loaded {colors.Count} colors");

            if (dictionary.Keys.Count > 0)
            {
                dictionary = loadImageTextures("input", GraphicsDevice);
                Console.WriteLine($"Found files in input folder: {dictionary.Keys.Count}");

                foreach (string key in dictionary.Keys)
                {
                    Console.WriteLine($"Converting file: {key}");
                    Texture2D inputTexture = dictionary[key];
                    int changeCount = inputTexture.convert(colors);
                    Console.WriteLine($"Pixel change count: {changeCount}");
                }

                if (dictionary.Keys.Count > 0)
                {
                    Console.WriteLine("Saving files");
                    saveConvertedTextures(dictionary, "output");
                    Console.WriteLine("Finished");
                }
            }
        }

        void saveConvertedTextures(Dictionary<string, Texture2D> convertedTextures, string outputFolderPath)
        {
            Directory.CreateDirectory(outputFolderPath);

            foreach (KeyValuePair<string, Texture2D> keyValuePair in convertedTextures)
            {
                string imagePath = keyValuePair.Key;
                Texture2D texture = keyValuePair.Value;

                string fileName = Path.GetFileName(imagePath);

                string outputPath = Path.Combine(outputFolderPath, fileName);

                using (FileStream fileStream = new(outputPath, FileMode.Create))
                {
                    texture.SaveAsPng(fileStream, texture.Width, texture.Height);
                }
            }
        }

        Dictionary<string, Texture2D> loadImageTextures(string relativeFolderPath, GraphicsDevice graphicsDevice)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string absoluteFolderPath = Path.Combine(baseDirectory, relativeFolderPath);
            List<string> imageFilePaths = getImageFilePaths(absoluteFolderPath);
            Dictionary<string, Texture2D> imageTextures = new();

            foreach (string imagePath in imageFilePaths)
            {
                using (FileStream fileStream = new(imagePath, FileMode.Open))
                {
                    Texture2D texture = Texture2D.FromStream(graphicsDevice, fileStream);
                    imageTextures.Add(imagePath, texture);
                }
            }

            return imageTextures;
        }

        List<string> getImageFilePaths(string relativeFolderPath)
        {
            string executableFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(executableFolderPath, relativeFolderPath);
            List<string> imageFilePaths = new();

            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);

                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file).ToLower();

                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".bmp")
                    {
                        imageFilePaths.Add(file);
                    }
                }
            }

            return imageFilePaths;
        }
    }
}
