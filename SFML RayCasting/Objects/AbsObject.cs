using SFML.Graphics;
using SFML.System;
using SFML_RayCasting.Maps;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Gif;



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


        public List<Vector2f> Points = new List<Vector2f>();
        public List<(Vector2f, Vector2f)> Connections { get; set; }
        public Dictionary<Vector2f, float> textureIndex = new Dictionary<Vector2f, float>();
        public void AddRelativePoint(Vector2f relativePoint)
        {
            Vector2f absolutePoint = Position + relativePoint;
            Points.Add(absolutePoint);
        }
        public void AddConnection(int index1, int index2)
        {
            if (index1 >= 0 && index1 < Points.Count &&
                index2 >= 0 && index2 < Points.Count)
            {
                Connections.Add((Points[index1], Points[index2]));
                CreateIndexTexture(index1, index2);
            }
            else
            {
                throw new IndexOutOfRangeException("Point indices are out of range.");
            }
        }

        protected void CreateIndexTexture(int index1, int index2)
        {
            if (texture == null) return;

            var pos1 = Points[index1];
            var pos2 = Points[index2];

            float distance = MathUtils.Distance(pos1, pos2);

            if (textureIndex.Count == 0)
            {
                textureIndex.Add(pos1, 0);
            }

            float textureWidth = (float)texture.Size.X;
            float index = distance / distPyWidhtTexture; // Используем N единиц расстояния как одну ширину текстуры

            textureIndex.TryAdd(pos2, textureIndex.Last().Value + index);
        }

        protected void RebuildTextureIndex()
        {
            if (texture == null || Points.Count < 2)
                return;

            textureIndex.Clear();

            for (int i = 0; i < Points.Count; i++)
            {
                int next = (i + 1) % Points.Count;
                CreateIndexTexture(i, next);
            }
        }

        public static VertexObject InstanceCircule(string Name, Vector2f pos, int Points, float radius, SFML.Graphics.Color color, float SizeWall, bool IsGlass = false)
        {
            VertexObject circle = new VertexObject(Name, pos, color, SizeWall, IsGlass);

            circle.InstantCircule(Points, radius);

            return circle;
        }

        public void InstantCircule(int Points, float radius)
        {
            var circle = this;
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
        }
        public static VertexObject InstanceCircule(string Name, Vector2f pos, int Points, float radius, string texture, float SizeWall = 1, bool IsGlass = false)
        {
            VertexObject circle = new VertexObject(Name, pos, texture, SizeWall, IsGlass);

            circle.InstantCircule(Points, radius);

            return circle;
        }

        public virtual void Update(Vector2f pos, float deltaTime, MapDef map)
        {
            if (isAnimated && animationFrames.Count > 0)
            {
                frameTimer += deltaTime* 2;

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

            using (Image<Rgba32> gif = SixLabors.ImageSharp.Image.Load<Rgba32>(path))
            {
                int frameCount = gif.Frames.Count;

                for (int i = 0; i < frameCount; i++)
                {
                    using (Image<Rgba32> frameImage = gif.Frames.CloneFrame(i))
                    {
                        int width = frameImage.Width;
                        int height = frameImage.Height;

                        byte[] pixels = new byte[width * height * 4];

                        frameImage.CopyPixelDataTo(pixels);

                        Texture tex = new Texture((uint)width, (uint)height);
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
