using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class SnakePrint
    {
        private Texture2D texture;
        private int width;
        private int height;
        private float scale;
        

        public SnakePrint(Texture2D texture, int width, int height, float scale) {
            this.texture = texture;
            this.width = width;
            this.height = height;
            this.scale = scale;
        }

        public void Draw (Vector2 position, int what, int type, SpriteBatch spriteBatch) {
            int x = 1;
            int y = 1;
            if (what == 1)
            {
                switch (type)
                {
                    case 1:
                        x = 0;
                        y = 1;
                        break;
                    case 2:
                        x = 0;
                        y = 0;
                        break;
                    case 3:
                        x = 1;
                        y = 0;
                        break;
                    case 4:
                        x = 2;
                        y = 0;
                        break;
                    case 5:
                        x = 2;
                        y = 1;
                        break;
                    case 6:
                        x = 2;
                        y = 2;
                        break;
                    default:
                        x = 0;
                        y = 0;
                        break;
                }
            }
            else if (what == 2)
            {
                switch (type)
                {
                    case 1:
                        x = 3;
                        y = 0;
                        break;
                    case 2:
                        x = 4;
                        y = 0;
                        break;
                    case 3:
                        x = 3;
                        y = 1;
                        break;
                    case 4:
                        x = 4;
                        y = 1;
                        break;
                    default:
                        x = 0;
                        y = 0;
                        break;
                }
            }
            else if (what == 3)
            {
                switch (type)
                {
                    case 1:
                        x = 3;
                        y = 2;
                        break;
                    case 2:
                        x = 4;
                        y = 2;
                        break;
                    case 3:
                        x = 3;
                        y = 3;
                        break;
                    case 4:
                        x = 4;
                        y = 3;
                        break;
                    default:
                        x = 0;
                        y = 0;
                        break;
                }
            }
           else if (what == 4)
            {
                x = 0;
                y = 3;
            }
            else {
                x = 1;
                y = 1;
            }
            x *= width;
            y *= height;
            spriteBatch.Draw(texture, position, new Rectangle(x, y, width, height), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
