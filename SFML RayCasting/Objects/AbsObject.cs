using SFML.Graphics;
using SFML.System;
using SFML_RayCasting.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Objects
{
    public abstract class AbsObject
    {
        public AbsObject(string Name, Vector2f pos, SFML.Graphics.Color color, float SizeWall = 1, bool IsGlass = false, float zIndex = 1)
        {
            this.Name = Name;
            this.Position = pos;
            this.IsGlass = IsGlass;
            this.Color = color;

            this.zIndex = zIndex;

            this.SizeWall = SizeWall;


			// Создание изображения нужного размера
			SFML.Graphics.Image image = new SFML.Graphics.Image(400, 400);

			// Заполнение изображения указанным цветом
			for (uint x = 0; x < 400; x++)
			{
				for (uint y = 0; y < 400; y++)
				{
					image.SetPixel(x, y, color);
				}
			}

			// Создание текстуры из изображения
			texture = new Texture(image);

			var sz = texture.Size;
			distPyWidhtTexture = 50 * SizeWall * ((float)sz.X * (float)sz.Y) / ((float)sz.Y * (float)sz.Y);
		}
        public AbsObject(string Name, Vector2f pos, string pathToTexture, float SizeWall = 1, bool IsGlass = false, float zIndex = 1)
        {
            this.Name = Name;
            this.Position = pos;
            this.IsGlass = IsGlass;
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathToTexture);

            Vector2u sz;

            if (Path.GetExtension(fullPath).ToLower() == ".gif")
            {
                sz = LoadGif(fullPath);
            }
            else
            {
                texture = new Texture(fullPath);
                sz = texture.Size;
            }
            this.zIndex = zIndex;


            distPyWidhtTexture = 50 * SizeWall * ((float)sz.X * (float)sz.Y) / ((float)sz.Y * (float)sz.Y);

            this.SizeWall = SizeWall;
        }
        public SFML.Graphics.Color Color;
        public float distPyWidhtTexture = 75.0f;
        public float SizeWall = 1;

        public float zIndex = 0;

        public bool isCollision = true;

        public bool IsGlass = false;
        public string Name { get; }
        public Vector2f Position { get; set; }
        public abstract List<Collision> CheckColision(Ray ray, float zIndex);
        public abstract void Draw();
        protected string pathToTexture;
        protected SFML.Graphics.Texture texture;
        public abstract Sprite GetSegment(Collision collision, float widht, float setUp, float setDown, float wallHeight);


        public static VertexObject InstanceCircule(string Name, Vector2f pos, int Points, float radius, SFML.Graphics.Color color, float SizeWall, bool IsGlass = false)
        {
            VertexObject circle = new VertexObject(Name, pos, color, SizeWall, IsGlass);

            // Число точек для аппроксимации круга
            int numPoints = Points;

            // Добавляем точки в форме круга
            for (int i = 0; i < numPoints; i++)
            {
                float angle = (float)i / numPoints * 2 * MathF.PI;
                float x = radius * MathF.Cos(angle);
                float y = radius * MathF.Sin(angle);
                circle.AddRelativePoint(new Vector2f(x, y));
            }

            // Соединяем точки линиями
            for (int i = 0; i < numPoints; i++)
            {
                int nextIndex = (i + 1) % numPoints;
                circle.AddConnection(i, nextIndex);
            }

            // Добавляем круг в коллекцию объектов
            return circle;
        }
        public static VertexObject InstanceCircule(string Name, Vector2f pos, int Points, float radius, string texture, float SizeWall = 1, bool IsGlass = false)
        {
            VertexObject circle = new VertexObject(Name, pos, texture, SizeWall, IsGlass);

            // Число точек для аппроксимации круга
            int numPoints = Points;

            // Добавляем точки в форме круга
            for (int i = 0; i < numPoints; i++)
            {
                float angle = (float)i / numPoints * 2 * MathF.PI;
                float x = radius * MathF.Cos(angle);
                float y = radius * MathF.Sin(angle);
                circle.AddRelativePoint(new Vector2f(x, y));
            }

            // Соединяем точки линиями
            for (int i = 0; i < numPoints; i++)
            {
                int nextIndex = (i + 1) % numPoints;
                circle.AddConnection(i, nextIndex);
            }

            // Добавляем круг в коллекцию объектов
            return circle;
        }

        public virtual void Update(Vector2f pos, float deltaTime, MapDef map)
        {
            if (isAnimated && animationFrames.Count > 0)
            {
                frameTimer += deltaTime;

                if (frameTimer >= frameTime)
                {
                    frameTimer = 0f;
                    currentFrame = (currentFrame + 1) % animationFrames.Count;
                    texture = animationFrames[currentFrame];
                }
            }
        }

        protected List<Texture> animationFrames;
        protected int currentFrame = 0;
        protected float frameTime = 0.1f; // время кадра
        protected float frameTimer = 0f;
        protected bool isAnimated = false;

        private Vector2u LoadGif(string path)
        {
            animationFrames = new List<Texture>();

            Vector2u sz = new();

            using (System.Drawing.Image gifImg = System.Drawing.Image.FromFile(path))
            {
                var dimension = new System.Drawing.Imaging.FrameDimension(gifImg.FrameDimensionsList[0]);
                int frameCount = gifImg.GetFrameCount(dimension);

                for (int i = 0; i < frameCount; i++)
                {
                    gifImg.SelectActiveFrame(dimension, i);

                    using (Bitmap bmp = new Bitmap(gifImg))
                    {
                        var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                        var data = bmp.LockBits(
                            rect,
                            System.Drawing.Imaging.ImageLockMode.ReadOnly,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                        int byteCount = data.Stride * data.Height;
                        byte[] pixels = new byte[byteCount];

                        System.Runtime.InteropServices.Marshal.Copy(
                            data.Scan0,
                            pixels,
                            0,
                            byteCount);

                        bmp.UnlockBits(data);

                        Texture tex = new Texture((uint)bmp.Width, (uint)bmp.Height);
                        tex.Update(pixels);

                        sz = tex.Size;

                        animationFrames.Add(tex);
                    }
                }
            }

            texture = animationFrames[0];
            isAnimated = true;
            return sz;
        }

    }
}
