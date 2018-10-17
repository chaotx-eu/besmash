namespace RougeLikeDemo {
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;

    using System;
    using System.Collections.Generic;

    public class Map {
        public string SpriteSheetPath {get;}
        public Texture2D SpriteSheet {get; set;}
        public SpriteBatch SpriteBatch {get; set;}
        public MapComponent[][] Components {get;}
        public List<Entity> Entities {get;}

        public int X {get; set;}
        public int Y {get; set;}
        public float Scale {get; set;}

        public Map(string spriteSheetPath, MapComponent[][] components) {
            Entities = new List<Entity>();
            SpriteSheetPath = spriteSheetPath;
            Components = components;

            // test values
            Scale = 2.5f;
        }

        public void Load(Game game) {
            SpriteBatch = new SpriteBatch(game.GraphicsDevice);
            SpriteSheet = game.Content.Load<Texture2D>(SpriteSheetPath);

            int x = 0, y = 0, m = 0;
            foreach(MapComponent[] row in Components) {
                foreach(MapComponent component in row) {
                    if(component != null) {
                        component.load(game.Content); // does nothing atm
                        component.SpriteBatch = SpriteBatch;
                        component.SpriteSheet = SpriteSheet;
                        component.X += x;
                        component.Y += y;

                        x += component.SpriteRectangle.Width;
                        if(m < component.SpriteRectangle.Height) m = component.SpriteRectangle.Height;
                    }
                }

                y += m;
                x = m = 0;
            }

            foreach(Entity e in Entities) {
                e.load(game.Content);
                e.SpriteBatch = SpriteBatch;
            }
        }

        /**
        * Adds entity to map.
        **/
        public void addEntity(Entity e) {
            e.AciveMap = this;
            Entities.Add(e);
        }

        /**
        * Remove entity from map.
        **/
        public void removeEntity(Entity e) {
            Entities.Remove(e);
        }

        public bool moveEntity(Entity e, Entity.Direction d) {
            int newX = e.MapX + (d == Entity.Direction.RIGHT ? 1 : d == Entity.Direction.LEFT ? -1 : 0);
            int newY = e.MapY + (d == Entity.Direction.DOWN ? 1 : d == Entity.Direction.UP ? -1 : 0);

            try {
                if(Components[newY][newX].Solid)
                    return false;

                e.MapX = newX;
                e.MapY = newY;
                return true;
            } catch(Exception ex) {
                if(ex is IndexOutOfRangeException || ex is NullReferenceException)
                    return false;

                throw ex;
            }
        }

        /**
        * Updates all Entities on this map.
        **/
        public void update(GameTime gameTime) {
            foreach(Entity e in Entities)
                e.update(gameTime);
        }

        /**
        * Draws all Game Objects on this map.
        **/
        public void draw() {
            // https://gamedev.stackexchange.com/questions/6820/how-do-i-disable-texture-filtering-for-sprite-scaling-in-xna-4-0
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            foreach(MapComponent[] row in Components)
                foreach(MapComponent component in row)
                    if(component != null) component.draw(X, Y, Scale);

            foreach(Entity e in Entities)
                e.draw(X, Y, Scale);

            SpriteBatch.End();
        }
    }
}